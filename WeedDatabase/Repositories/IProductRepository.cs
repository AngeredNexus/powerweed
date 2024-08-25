using Database.Domain;

namespace Database.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll();
    Task<IEnumerable<Product>> GetFilterId(IEnumerable<Guid> idSet);

    Task<List<ProductImage>> GetProductImagesBatchById(List<Guid> productsId);
    Task<List<ProductSpecification>> GetProductSpecificationsBatchById(List<Guid> productsId);
}