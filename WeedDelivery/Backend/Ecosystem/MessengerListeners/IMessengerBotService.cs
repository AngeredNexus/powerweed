using Database.Domain;
using WeedDelivery.Backend.Ecosystem.Notifications;
using WeedDelivery.Backend.Ecosystem.Notifications.Messengers;

namespace WeedDelivery.Backend.Ecosystem.MessengerListeners;

public interface IMessengerBotService
{
    Task<object> SendMessageAsync(Guid contactDataId, string message);
    public NotificationRole Role { get; }
    public ContactType MessengerType { get; }
}