using Database.Domain;

namespace WeedDelivery.Backend.Ecosystem.Store;

public interface IProductViewService
{
    Task<List<Product>> GetProductsAsync(Object filter = null!);
}