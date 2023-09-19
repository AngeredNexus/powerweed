using WeedDatabase.Context;
using WeedDatabase.Context.Interfaces;
using WeedDatabase.Models.Configuration.Database;

namespace WeedDelivery.Backend.Systems.App.Common.Services;

public class TelegramContextAcceptor : ITelegramContextAcceptor
{
    private readonly AppDatabasePostgreConfig _config;

    public TelegramContextAcceptor(Microsoft.Extensions.Options.IOptions<AppDatabaseConfig> config)
    {
        _config = config.Value.Telegram;
    }

    public TelegramContext CreateContext()
    {
        return new TelegramContext(_config);
    }
}