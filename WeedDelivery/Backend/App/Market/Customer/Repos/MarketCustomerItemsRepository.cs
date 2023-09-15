using Microsoft.EntityFrameworkCore;
using WeedDatabase.Context.Interfaces;
using WeedDatabase.Domain;
using WeedDatabase.Domain.App;
using WeedDatabase.Repositories;

namespace WeedDelivery.Backend.App.Market.Customer.Repos;

public class MarketCustomerItemsRepository : IMarketCustomerItemsRepository
{
    private readonly IWeedContextAcceptor _contextAcceptor;

    public MarketCustomerItemsRepository(IWeedContextAcceptor contextAcceptor)
    {
        _contextAcceptor = contextAcceptor;
    }
    
    public async Task<List<WeedItem>> GetAllByQuery()
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var allWeedRequested = await dbCtx.Weed.ToListAsync();

        return allWeedRequested;
    }
}