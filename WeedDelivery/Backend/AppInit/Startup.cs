using System.Reflection;
using Autofac;

using Microsoft.AspNetCore.Mvc;
using WeedDatabase.Context.Interfaces;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.AppInit.Configuration.Common;
using WeedDelivery.Backend.Systems.App.Bots;
using WeedDelivery.Backend.Systems.App.Common.Repos;
using WeedDelivery.Backend.Systems.App.Common.Services;
using WeedDelivery.Backend.Systems.App.Market.Admin.Interfaces;
using WeedDelivery.Backend.Systems.App.Market.Admin.Repos;
using WeedDelivery.Backend.Systems.App.Market.Admin.Services;
using WeedDelivery.Backend.Systems.App.Market.Customer.Interfaces;
using WeedDelivery.Backend.Systems.App.Market.Customer.Repos;
using WeedDelivery.Backend.Systems.App.Market.Customer.Services;
using WeedDelivery.Backend.Systems.App.Ordering.Interfaces;
using WeedDelivery.Backend.Systems.App.Ordering.Repos;
using WeedDelivery.Backend.Systems.App.Ordering.Services;
using WeedDelivery.Backend.Systems.Messangers.Interfaces;
using WeedDelivery.Backend.Systems.Messangers.Services;
using IContainer = Autofac.IContainer;

namespace WeedDelivery.Backend.AppInit;

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
            .Where(p => p.IsAssignableTo<AppConfiguration>()).As<IAppConfiguration>();

        ApplicationContainer = iocContainerBuilder.Build();

        _configurationModules = ApplicationContainer.Resolve<IEnumerable<IAppConfiguration>>();
    }

    public void ConfigureServices(IServiceCollection services)
    {

        foreach (var module in _configurationModules)
        {
            module.ConfigureServices(services);
        }
        
        services
            .AddRouting(options => options.LowercaseUrls = true)
            .AddControllers()
            .AddNewtonsoftJson();
        
        services.AddRazorPages().AddRazorRuntimeCompilation();        
        
        services.AddSignalR();
        
        services.AddSwaggerGen();
        services.AddApiVersioning(o =>
        {
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);

            o.ReportApiVersions = true;
        });
        
        // services.AddScoped<ITelegramUser, TelegramUserValidator>();
        // services.Configure<TelegramOption>(Configuration.GetSection(nameof(TelegramOption)));
    }

    // https://stackoverflow.com/questions/58133507/configureservices-returning-a-system-iserviceprovider-isnt-supported
    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<TelegramUserRepository>().As<ITelegramUserRepository>();
        builder.RegisterType<TelegramContextAcceptor>().As<ITelegramContextAcceptor>();
        
        builder.RegisterType<MarketAdminItemsService>().As<IMarketAdminItemsService>();
        
        builder.RegisterType<OrderAdminService>().As<IOrderAdminService>();
        builder.RegisterType<MarketCustomerOrderService>().As<IMarketCustomerOrderService>();
        
        builder.RegisterType<MarketCustomerSearchService>().As<IMarketCustomerSearchService>();
        builder.RegisterType<MarketCustomerCategoryService>().As<IMarketCustomerCategoryService>();
        
        builder.RegisterType<MarketAdminItemsRepository>().As<IMarketAdminItemsRepository>();
        builder.RegisterType<MarketCustomerItemsRepository>().As<IMarketCustomerItemsRepository>(); 
        builder.RegisterType<MarketAdminOrderRepository>().As<IMarketAdminOrderRepository>();
        builder.RegisterType<MarketCustomerOrdersRepository>().As<IMarketCustomerOrdersRepository>();
        builder.RegisterType<UserRepository>().As<IUserRepository>();
        
        
        builder.RegisterType<ApplicationTelegramBotService>().As<IApplicationTelegramBotService>().SingleInstance();
        builder.RegisterType<MessengerBotFactory>().As<IMessengerBotFactory>().SingleInstance();

        builder.RegisterAssemblyTypes(typeof(MessengerBotApiBaseService).Assembly)
            .Where(x => x.IsAssignableTo(typeof(IMessengerBotApiService))).As<IMessengerBotApiService>();
        
        builder.RegisterType<TelegramUserRepository>().As<ITelegramUserRepository>();
        
        builder.RegisterType<WeedContextAcceptor>().As<IWeedContextAcceptor>();
        builder.RegisterType<TelegramContextAcceptor>().As<ITelegramContextAcceptor>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "ZALUPA V1");
            });
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        
        app.UseHttpLogging();
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        
        foreach (var module in _configurationModules)
        {
            module.Configure(app, env, serviceProvider);
        }
        
        // app.UsePathBase("/src");
        
        // TODO удалить, если  nginx способен решить и так вопросы с CORS
        
        app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true)
            .AllowCredentials());


        // нужно только локально для разработки без спец настроек для доступа через интернет
        // будет мешать если нужно экспозить только http
        // app.UseHttpsRedirection();

        // нужно определить перез swagger, если требуется инжектить кастомные js при загрузке
        app.UseStaticFiles(); // For the wwwroot folder
        
        // app.UseSpaStaticFiles();
        // app.UseCookiePolicy();

        app.UseRouting();


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });

        // Fire async singleton-service 
        serviceProvider.Resolve<IMessengerBotFactory>();
    }
}