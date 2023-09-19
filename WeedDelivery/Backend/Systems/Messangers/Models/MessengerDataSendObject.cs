using WeedDatabase.Domain.Common;
using WeedDelivery.Backend.Systems.Messangers.Interfaces;

namespace WeedDelivery.Backend.Systems.Messangers.Models;

public class MessengerDataSendObject
{
    public SmokiUser AppUser { get; set; } = new();
    public string Message { get; set; } = string.Empty;
    public IMessengerSpecificMessageData MessageObject { get; set; } = new MessengerEmptySpecificMessageData();
}