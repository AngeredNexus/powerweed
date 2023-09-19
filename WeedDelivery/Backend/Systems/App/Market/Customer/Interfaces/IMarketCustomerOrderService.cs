using WeedDatabase.Domain.App;
using WeedDatabase.Domain.Common;
using WeedDelivery.Backend.Models.App.Entities;

namespace WeedDelivery.Backend.Systems.App.Market.Customer.Interfaces;

public interface IMarketCustomerOrderService
{
    Task<CustomerOrderAcceptor> TryToPlaceOrder(Order orderToPlace, SmokiUser userData);
}