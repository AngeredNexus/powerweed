using Database.Domain;
using Database.Repositories;
using WeedDelivery.Backend.Common.Utils;

namespace WeedDelivery.Backend.Ecosystem.Auth;

public class AuthService : IAuthService
{
    private static readonly HashSet<AuthRequset> Requests = new();
    private readonly IUserIdentityRepository _userRepo;

    public AuthService(IUserIdentityRepository userRepo)
    {
        _userRepo = userRepo;
    }
    
    public Task<AuthRequset> CreateRequest()
    {
        var request = new AuthRequset(GuidExtensions.Sequential().ToString());
        Requests.Add(request);
        return Task.FromResult(request);
    }

    public async Task<AuthRequset?> WaitAuthorizeAsync(string code, CancellationToken cancellationToken = default!)
    {
        var req = Requests.FirstOrDefault(x => x.Code == code);

        return await Task.Run(async () =>
        {
            if(req is null)
                return null;
            
            while(!req.IsReady)
                await Task.Delay(100, cancellationToken);

            if(cancellationToken.IsCancellationRequested)
                return null;
            
            Requests.Remove(req);
            return req;
        }, cancellationToken);
    }

    public bool IsHaveRequests()
    {
        return Requests.Any();
    }

    public async Task<bool> SetToken(User user, string token)
    {
        user.TokenSync = token;
        await _userRepo.UpdateUserAsync(user);
        return true;
    }

    public async Task<User?> GetUserByToken(string token)
    {
        return await _userRepo.FindByTokenAsync(token);
    }

    public Task<bool> HandleCode(ContactAuthData contactAuthData)
    {
        var authReq = Requests.FirstOrDefault(x => x.Code == contactAuthData.Code);

        if (authReq is null) return Task.FromResult(false);
        
        authReq.AuthContact = contactAuthData.AuthContact;
        authReq.IsReady = true;
        
        return Task.FromResult(true);
    }
    
}