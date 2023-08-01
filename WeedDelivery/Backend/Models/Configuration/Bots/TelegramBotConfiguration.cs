using Newtonsoft.Json;
using WeedDatabase.Domain.Telegram.Types;

namespace WeedDelivery.Backend.Models.Configuration.Bots;

public class TelegramBotConfiguration
{
    public string Token { get; set; }
    
    public TelegramBotType BotType { get; set; }
}