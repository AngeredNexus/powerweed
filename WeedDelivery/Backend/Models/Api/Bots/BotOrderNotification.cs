using WeedDatabase.Domain.App;
using WeedDatabase.Domain.App.Types;

namespace WeedDelivery.Backend.Models.Api.Bots;

public class BotOrderNotification
{
    public string Firstname { get; set; }
    
    public string Lastname { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Address { get; set; }
    
    public string? DeliveryMan { get; set; }

    public OrderStatus Status { get; set; }

    public List<OrderItem> Items { get; set; } = new();
    
    public int TotalAmount
    {
        get { return Items.Sum(x => x.Amount); }
    }

    public int TotalSum
    {
        get
        {
            if (TotalAmount < 50)
            {
                if (TotalAmount < 10)
                {
                    if (TotalAmount < 5)
                    {
                        // Calculate by weed price
                    }

                    return TotalAmount * 350;

                }

                return TotalAmount * 300;
            }

            return TotalAmount * 250;
        }
    }

    public int TotalSumWithDelivery
    {
        get
        {
            if (TotalAmount > 5)
                return 50;

            return 150;
        }
    }
}