using WeedDatabase.Domain;
using WeedDatabase.Domain.App;

namespace WeedDatabase.Repositories;

public interface IMarketCustomerOrdersRepository
{
    Task<bool> TryToPlace(Order order);
}