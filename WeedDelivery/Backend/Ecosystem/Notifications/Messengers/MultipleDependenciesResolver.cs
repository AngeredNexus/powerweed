using Database.Domain;
using WeedDelivery.Backend.Ecosystem.MessengerListeners;

namespace WeedDelivery.Backend.Ecosystem.Notifications.Messengers;

public class MultipleDependenciesResolver : IMultipleDependenciesResolver
{
    private readonly Dictionary<ContactType, IMessengerBotService> _dependencies;

    public MultipleDependenciesResolver(IEnumerable<IMessengerBotService> dependencies)
    {
        _dependencies = dependencies.ToDictionary(x => x.MessengerType, y => y);
    }

    public IMessengerBotService Resolve(ContactType type)
    {
        if (_dependencies.TryGetValue(type, out var service))
        {
            return service;
        }
        throw new InvalidOperationException($"No service for type {type}");
    }
}