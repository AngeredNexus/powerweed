using Newtonsoft.Json;
using WeedDatabase.Domain.Common;

namespace WeedDelivery.Backend.Models.Api.Common;

public class TelegramCoockie
{
    [JsonProperty("id")]
    public long Id { get; set; }
        
    [JsonProperty("name")]
    public string Name { get; set; }
        
    [JsonProperty("language")]
    public LanguageTypes Language { get; set; }
}