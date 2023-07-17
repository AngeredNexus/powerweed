using WeedDatabase.Domain;
using WeedDatabase.Domain.App;

namespace WeedDatabase.Repositories;

public interface IMarketAdminOrderRepository
{
    Task<List<Order>> GetAll(bool isHistoricMode = false);

    Task Update(Order item);
    
    Task Remove(Guid id);
}