namespace WeedDelivery.Backend.AppInit.Configuration.Common;

public abstract class AppConfiguration : IAppConfiguration
{
    
    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        // nothing to do by default
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        // nothing to do by default
    }

    public virtual void Configure(IConfiguration configuration)
    {
        // nothing to do by default
    }
}