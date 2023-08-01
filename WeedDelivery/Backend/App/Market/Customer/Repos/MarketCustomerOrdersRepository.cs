using WeedDatabase.Context.Interfaces;
using WeedDatabase.Domain;
using WeedDatabase.Domain.App;
using WeedDatabase.Repositories;

namespace WeedDelivery.Backend.App.Market.Customer.Repos;

public class MarketCustomerOrdersRepository : IMarketCustomerOrdersRepository
{
    
    private readonly IWeedContextAcceptor _contextAcceptor;

    public MarketCustomerOrdersRepository(IWeedContextAcceptor contextAcceptor)
    {
        _contextAcceptor = contextAcceptor;
    }
    
    public async Task<bool> TryToPlace(Order order)
    {
        bool isSuccess;
        
        await using var dbCtx = _contextAcceptor.CreateContext();
        var now = DateTime.Now;
        
        try
        {
            order.Created = now;
            order.Modified = now;
            order.Items.ForEach(x =>
            {
                x.Created = now;
                x.Modified = now;
            });
            
            dbCtx.Orders.Add(order);
            dbCtx.OrderItems.AddRange(order.Items);
            
            await dbCtx.SaveChangesAsync();
            isSuccess = true;
        }
        catch
        {
            isSuccess = false;
        }
        
        return isSuccess;
    }
}