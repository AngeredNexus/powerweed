using Database.Domain;
using Newtonsoft.Json;

namespace WeedDelivery.Backend.Models.Api.Response;

public class ApiProduct
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("price")]
    public int Price { get; set; }
    
    [JsonProperty("images")]
    public List<string> Images { get; set; } = new();
    
    [JsonProperty("specification")]
    public List<ProductSpecification> Specification { get; set; } = new();
}