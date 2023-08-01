using Newtonsoft.Json;

namespace WeedDelivery.Backend.Models.Telegram.Menu.Common;

public class TelegramBaseResponse<T> where T: class
{
    [JsonProperty("menuType")]
    public TelegramBotMenuType MenuType { get; set; }
    
    [JsonProperty("result")]
    public T Result { get; set; }
}