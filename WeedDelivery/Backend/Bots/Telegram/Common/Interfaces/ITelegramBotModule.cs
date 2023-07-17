using Telegram.Bot;
using Telegram.Bot.Types;
using WeedDatabase.Domain.Telegram.Types;
using WeedDelivery.Backend.Models.Telegram;

namespace WeedDelivery.Backend.Bots.Telegram.Common.Interfaces;

public interface ITelegramBotModule
{
    Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken);

    Task HandleUpdateAsync(TelegramHandleRequestForm form);
    Task HandleMenuCallback(TelegramHandleRequestForm form);
    
    Task SendMessageAsync(string userId, string message);
    
    TelegramBotType BotType { get; }
}