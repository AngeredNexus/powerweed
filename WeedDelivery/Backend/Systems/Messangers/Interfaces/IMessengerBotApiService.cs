using WeedDatabase.Domain.Telegram.Types;
using WeedDelivery.Backend.Systems.Messangers.Models;
using WeedDelivery.Backend.Systems.Messangers.Models.Types;

namespace WeedDelivery.Backend.Systems.Messangers.Interfaces;

public interface IMessengerBotApiService : IHostedService
{
    MessengerSourceType MessengerSource { get; }
    MessengerBotType BotType { get; }
    void Configure(MessengerSetupObject setup);
    Task SendMessage(MessengerDataSendObject message);
} 