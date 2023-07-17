using WeedDatabase.Domain;
using WeedDatabase.Domain.App;

namespace WeedDatabase.Repositories;

public interface IMarketAdminItemsRepository
{
    Task<List<WeedItem>> GetAll(bool isHistoricMode = false);

    Task Update(WeedItem item);

    Task Remove(Guid id);
}