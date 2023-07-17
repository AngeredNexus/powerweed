using WeedDatabase.Repositories;

namespace WeedDelivery.Backend.Bots.Telegram.Common.Services;

public class TelegramBaseSystemService : BackgroundService
{
    private readonly TelegramBaseBotModule _mainBot;

    private readonly ITelegramUserRepository _telegramUserRepository;
    // private readonly TelegramBaseBotModule _clientNotificationBot;
    // private readonly TelegramBaseBotModule _operatorNotificationBot;

    public TelegramBaseSystemService(ILoggerFactory factory, ITelegramUserRepository telegramUserRepository) // IEnumerable<TelegramBaseBotModule> modules)
    {
        _telegramUserRepository = telegramUserRepository;
        
        _mainBot = new TelegramMenuBotModule(new Logger<TelegramMenuBotModule>(factory), _telegramUserRepository);

        // _clientNotificationBot = new TelegramNotificationBotModule(new Logger<TelegramNotificationBotModule>(factory));
        // _operatorNotificationBot = new TelegramNotificationBotModule(new Logger<TelegramNotificationBotModule>(factory));
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _mainBot.StartBot("6049635176:AAG9xCt9w2p0mazE7pqUGMcENs1bx8kUx20", stoppingToken);
        // _clientNotificationBot.StartBot("", stoppingToken);
        // _operatorNotificationBot.StartBot("", stoppingToken);
    }
}