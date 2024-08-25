using Database.Domain;

namespace WeedDelivery.Backend.Ecosystem.Store;

public interface IProductAdditionalInfoService
{
    Task<List<ProductImage>> GetProductImages(Guid productId);
    Task<Dictionary<Guid, List<ProductImage>>> GetProductImagesBatch(List<Guid> productId);
    
    Task<List<ProductSpecification>> GetProductSpecification(Guid productId);
    Task<Dictionary<Guid, List<ProductSpecification>>> GetProductSpecificationnBatch(List<Guid> productId);
}