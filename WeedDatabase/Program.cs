﻿using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using ServiceStack;
using WeedDatabase.Context;
using WeedDatabase.Models.Configuration.Database;
using ILogger = Serilog.ILogger;


namespace WeedDatabase;

public static class Program
{

    static ILogger _logger;
    private static IServiceProvider _provider;
    private static IConfiguration _configuration;

    static async Task Main()
    {
        var services = new ServiceCollection();
        _configuration = BuildConfiguration();

        // Зарегестрировать логгер
        // Зарегестрировать сервис управления миграциями
        // Вызвать и дождаться обновление
        // TODO сделать возможность выбора соединения с БД

        ConfigureLogger();
        _logger = Log.Logger;

        services.AddLogging(bldr => bldr.AddSerilog(dispose: true));
        services.AddEntityFrameworkNpgsql();


        var config = _configuration.GetSection("Databases").Get<AppDatabaseConfig>();

        if (config is null)
            throw new ConfigurationErrorsException("Database configuration was not found!");

        var mainDbConfig = config.Main.ConstructConnectionString();
        var telegramDbConfig = config.Telegram.ConstructConnectionString();

        services.AddDbContext<WeedContext>(
            options => options.UseNpgsql(mainDbConfig));

        services.AddDbContext<TelegramContext>(
            options => options.UseNpgsql(telegramDbConfig));

        _provider = services.BuildServiceProvider();
        
        await ApplyMigrations();
    }

    static void ConfigureLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
    }

    static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.secret.json")
            .AddEnvironmentVariables()
            .Build();
    }


    static async Task ApplyMigrations()
    {
        _logger.Information("Инициализация соединения к БД");

        var mainContext = _provider.GetRequiredService<WeedContext>();
        var telegram = _provider.GetRequiredService<TelegramContext>();
        
        
        _logger.Information("Запуск задачи применения миграций для WEED");
        var commonMigrationApplyTask = mainContext.Database.MigrateAsync();

        _logger.Information("Запуск задачи применения миграций для TELEGRAM");
        var mergedMigrationApplyTask = telegram.Database.MigrateAsync();
        

        _logger.Information("Ожидание окончания применения миграций");

        await Task.WhenAll(commonMigrationApplyTask, mergedMigrationApplyTask);

        _logger.Information("Все БД обновлены!");
    }

    public static string ConstructConnectionString(this AppDatabasePostgreConfig config)
    {
        var connStringBuilder = new StringBuilder();
        
        connStringBuilder.Append($"Server={config.Host};");
        connStringBuilder.Append($"Port={config.Port};");
        connStringBuilder.Append($"Database={config.DbName};");
        connStringBuilder.Append($"Username={config.User};");
        connStringBuilder.Append($"Password={config.Password};");
        
        if(config.EnableSsl is true)
            connStringBuilder.Append("SslMode=Require;");
        if(config.TrustServerCert is true)
            connStringBuilder.Append("TrustServerCertificate=True;");
        
        
        var connStr = connStringBuilder.ToString();
        return connStr;
    }
    
}