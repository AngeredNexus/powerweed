using Newtonsoft.Json;

namespace WeedDelivery.Backend.Models.Api.Request;

public class ProductOrderApi
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonProperty("amount")]
    public int Amount { get; set; }
}