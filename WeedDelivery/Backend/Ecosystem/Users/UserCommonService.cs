using Database.Domain;
using Database.Repositories;
using WeedDelivery.Backend.Common.Utils;

namespace WeedDelivery.Backend.Ecosystem.Users;

public class UserCommonService : IUserCommonService
{
    private readonly IUserIdentityRepository _userRepository;

    public UserCommonService(IUserIdentityRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> GetExistedUserOrCreateByContactAsync(Guid contactId)
    {
        var user = await _userRepository.FindByContactIdAsync(contactId);

        if (user is not null) return user;
        
        user = new User()
        {
            Id = GuidExtensions.Sequential(),
            Roles = new[] {"cstmr"},
            ContactId = contactId
        };
            
        await _userRepository.AddUserAsync(user);

        return user;
    }
}