using WeedDatabase.Domain.Common;

namespace WeedDatabase.Repositories;

public interface IUserRepository
{
    Task<List<SmokiUser>> GetAllUsers();
    
    Task<List<SmokiUser>> GetOps();
    
    Task<SmokiUser?> GetUserByIdentity(IdentitySource source, string token);
    Task<SmokiUser?> GetUserByIdentityHash(string hash);


    Task AddUser(SmokiUser user);
    Task UpdateUser(SmokiUser user);
}