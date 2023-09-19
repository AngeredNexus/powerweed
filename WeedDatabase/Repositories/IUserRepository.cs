using WeedDatabase.Domain.Common;
using WeedDatabase.Domain.Telegram.Types;

namespace WeedDatabase.Repositories;

public interface IUserRepository
{
    Task<List<SmokiUser>> GetAllUsers();
    
    Task<List<SmokiUser>> GetOps();
    
    Task<SmokiUser?> GetUserByIdentity(MessengerSourceType source, string token);
    Task<SmokiUser?> GetUserByIdentityHash(string hash);
    Task<SmokiUser?> GetUserByIdentityCode(string code);


    Task AddUser(SmokiUser user);
    Task UpdateUser(SmokiUser user);
}