using WeedDatabase.Domain;
using WeedDatabase.Domain.App;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.App.Market.Admin.Interfaces;

namespace WeedDelivery.Backend.App.Market.Admin.Services;

public class MarketAdminItemsService : IMarketAdminItemsService
{
    private readonly IMarketAdminItemsRepository _repo;

    public MarketAdminItemsService(IMarketAdminItemsRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<WeedItem>> GetAllItems(bool isHistoric)
    {
        var items = await _repo.GetAll();
        return items;
    }

    public async Task Edit(WeedItem item)
    {
        await _repo.Update(item);
    }

    public async Task Remove(Guid id)
    {
        await _repo.Remove(id);
    }
}