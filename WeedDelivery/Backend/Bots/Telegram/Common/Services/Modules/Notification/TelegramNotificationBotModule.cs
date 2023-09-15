using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Models.Telegram;

namespace WeedDelivery.Backend.Bots.Telegram.Common.Services.Modules.Notification;

public class TelegramNotificationBotModule : TelegramBaseBotModule
{

    private readonly ILogger _logger;
    private readonly ITelegramUserRepository _telegramUserRepository;
    private readonly IUserRepository _userRepository;

    public override TelegramBotType BotType => TelegramBotType.OrderNotificationCustomerBot;
    
    
    public TelegramNotificationBotModule(ILogger<TelegramNotificationBotModule> logger, ITelegramUserRepository telegramUserRepository, IUserRepository userRepository) 
        : base(logger, telegramUserRepository, userRepository)
    {
        _logger = logger;
        _telegramUserRepository = telegramUserRepository;
        _userRepository = userRepository;
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