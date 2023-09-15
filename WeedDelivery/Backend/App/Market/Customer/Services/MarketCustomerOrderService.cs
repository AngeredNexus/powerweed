using WeedDatabase.Domain;
using WeedDatabase.Domain.App;
using WeedDatabase.Domain.Common;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.App.Bots;
using WeedDelivery.Backend.App.Market.Customer.Interfaces;
using WeedDelivery.Backend.Models.Api.Bots;
using WeedDelivery.Backend.Models.Api.Common;
using WeedDelivery.Backend.Models.App.Entities;

namespace WeedDelivery.Backend.App.Market.Customer.Services;

public class MarketCustomerOrderService : IMarketCustomerOrderService
{

    private readonly IMarketCustomerOrdersRepository _repo;
    private readonly IMarketAdminItemsRepository _adminItemsRepository;
    private readonly IApplicationTelegramBotService _applicationTelegramBotService;

    public MarketCustomerOrderService(IMarketCustomerOrdersRepository repo, IApplicationTelegramBotService applicationTelegramBotService, IMarketAdminItemsRepository adminItemsRepository)
    {
        _repo = repo;
        _applicationTelegramBotService = applicationTelegramBotService;
        _adminItemsRepository = adminItemsRepository;
    }

    public async Task<CustomerOrderAcceptor> TryToPlaceOrder(Order orderToPlace, SmokiUser userData)
    {

        var weeds = await _adminItemsRepository.GetAll();
        var weedPerId = weeds.ToDictionary(x => x.Id, y => y);

        orderToPlace.Items.ForEach(x => x.Name = weedPerId.TryGetValue(x.WeedId, out var weed) ? weed.Name : "Ошибка получения имени!");
        
        var oItems = orderToPlace.Items.Select(x => new OrderItemView()
        {
            Name = x.Name,
            Amount = x.Amount,
            WeedId = x.WeedId,
            Price = weedPerId.TryGetValue(x.WeedId, out var weed) ? weed.Price : 400,
            HasDiscount = weed?.HasDiscount ?? true,
            DiscountGradeStep = weed?.DiscountStep ?? 50
        }).ToList();
        
        var isSuccsessed = await _repo.TryToPlace(orderToPlace);

        var insertionResult = new CustomerOrderAcceptor()
        {
            IsSuccessful = isSuccsessed,
            OrderId = orderToPlace.Id
        };

        if (isSuccsessed)
        {
            var orderNotificationObject = new BotOrderNotification()
            {
                Id = orderToPlace.Id,
                Items = oItems,
                Address = orderToPlace.Address,
                Firstname = orderToPlace.Firstname,
                Lastname = orderToPlace.Lastname,
                Comment = orderToPlace.Comment,
                Status = orderToPlace.Status,
                PhoneNumber = orderToPlace.PhoneNumber,
                DeliveryMan = orderToPlace.DeliveryMan
            };
            
            await _applicationTelegramBotService.NotifyAboutOrder(orderNotificationObject, userData);
        }
        
        return insertionResult;
    }
}