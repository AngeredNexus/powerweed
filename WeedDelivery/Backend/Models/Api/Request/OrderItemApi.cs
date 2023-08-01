using Newtonsoft.Json;

namespace WeedDelivery.Backend.Models.Api.Request;

public class OrderItemApi
{
    [JsonProperty("weedId")]
    public Guid WeedId { get; set; }
    
    [JsonProperty("amount")]
    public int Amount { get; set; }
}