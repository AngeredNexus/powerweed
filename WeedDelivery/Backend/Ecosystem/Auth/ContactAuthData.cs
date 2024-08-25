using Database.Domain;

namespace WeedDelivery.Backend.Ecosystem.Auth;

public class ContactAuthData
{
    public string Code { get; set; }
    public AuthContact? AuthContact { get; set; } = new();
}