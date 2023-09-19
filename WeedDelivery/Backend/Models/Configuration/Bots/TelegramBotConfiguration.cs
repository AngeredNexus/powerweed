using Newtonsoft.Json;
using WeedDatabase.Domain.Telegram.Types;
using WeedDelivery.Backend.Systems.Messangers.Models.Types;

namespace WeedDelivery.Backend.Models.Configuration.Bots;

public class TelegramBotConfiguration
{
    public string Token { get; set; }
    
    public MessengerBotType BotType { get; set; }
}