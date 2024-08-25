using Database.Context.Interfaces;
using Database.Domain;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WeedDelivery.Backend.Ecosystem.Repositories;

public class ProductRepository : IProductRepository
{
    
    private readonly IProductDbContextAcceptor _contextAcceptor;

    public ProductRepository(IProductDbContextAcceptor contextAcceptor)
    {
        _contextAcceptor = contextAcceptor;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        await using var dbCtx = _contextAcceptor.CreateContext();
        
        var products = await dbCtx.Products.ToListAsync();
        return products;
    }

    public async Task<IEnumerable<Product>> GetFilterId(IEnumerable<Guid> idSet)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();
        
        var products = await dbCtx.Products.Where(x => idSet.Contains(x.Id)).ToListAsync();
        return products;
    }

    public async Task<List<ProductImage>> GetProductImagesBatchById(List<Guid> productsId)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();
        
        var products = await dbCtx.Images.Where(x => productsId.Contains(x.ProductId)).ToListAsync();
        return products;
    }

    public async Task<List<ProductSpecification>> GetProductSpecificationsBatchById(List<Guid> productsId)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();
        
        var products = await dbCtx.Specifications.Where(x => productsId.Contains(x.ProductId)).ToListAsync();
        return products;
    }
}