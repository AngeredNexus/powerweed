using Database.Domain;
using Database.Repositories;

namespace WeedDelivery.Backend.Ecosystem.Store;

public class ProductAdditionalInfoService : IProductAdditionalInfoService
{
    
    private readonly IProductRepository _productRepository;

    public ProductAdditionalInfoService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductImage>> GetProductImages(Guid productId)
    {
        var images = await _productRepository.GetProductImagesBatchById(new List<Guid>(){productId});
        return images;
    }

    public async Task<Dictionary<Guid, List<ProductImage>>> GetProductImagesBatch(List<Guid> productsId)
    {
        var images = await _productRepository.GetProductImagesBatchById(productsId);
        return images.GroupBy(x => x.ProductId).ToDictionary(x => x.Key, y => y.ToList());
    }

    public async Task<List<ProductSpecification>> GetProductSpecification(Guid productId)
    {
        throw new NotImplementedException();
    }

    public async Task<Dictionary<Guid, List<ProductSpecification>>> GetProductSpecificationnBatch(List<Guid> productsId)
    {
        var images = await _productRepository.GetProductSpecificationsBatchById(productsId);
        return images.GroupBy(x => x.ProductId).ToDictionary(x => x.Key, y => y.ToList());
    }
}