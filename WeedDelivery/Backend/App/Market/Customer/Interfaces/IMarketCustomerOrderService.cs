using WeedDatabase.Domain.App;
using WeedDelivery.Backend.Models.Api.Common;
using WeedDelivery.Backend.Models.App.Entities;

namespace WeedDelivery.Backend.App.Market.Customer.Interfaces;

public interface IMarketCustomerOrderService
{
    Task<CustomerOrderAcceptor> TryToPlaceOrder(Order orderToPlace, TelegramCoockie userData);
}