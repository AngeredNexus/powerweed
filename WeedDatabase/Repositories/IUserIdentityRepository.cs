using Database.Domain;

namespace Database.Repositories;

public interface IUserIdentityRepository
{
    Task<User?> FindByIdAsync(Guid id);
    Task<User?> FindByContactIdAsync(Guid id);
    Task<User?> FindByTokenAsync(String token);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task UpdateContactAsync(AuthContact? contact);
    Task<AuthContact?> GetContactByUserAsync(User user);
}