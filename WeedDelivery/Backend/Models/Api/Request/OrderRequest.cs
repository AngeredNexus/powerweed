using Database.Domain;

namespace WeedDelivery.Backend.Models.Api.Request;

public class OrderRequest
{
    public User RelatedUser { get; set; }
    
    public string Name { get; set; }
    
    public string Phone { get; set; }
    
    public string Comment { get; set; }
    
    public List<ProductOrderApi> Items { get; set; } = new();
    
    public string Address { get; set; } = string.Empty;
}