using WeedDelivery.Backend.Models.App.Entities;

namespace WeedDelivery.Backend.Systems.App.Market.Customer.Interfaces;

public interface IMarketCustomerCategoryService
{
    Task<List<WeedCategoryView>> GetCategoryTree();
}