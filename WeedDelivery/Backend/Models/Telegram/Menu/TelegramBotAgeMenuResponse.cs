using Newtonsoft.Json;

namespace WeedDelivery.Backend.Models.Telegram.Menu;

public class TelegramBotAgeMenuResponse
{
    [JsonProperty("hasLegalAge")]
    public bool HasLegalAge { get; set; }
}