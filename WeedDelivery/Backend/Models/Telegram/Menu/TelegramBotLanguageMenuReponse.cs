using Newtonsoft.Json;
using WeedDatabase.Domain.Common;

namespace WeedDelivery.Backend.Models.Telegram.Menu;

public class TelegramBotLanguageMenuReponse
{
    [JsonProperty("lang")]
    public LanguageTypes Language { get; set; }
}