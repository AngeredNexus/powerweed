namespace WeedDelivery.Backend.AppInit.Configuration.Common;

public interface IAppConfiguration
{
    void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider);
    void ConfigureServices(IServiceCollection services);
    
    void Configure(IConfiguration configuration);
}