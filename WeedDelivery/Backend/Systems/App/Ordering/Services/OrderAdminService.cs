using WeedDatabase.Domain.App;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Systems.App.Ordering.Interfaces;

namespace WeedDelivery.Backend.Systems.App.Ordering.Services;

public class OrderAdminService : IOrderAdminService
{

    public readonly IMarketAdminOrderRepository _repo;

    public OrderAdminService(IMarketAdminOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<Order>> GetAllOrders(bool isHistoric)
    {
        var orders = await _repo.GetAll(isHistoric);
        return orders;
    }

    public async Task EditOrder(Order order)
    {
        await _repo.Update(order);
    }

    public async Task RemoveOrder(Guid id)
    {
        await _repo.Remove(id);
    }
}