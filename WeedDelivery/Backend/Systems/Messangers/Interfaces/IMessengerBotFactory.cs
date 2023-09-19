using WeedDatabase.Domain.Telegram.Types;
using WeedDelivery.Backend.Systems.Messangers.Models.Types;
using WeedDelivery.Backend.Systems.Messangers.Services;

namespace WeedDelivery.Backend.Systems.Messangers.Interfaces;

public interface IMessengerBotFactory
{
    IMessengerBotApiService GetSpecificBotInstance(MessengerSourceType source, MessengerBotType botType);
}