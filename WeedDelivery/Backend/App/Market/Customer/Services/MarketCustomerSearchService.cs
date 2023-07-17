using WeedDatabase.Domain;
using WeedDatabase.Domain.App;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.App.Market.Customer.Interfaces;

namespace WeedDelivery.Backend.App.Market.Customer.Services;

public class MarketCustomerSearchService : IMarketCustomerSearchService
{

    private readonly IMarketCustomerItemsRepository _repo;

    public MarketCustomerSearchService(IMarketCustomerItemsRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<WeedItem>> SearchByCategory()
    {
        return await _repo.GetAllByQuery();
    }
}