using WeedDatabase.Context;
using WeedDatabase.Context.Interfaces;
using WeedDatabase.Models.Configuration.Database;

namespace WeedDelivery.Backend.Systems.App.Common.Services;

public class WeedContextAcceptor : IWeedContextAcceptor
{
    
    private readonly AppDatabasePostgreConfig _config;
    
    public WeedContextAcceptor(Microsoft.Extensions.Options.IOptions<AppDatabaseConfig> config)
    {
        _config = config.Value.Main;
    }
    
    public WeedContext CreateContext()
    {
        return new WeedContext(_config);
    }
}