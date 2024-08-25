using Database.Context;
using Database.Context.Interfaces;
using Database.Models.Configuration.Database;

namespace WeedDelivery.Backend;

public class ProductDbContextAcceptor : IProductDbContextAcceptor
{
    
    private readonly AppDatabasePostgreConfig _config;
    
    public ProductDbContextAcceptor(Microsoft.Extensions.Options.IOptions<AppDatabaseConfig> config)
    {
        _config = config.Value.Main;
    }
    
    public ProductDbContext CreateContext()
    {
        return new ProductDbContext(_config);
    }
}