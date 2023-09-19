using Telegram.Bot.Types.ReplyMarkups;
using WeedDelivery.Backend.Systems.Messangers.Interfaces;

namespace WeedDelivery.Backend.Systems.Messangers.Models.MessengerSendingMessageObject;

public class TelegramSendingMessage : MessengerEmptySpecificMessageData
{
    public IReplyMarkup? Markup { get; set; }
}