using WeedDatabase.Domain;
using WeedDatabase.Domain.App;
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

    public async Task<CustomerOrderAcceptor> TryToPlaceOrder(Order orderToPlace, TelegramCoockie userData)
    {

        var weeds = await _adminItemsRepository.GetAll();
        var namesPerId = weeds.ToDictionary(x => x.Id, y => y.Name);
        
        orderToPlace.Items.ForEach(x =>
        {
            x.Name = namesPerId.TryGetValue(x.WeedId, out var name) ? name : "Ошибка получения имени!";
        });
        
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
                Items = orderToPlace.Items,
                Address = orderToPlace.Address,
                Firstname = orderToPlace.Firstname,
                Lastname = orderToPlace.Lastname,
                Status = orderToPlace.Status,
                PhoneNumber = orderToPlace.PhoneNumber,
                DeliveryMan = orderToPlace.DeliveryMan
            };
            
            await _applicationTelegramBotService.NotifyAboutOrder(orderNotificationObject, userData);
        }
        
        return insertionResult;
    }
}