using Telegram.Bot;
using Telegram.Bot.Types;
using WeedDatabase.Domain.Telegram;
using WeedDelivery.Backend.Systems.Messangers.Models;

namespace WeedDelivery.Backend.Models.Telegram;

public class TelegramHandleRequestForm
{
    public TelegramHandleRequestForm(Update telegramUpdate, string hash)
    {
        TelegramUpdate = telegramUpdate;
        AppMessage = new MessengerDataUpdateObject()
        {
            Hash = hash,
            Message = telegramUpdate.Message?.Text ?? string.Empty
        };
    }

    public TelegramBotUser? User { get; set; }

    public Update? TelegramUpdate { get; set; }
    
    public MessengerDataUpdateObject AppMessage { get; set; }
}