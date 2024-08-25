using Database.Domain;
using WeedDelivery.Backend.Ecosystem.Auth;

namespace WeedDelivery.Backend.Ecosystem.Users;

public interface IUserCommonService
{
    Task<User> GetExistedUserOrCreateByContactAsync(Guid contactId);
}