using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Serilog;
using ViteDotNet;
using ViteDotNet.Middleware;
using WeedDelivery.Backend.Ecosystem.MessengerListeners;
using WeedDelivery.Backend.Ecosystem.MessengerListeners.Telegram;
using WeedDelivery.Backend.Ecosystem.Notifications;
using WeedDelivery.Backend.Ecosystem.Notifications.Messengers;
using WeedDelivery.Backend.Ecosystem.Users;
using WeedDelivery.Initialization.Configuration.Common;

namespace WeedDelivery.Initialization.Configuration;

public class ApplicationBaseConfiguration : AppConfiguration
{
    
    private readonly IConfiguration _appSettings = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();


    public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        app.UseSerilogRequestLogging();
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "PStore V1");
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
        
        // TODO удалить, если  nginx способен решить и так вопросы с CORS
        app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true)
            .AllowCredentials());
        
        app.UseStaticFiles(); // For the wwwroot folder
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });
        
        app.RunViteDevServer("./ProductShop");
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT",
                Description = "TOKEN",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };
                
            c.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            };
            c.AddSecurityRequirement(securityRequirement);
        });
        
        services
            .AddRouting(options => options.LowercaseUrls = true)
            .AddControllers()
            .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        
        services.AddRazorPages().AddRazorRuntimeCompilation().AddRazorPagesOptions(o => o.Conventions.AddPageRoute("/index", "{*url}"));
        services.AddViteIntegration(_appSettings);
        
        services.AddApiVersioning(o =>
        {
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);

            o.ReportApiVersions = true;
        });

        services.AddHttpLogging(_=>{});
        services.AddSerilog();
    }

    public override void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<UserCommonService>().As<IUserCommonService>();
        builder.RegisterType<MultipleDependenciesResolver>().As<IMultipleDependenciesResolver>().SingleInstance();
        builder.RegisterType<MessengerNotificationService>().As<IMessengerNotificationService>().SingleInstance();
        
        builder.RegisterType<TelegramCustomerBotService>().As<IMessengerBotService>().SingleInstance();
        builder.RegisterType<TelegramManagerBotService>().As<IMessengerBotService>().SingleInstance();
        
        builder.RegisterType<TelegramCustomerBotService>().As<IHostedService>().SingleInstance();
        builder.RegisterType<TelegramManagerBotService>().As<IHostedService>().SingleInstance();
    }
}