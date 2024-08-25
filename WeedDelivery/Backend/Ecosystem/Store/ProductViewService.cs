using Database.Domain;
using Database.Repositories;

namespace WeedDelivery.Backend.Ecosystem.Store;

public class ProductViewService : IProductViewService
{
    
    private readonly IProductRepository _productRepository;

    public ProductViewService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetProductsAsync(Object filter = null!)
    {
        return (await _productRepository.GetAll()).ToList();
    }
}