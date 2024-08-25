using Database.Domain;
using Newtonsoft.Json;

namespace WeedDelivery.Backend.Ecosystem.Discount;

public class DiscountOrder
{
    [JsonProperty("productOrder")]
    public ProductOrder Order { get; set; }
    
    [JsonProperty("originalPrice")]
    public decimal Original { get; set; }
    
    [JsonProperty("discountedPrice")]
    public decimal Updated { get; set; }
}