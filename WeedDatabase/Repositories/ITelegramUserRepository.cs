using WeedDatabase.Domain.Telegram;
using WeedDatabase.Domain.Telegram.Types;

namespace WeedDatabase.Repositories;

public interface ITelegramUserRepository
{
    Task<TelegramBotUser> GetTelegramMainBotUser(long userId);
    Task<TelegramBotUser?> GetTelegramUserByHash(string hash);
    
    Task<TelegramBotUser> InsertOrGetExisted(TelegramBotUser? user);
    Task UpdateUser(TelegramBotUser? user);
}