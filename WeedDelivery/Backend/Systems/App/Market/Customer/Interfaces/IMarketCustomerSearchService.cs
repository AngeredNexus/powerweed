using WeedDatabase.Domain.App;

namespace WeedDelivery.Backend.Systems.App.Market.Customer.Interfaces;

public interface IMarketCustomerSearchService
{
    Task<List<WeedItem>> SearchByCategory();
}