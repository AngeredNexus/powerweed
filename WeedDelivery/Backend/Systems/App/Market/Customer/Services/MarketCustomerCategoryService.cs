using WeedDelivery.Backend.Models.App.Entities;
using WeedDelivery.Backend.Systems.App.Market.Customer.Interfaces;

namespace WeedDelivery.Backend.Systems.App.Market.Customer.Services;

public class MarketCustomerCategoryService : IMarketCustomerCategoryService
{
    public Task<List<WeedCategoryView>> GetCategoryTree()
    {
        throw new NotImplementedException();
    }
}