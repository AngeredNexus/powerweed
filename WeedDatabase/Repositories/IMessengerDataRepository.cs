using Database.Domain;

namespace Database.Repositories;

public interface IMessengerDataRepository
{
    Task<TelegramContact?> GetContactByIdAsync(Guid id);
    Task<AuthContact?> GetAuthByDataIdAsync(Guid id);
    Task<AuthContact?> GetAuthContactByIdAsync(Guid id);
    Task<TelegramContact?> GetContactByDataAsync(string chatId, string username);
    
    Task InsertTelegramContactAsync(AuthContact baseContact, TelegramContact contact);

    Task<bool> TryInsertAuthContactWithDataAsync(AuthContact baseContact, TelegramContact contact);

    Task<List<Guid>> GetTelegramManagersDataIdsAsync();
}