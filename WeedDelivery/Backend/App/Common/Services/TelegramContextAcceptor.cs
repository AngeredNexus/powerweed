using WeedDatabase.Context;
using WeedDatabase.Context.Interfaces;
using WeedDatabase.Models.Configuration.Database;
using WeedDelivery.Backend.Models.Configuration.Database;

namespace WeedDelivery.Backend.App.Common.Services;

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