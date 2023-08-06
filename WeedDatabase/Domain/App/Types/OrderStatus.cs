namespace WeedDatabase.Domain.App.Types;

public enum OrderStatus
{
    Unknown = 0,
    
    Pending = 1,
    Delivery = 2,
    Transfer = 3,
    Ready = 4,
    Canceled = 5
}