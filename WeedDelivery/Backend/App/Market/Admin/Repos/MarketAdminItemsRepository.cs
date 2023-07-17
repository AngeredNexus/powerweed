using Microsoft.EntityFrameworkCore;
using WeedDatabase.Context.Interfaces;
using WeedDatabase.Domain.App;
using WeedDatabase.Repositories;

namespace WeedDelivery.Backend.App.Market.Admin.Repos;

public class MarketAdminItemsRepository : IMarketAdminItemsRepository
{

    private readonly IWeedContextAcceptor _contextAcceptor;

    public MarketAdminItemsRepository(IWeedContextAcceptor contextAcceptor)
    {
        _contextAcceptor = contextAcceptor;
    }

    public async Task<List<WeedItem>> GetAll(bool isHistoricMode = false)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var allWeedRequested = await dbCtx.Weed.ToListAsync();

        return allWeedRequested;
    }

    public async Task Update(WeedItem item)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        dbCtx.Attach(item);
        await dbCtx.SaveChangesAsync();
    }
    
    public async Task Remove(Guid id)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var removeSelectorQuery = dbCtx.Weed.Where(x => x.Id == id);
        dbCtx.Remove(removeSelectorQuery);
        
        await dbCtx.SaveChangesAsync();
    }
}