using Autofac;

namespace WeedDelivery.Initialization.Configuration.Common;

public interface IAppConfiguration
{
    void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider);
    void ConfigureServices(IServiceCollection services);
    
    void Configure(IConfiguration configuration);

    void ConfigureContainer(ContainerBuilder builder);
}