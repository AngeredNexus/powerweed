using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Bots.Telegram.Common.Interfaces;
using WeedDelivery.Backend.Models.Configuration.Bots;

namespace WeedDelivery.Backend.Bots.Telegram.Common.Services;

public class TelegramBaseSystemService : BackgroundService
{
    private readonly ITelegramBotFactory _telegramBotFactory;
    private readonly ITelegramUserRepository _telegramUserRepository;

    private readonly Microsoft.Extensions.Options.IOptions<AppTelegramConfiguration> _telegramOptions;

    public TelegramBaseSystemService(ITelegramUserRepository telegramUserRepository, ITelegramBotFactory telegramBotFactory, Microsoft.Extensions.Options.IOptions<AppTelegramConfiguration> telegramOptions)
    {
        _telegramUserRepository = telegramUserRepository;
        _telegramBotFactory = telegramBotFactory;
        _telegramOptions = telegramOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var moduleTasks = new List<Task>();
        
        Enum.GetValues<TelegramBotType>().Except(new [] { TelegramBotType.Unknown }).ToList().ForEach(moduleType =>
        {
            var module = _telegramBotFactory.GetModule(moduleType);

            var moduleConfig = _telegramOptions.Value.Bots.FirstOrDefault(x => x.BotType == module.BotType);

            if (moduleConfig is null)
            {
                // log error!
                return;
            }
            
            var moduleTask = module.Listen(moduleConfig.Token, stoppingToken);
            moduleTasks.Add(moduleTask);
        });

        await Task.WhenAll(moduleTasks);

        // Infinite W8 bots
        while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);

    }
}