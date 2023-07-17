using WeedDatabase.Domain;
using WeedDatabase.Domain.App;

namespace WeedDelivery.Backend.App.Market.Customer.Interfaces;

public interface IMarketCustomerSearchService
{
    Task<List<WeedItem>> SearchByCategory();
}