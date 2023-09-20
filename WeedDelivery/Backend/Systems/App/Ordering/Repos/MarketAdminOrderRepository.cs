using Microsoft.EntityFrameworkCore;
using WeedDatabase.Context.Interfaces;
using WeedDatabase.Domain.App;
using WeedDatabase.Domain.App.Types;
using WeedDatabase.Repositories;

namespace WeedDelivery.Backend.Systems.App.Ordering.Repos;

public class MarketAdminOrderRepository : IMarketAdminOrderRepository
{

    private readonly IWeedContextAcceptor _contextAcceptor;

    public MarketAdminOrderRepository(IWeedContextAcceptor contextAcceptor)
    {
        _contextAcceptor = contextAcceptor;
    }

    public async Task<List<Order>> GetAll(bool isHistoricMode = false)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        List<Order>? allWeedRequested;
        
        if (isHistoricMode)
        {
            allWeedRequested = await dbCtx.Orders.Where(x => x.Status == OrderStatus.Ready).ToListAsync();
        }
        else
        {
            allWeedRequested = await dbCtx.Orders.Where(x => x.Status != OrderStatus.Ready).ToListAsync();
        }
        
        return allWeedRequested;
    }

    public async Task Update(Order item)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        dbCtx.Orders.Attach(item);
        await dbCtx.SaveChangesAsync();
    }
    
    public async Task Remove(Guid id)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var removeSelectorQuery = dbCtx.Orders.Where(x => x.Id == id);
        dbCtx.Remove(removeSelectorQuery); 
        
        await dbCtx.SaveChangesAsync();
    }

    public async Task SetStatus(Guid id, OrderStatus status)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var order = await dbCtx.Orders.FirstOrDefaultAsync(x => x.Id == id);
        
        if(order is not null)
            order.Status = status;

        await dbCtx.SaveChangesAsync();
    }

    public async Task<Order?> GetOrderById(Guid id)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var order = await dbCtx.Orders.FirstOrDefaultAsync(x => x.Id == id);
        
        return order;
    }
}