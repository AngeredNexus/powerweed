using WeedDatabase.Domain.App;

namespace WeedDelivery.Backend.Systems.App.Ordering.Interfaces;

public interface IOrderAdminService
{
    Task<List<Order>> GetAllOrders(bool isHistoric);

    Task EditOrder(Order order);

    Task RemoveOrder(Guid id);
}