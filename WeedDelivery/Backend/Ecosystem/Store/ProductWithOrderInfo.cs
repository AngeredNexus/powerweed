using Database.Domain;

namespace WeedDelivery.Backend.Ecosystem.Store;

public class ProductWithOrderInfo
{
    public ProductOrder OrderInfo { get; set; } = new();
    public Product ProductInfo { get; set; } = new();
}