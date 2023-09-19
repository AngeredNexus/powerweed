using Newtonsoft.Json;

namespace WeedDelivery.Backend.Models.Api.Request;

public class OrderApi
{
    [JsonProperty("firstname")]
    public string Firstname { get; set; }
    
    [JsonProperty("lastname")]
    public string Lastname { get; set; }
    
    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; }
    
    [JsonProperty("address")]
    public string Address { get; set; }
    
    [JsonProperty("comment")]
    public string Comment { get; set; }

    [JsonProperty("items")] 
    public List<OrderItemApi> Items { get; set; } = new();
    
    [JsonProperty("hash")]
    public string Hash { get; set; }
}