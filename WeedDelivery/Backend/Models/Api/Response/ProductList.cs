using Newtonsoft.Json;

namespace WeedDelivery.Backend.Models.Api.Response;

public class ProductList
{
    [JsonProperty("products")]
    public List<ApiProduct> Products { get; set; } = new();
}