namespace WeedDelivery.Backend.Models.App.Entities;

public class OrderItemView
{
    
    public Guid WeedId { get; set; }
    
    public int Amount { get; set; }
    
    public string Name { get; set; }
    
    public int Price { get; set; }
    
    public int DiscountGradeStep { get; set; }

    public bool HasDiscount { get; set; }
}