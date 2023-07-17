using WeedDatabase.Domain;
using WeedDatabase.Domain.App;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.App.Market.Customer.Interfaces;
using WeedDelivery.Backend.Models.App.Entities;

namespace WeedDelivery.Backend.App.Market.Customer.Services;

public class MarketCustomerOrderService : IMarketCustomerOrderService
{

    private readonly IMarketCustomerOrdersRepository _repo;

    public MarketCustomerOrderService(IMarketCustomerOrdersRepository repo)
    {
        _repo = repo;
    }

    public async Task<CustomerOrderAcceptor> TryToPlaceOrder(Order orderToPlace)
    {
        var isSuccsessed = await _repo.TryToPlace(orderToPlace);

        var insertionResult = new CustomerOrderAcceptor()
        {
            IsSuccessful = isSuccsessed,
            OrderId = orderToPlace.Id
        };

        return insertionResult;
    }
}