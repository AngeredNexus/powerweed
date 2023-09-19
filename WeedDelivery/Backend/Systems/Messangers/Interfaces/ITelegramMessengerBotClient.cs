using Telegram.Bot;
using Telegram.Bot.Types;
using WeedDelivery.Backend.Models.Telegram;
using WeedDelivery.Backend.Systems.Messangers.Models;
using WeedDelivery.Backend.Systems.Messangers.Models.MessengerSendingMessageObject;

namespace WeedDelivery.Backend.Systems.Messangers.Interfaces;

public interface ITelegramMessengerBotClient
{
    Task BaseHandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

    Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
    
    Task<MessengerDataSendObject> HandleTelegramMessegeInput(TelegramHandleRequestForm form);
}