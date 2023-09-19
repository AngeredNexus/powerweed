using Microsoft.EntityFrameworkCore;
using WeedDatabase.Context.Interfaces;
using WeedDatabase.Domain.Telegram;
using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;

namespace WeedDelivery.Backend.Systems.Messangers.Services;

public class TelegramUserRepository : ITelegramUserRepository
{

    private readonly ITelegramContextAcceptor _contextAcceptor;

    public TelegramUserRepository(ITelegramContextAcceptor contextAcceptor)
    {
        _contextAcceptor = contextAcceptor;
    }

    public async Task<TelegramBotUser> GetTelegramMainBotUser(long userId)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var existedUser = await dbCtx.Users.FirstOrDefaultAsync(x => x.MessengerSource == MessengerSourceType.Telegram && x.UserId == userId);

        if (existedUser is not null)
            return existedUser;

        return new TelegramBotUser();
    }

    public async Task<TelegramBotUser?> GetTelegramUserByHash(string hash)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var existedUser = await dbCtx.Users.FirstOrDefaultAsync(x => x.MessengerSource == MessengerSourceType.Telegram && x.Hash == hash);

        return existedUser;
    }

    public async Task<TelegramBotUser> InsertOrGetExisted(TelegramBotUser? user)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var existedUser = await dbCtx.Users.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.MessengerSource == MessengerSourceType.Telegram);

        if (existedUser is not null)
            return existedUser;

        dbCtx.Add(user);
        await dbCtx.SaveChangesAsync();

        return user;
    }

    public async Task UpdateUser(TelegramBotUser? user)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var existedUser = await GetTelegramUserByHash(user.Hash);

        if (existedUser is null)
            throw new ArgumentNullException($"user ({user.UserId}) for Bot ({MessengerSourceType.Telegram}) does not exist!");

        user.Id = existedUser.Id;
        user.Modified = DateTime.Now;

        existedUser.IsActive = user.IsActive;
        existedUser.HasAcceptedLawPolicy = user.HasAcceptedLawPolicy;
        existedUser.Lang = user.Lang;
        
        dbCtx.Update(existedUser);
        await dbCtx.SaveChangesAsync();
    }
}