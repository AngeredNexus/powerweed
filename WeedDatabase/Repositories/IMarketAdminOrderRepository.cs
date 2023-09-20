using WeedDatabase.Domain;
using WeedDatabase.Domain.App;
using WeedDatabase.Domain.App.Types;

namespace WeedDatabase.Repositories;

public interface IMarketAdminOrderRepository
{
    Task<List<Order>> GetAll(bool isHistoricMode = false);

    Task Update(Order item);
    
    Task Remove(Guid id);

    Task SetStatus(Guid id, OrderStatus status);

    Task<Order?> GetOrderById(Guid id);
}