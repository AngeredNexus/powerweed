using WeedDatabase.Domain.Telegram;
using WeedDatabase.Domain.Telegram.Types;

namespace WeedDatabase.Repositories;

public interface ITelegramUserRepository
{
    Task<TelegramBotUser> GetTelegramMainBotUser(long userId);
    
    Task<TelegramBotUser> InsertOrGetExisted(TelegramBotUser user, TelegramBotType type);
    Task UpdateUser(TelegramBotUser user, TelegramBotType type);
}