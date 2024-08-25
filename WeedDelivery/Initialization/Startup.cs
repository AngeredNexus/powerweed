using Autofac;
using WeedDelivery.Initialization.Configuration.Common;
using IContainer = Autofac.IContainer;

namespace WeedDelivery.Initialization;

public class Startup
{
    /// <summary>
    /// Доступ к json-конфиг файлу
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// контейнер всех компонентов приложения
    /// </summary>
    public IContainer ApplicationContainer { get; private set; }

    
    public IEnumerable<IAppConfiguration> _configurationModules { get; private set; }


    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

        var iocContainerBuilder = new ContainerBuilder();

        iocContainerBuilder.RegisterAssemblyTypes(typeof(AppConfiguration).Assembly)
            .Where(p => p.IsSubclassOf(typeof(AppConfiguration))).As<IAppConfiguration>();

        ApplicationContainer = iocContainerBuilder.Build();
        _configurationModules = ApplicationContainer.Resolve<IEnumerable<IAppConfiguration>>();
        
        foreach (var module in _configurationModules)
        {
            module.Configure(configuration);
        }
    }

    public void ConfigureServices(IServiceCollection services)
    {
        foreach (var module in _configurationModules)
        {
            module.ConfigureServices(services);
        }
    }

    // https://stackoverflow.com/questions/58133507/configureservices-returning-a-system-iserviceprovider-isnt-supported
    public void ConfigureContainer(ContainerBuilder builder)
    {
        foreach (var module in _configurationModules)
        {
            module.ConfigureContainer(builder);
        }
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        foreach (var module in _configurationModules)
        {
            module.Configure(app, env, serviceProvider);
        }
    }
}