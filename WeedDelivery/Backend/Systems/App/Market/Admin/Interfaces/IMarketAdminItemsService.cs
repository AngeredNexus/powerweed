using WeedDatabase.Domain.App;

namespace WeedDelivery.Backend.Systems.App.Market.Admin.Interfaces;

public interface IMarketAdminItemsService
{
    Task<List<WeedItem>> GetAllItems(bool isHistoric);

    Task Edit(WeedItem item);
    
    Task Remove(Guid id);
}