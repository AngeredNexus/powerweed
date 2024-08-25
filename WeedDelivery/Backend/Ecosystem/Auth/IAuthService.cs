using Database.Domain;

namespace WeedDelivery.Backend.Ecosystem.Auth;

public interface IAuthService
{
    Task<bool> HandleCode(ContactAuthData contactAuthData);
    Task<AuthRequset> CreateRequest();
    
    Task<AuthRequset?> WaitAuthorizeAsync(string code, CancellationToken cancellationToken = default!);

    bool IsHaveRequests();
    
    Task<bool> SetToken(User user, string token);
    Task<User?> GetUserByToken(string token);
}