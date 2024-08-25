using Autofac;
using Database;
using Database.Context;
using Database.Context.Interfaces;
using Database.Models.Configuration.Database;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;
using WeedDelivery.Backend;
using WeedDelivery.Backend.Ecosystem.Repositories;
using WeedDelivery.Initialization.Configuration.Common;

namespace WeedDelivery.Initialization.Configuration;

public class ApplicationDbContextConfiguration : AppConfiguration
{
    
    private readonly IConfiguration _appSettingsSecret;

    public ApplicationDbContextConfiguration()
    {
        _appSettingsSecret = new ConfigurationBuilder()
            .AddJsonFile("appsettings.secret.json")
            .AddEnvironmentVariables()
            .Build();
        
    }
    
    public override void ConfigureServices(IServiceCollection services)
    {
        var config = _appSettingsSecret.GetSection("Databases").Get<AppDatabaseConfig>();

        if (config is null)
            throw new ConfigurationErrorsException("Database configuration was not found!");
        
        var mainDbConfig = config.Main.ConstructConnectionString();

        services.AddDbContext<ProductDbContext>(
            options => options.UseNpgsql(mainDbConfig));

        services.Configure<AppDatabaseConfig>(_appSettingsSecret.GetSection("Databases"));
    }

    public override void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<ProductDbContextAcceptor>().As<IProductDbContextAcceptor>();
        
        builder.RegisterType<UserIdentityRepository>().As<IUserIdentityRepository>();
        builder.RegisterType<ProductRepository>().As<IProductRepository>();
        builder.RegisterType<OrderRepository>().As<IOrderRepository>();
        builder.RegisterType<MessengerDataRepository>().As<IMessengerDataRepository>();
    }
}