using Database.Domain;
using Database.Repositories;
using WeedDelivery.Backend.Ecosystem.Discount;
using WeedDelivery.Backend.Ecosystem.Notifications.Messengers;
using WeedDelivery.Backend.Models.Api.Request;

namespace WeedDelivery.Backend.Ecosystem.Store;

public class ProductOrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMessengerDataRepository _messengerDataRepository;
    private readonly IProductRepository _productRepository;
    private readonly IDiscountService _discountService;
    private readonly IMessengerNotificationService _messengerNotification;
    private readonly ILogger _logger;

    public ProductOrderService(IOrderRepository orderRepository, IProductRepository productRepository, IDiscountService discountService, IMessengerDataRepository messengerDataRepository, IMessengerNotificationService messengerNotification, ILogger<ProductOrderService> logger)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _discountService = discountService;
        _messengerDataRepository = messengerDataRepository;
        _messengerNotification = messengerNotification;
        _logger = logger;
    }

    public async Task<UserOrder> TryCreateOrder(OrderRequest order)
    {
        var items = order.Items.Select(x => new ProductOrder()
        {
            ProductId = x.Id,
            Amount = x.Amount,
        }).ToList();
        
        var products = (await _productRepository.GetFilterId(items.Select(x => x.ProductId).ToList())).ToDictionary(x => x.Id, x => x);

        var mapped = items.Select(x => new ProductWithOrderInfo()
        {
            ProductInfo = products[x.ProductId],
            OrderInfo = x,
        }).ToList();
        
        var discount = await _discountService.SetupDiscount(items);
        
        var price = discount.Sum(x => x.Original * x.Order.Amount);
        var discountedPrice = discount.Sum(x => x.Updated * x.Order.Amount);
        
        var userOrder = new UserOrder()
        {
            UserId = order.RelatedUser.Id,
            ContactId = order.RelatedUser.ContactId ?? Guid.Empty,
            ProductPrice = price,
            DiscountedProdcutPrice = discountedPrice,
            DeliveryPrice = 150,
            MapUrl = order.Address,
            Created = DateTime.Now
        };
        
        await _orderRepository.WriteUserOrder(userOrder, items);

        var contact = await _messengerDataRepository.GetAuthContactByIdAsync(order.RelatedUser.ContactId ?? Guid.Empty);

        if (contact is not null)
        {
            await _messengerNotification.SendOrderNotification(NotificationRole.Manager, userOrder, order, contact, mapped);
            await _messengerNotification.SendOrderNotification(NotificationRole.Customer, userOrder, order, contact, mapped);
        }
        else
        {
            _logger.LogWarning("Contact({CId}) for user({UsrId}) not found!", order.RelatedUser.ContactId, order.RelatedUser.Id);
        }
        
        return userOrder;
    }
}