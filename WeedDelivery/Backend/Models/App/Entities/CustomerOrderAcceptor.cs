namespace WeedDelivery.Backend.Models.App.Entities;

public class CustomerOrderAcceptor
{
    public bool IsSuccessful { get; set; }
    public Guid OrderId { get; set; }
}