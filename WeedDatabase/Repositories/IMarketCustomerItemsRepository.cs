using WeedDatabase.Domain;
using WeedDatabase.Domain.App;

namespace WeedDatabase.Repositories;

public interface IMarketCustomerItemsRepository
{
    Task<List<WeedItem>> GetAllByQuery();
}