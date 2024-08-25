using Newtonsoft.Json;

namespace WeedDelivery.Backend.Models.Api.Request;

public class OrderRequestApi
{
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("phone")]
    public string Phone { get; set; }
    
    [JsonProperty("comment")]
    public string Comment { get; set; }
    
    [JsonProperty("items")]
    public List<ProductOrderApi> Items { get; set; } = new();
    
    [JsonProperty("address")]
    public string Address { get; set; } = string.Empty;
}