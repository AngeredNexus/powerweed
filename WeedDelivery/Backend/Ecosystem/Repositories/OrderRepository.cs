using Database.Context.Interfaces;
using Database.Domain;
using Database.Repositories;

namespace WeedDelivery.Backend.Ecosystem.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IProductDbContextAcceptor _contextAcceptor;

    public OrderRepository(IProductDbContextAcceptor contextAcceptor)
    {
        _contextAcceptor = contextAcceptor;
    }

    public async Task<bool> WriteUserOrder(UserOrder userOrder, List<ProductOrder> productOrders)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();
        
        dbCtx.UserOrders.Add(userOrder);
        
        productOrders.ForEach(x => x.OrderId = userOrder.Id);
        dbCtx.ProductOrders.AddRange(productOrders);
        
        await dbCtx.SaveChangesAsync();
        return true;
    }
}