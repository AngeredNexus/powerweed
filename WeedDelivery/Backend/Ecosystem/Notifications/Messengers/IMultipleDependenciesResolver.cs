using Database.Domain;
using WeedDelivery.Backend.Ecosystem.MessengerListeners;

namespace WeedDelivery.Backend.Ecosystem.Notifications.Messengers;

public interface IMultipleDependenciesResolver
{
    IMessengerBotService Resolve(ContactType type);
}