using WeedDatabase.Domain.Telegram.Types;
using WeedDelivery.Backend.Systems.Messangers.Models.Types;

namespace WeedDelivery.Backend.Systems.Messangers.Models;

public class MessengerDelayedMessageSendObject
{
    public MessengerSourceType Messenger { get; set; }
    public MessengerBotType Type { get; set; }
    public MessengerDataSendObject MessageToSend { get; set; }
}