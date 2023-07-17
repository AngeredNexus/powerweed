using WeedDelivery.Backend.App.Market.Customer.Interfaces;
using WeedDelivery.Backend.Models.App.Entities;

namespace WeedDelivery.Backend.App.Market.Customer.Services;

public class MarketCustomerCategoryService : IMarketCustomerCategoryService
{
    public Task<List<WeedCategoryView>> GetCategoryTree()
    {
        throw new NotImplementedException();
    }
}