using Telegram.Bot;
using Telegram.Bot.Types;
using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Models.Telegram;

namespace WeedDelivery.Backend.Bots.Telegram.Common.Services;

public class TelegramNotificationBotModule : TelegramBaseBotModule
{

    private readonly ILogger _logger;
    private readonly ITelegramUserRepository _telegramUserRepository;

    public override TelegramBotType BotType => TelegramBotType.MainBot;
    
    
    public TelegramNotificationBotModule(ILogger<TelegramNotificationBotModule> logger, ITelegramUserRepository telegramUserRepository) : base(logger, telegramUserRepository)
    {
        _logger = logger;
        _telegramUserRepository = telegramUserRepository;
    }

    public override async Task HandleUpdateAsync(TelegramHandleRequestForm form)
    {
        // Nothing to do
    }

    public override async Task HandleMenuCallback(TelegramHandleRequestForm form)
    {
        // Nothing to do
    }
}