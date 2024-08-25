using Database.Domain;

namespace WeedDelivery.Backend.Ecosystem.Auth;

public class AuthRequset
{
    public AuthRequset(string code)
    {
        Code = code;
    }

    public string Code { get; }
    public bool IsReady { get; set; }
    public AuthContact? AuthContact { get; set; }

    private bool Equals(AuthRequset other)
    {
        return Code == other.Code;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AuthRequset)obj);
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }

    public static bool operator ==(AuthRequset? left, AuthRequset? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AuthRequset? left, AuthRequset? right)
    {
        return !Equals(left, right);
    }
}