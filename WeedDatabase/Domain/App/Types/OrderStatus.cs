using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeedDatabase.Domain.App.Types;

public enum OrderStatus
{
    
    Unknown = 0,
    
    Pending = 1,
    Prepare = 2,
    Delivery = 3,
    Ready = 4,
    Canceled = 5
}