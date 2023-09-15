using WeedDatabase.Domain.App;
using WeedDatabase.Domain.App.Types;
using WeedDelivery.Backend.Models.App.Entities;

namespace WeedDelivery.Backend.Models.Api.Bots;

public class BotOrderNotification
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    
    public string Lastname { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Address { get; set; }
    
    public string Comment { get; set; }
    
    public string? DeliveryMan { get; set; }

    public OrderStatus Status { get; set; }

    public List<OrderItemView> Items { get; set; } = new();
    
    public int TotalAmount
    {
        get { return Items.Sum(x => x.Amount); }
    }

    public int DiscountGrade => TotalAmount < 50 ? TotalAmount < 10 ? TotalAmount < 5 ? 0 : 1 : 2 : 3;

    public int OrderPrice
    {
        get
        {
            return Items.Sum(x =>
            {
                if (x.HasDiscount)
                {
                    /*
                     Распределение градации(множителя шага дисконта относительно градации условий):
                     x < 5 := 0; - меньше 5ти гр.
                     x < 10 := 1; - меньше 10ти гр.
                     x < 50 := 2; - меньше 50ти гр.
                     x >= 50 := 3; - от 50ти гр.
                    */
                    
                    return x.Amount * (x.Price - x.DiscountGradeStep * DiscountGrade);
                }
                return x.Amount * x.Price;
            });
        }
    }

    public int OrderDeliveryPrice
    {
        get
        {
            if (TotalAmount < 5)
                return 150;

            return 50;
        }
    }
}