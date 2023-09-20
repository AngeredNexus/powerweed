using System.Collections.Concurrent;
using WeedDatabase.Domain.Common;
using WeedDatabase.Domain.Telegram.Types;
using WeedDelivery.Backend.Systems.Messangers.Interfaces;
using WeedDelivery.Backend.Systems.Messangers.Models;
using WeedDelivery.Backend.Systems.Messangers.Models.Types;

namespace WeedDelivery.Backend.Systems.Messangers.Services;

public abstract class MessengerBotApiBaseService : IMessengerBotApiService
{

    private BlockingCollection<MessengerDelayedMessageSendObject> _delayedMessages = new();

    protected void AddMessageToInvoke(MessengerSourceType messengerToInvoke, MessengerBotType botToInvoke,
        MessengerDataSendObject message)
    {
        var delayedMessage = new MessengerDelayedMessageSendObject()
        {
            Messenger = messengerToInvoke,
            Type = botToInvoke,
            MessageToSend = message
        };
        
        _delayedMessages.Add(delayedMessage);
    }
    
    /// <summary>
    /// Тип мессенджера
    /// </summary>
    public abstract MessengerSourceType MessengerSource { get; }

    public abstract MessengerBotType BotType { get; }

    /// <summary>
    /// Прежде, чем запускать микросервер-бота мессенджера, его нужно сконфигурировать(передать секретный ключ, прим.)
    /// </summary>
    public abstract void Configure(MessengerSetupObject setup);

    public abstract Task SendMessage(MessengerDataSendObject messenger);
    public async Task<IEnumerable<MessengerDelayedMessageSendObject>> AwaitMessages()
    {
        return _delayedMessages.GetConsumingEnumerable();
    }

    /// <summary>
    /// Запустить микросервер-бота
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task StartAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Завершить соединение\попытки переподсоединения 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task StopAsync(CancellationToken cancellationToken);

}