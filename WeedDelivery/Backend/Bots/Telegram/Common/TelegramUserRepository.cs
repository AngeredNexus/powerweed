using Microsoft.EntityFrameworkCore;
using WeedDatabase.Context;
using WeedDatabase.Context.Interfaces;
using WeedDatabase.Domain.Telegram;
using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;

namespace WeedDelivery.Backend.Bots.Telegram.Common;

public class TelegramUserRepository : ITelegramUserRepository
{

    private readonly ITelegramContextAcceptor _contextAcceptor;

    public TelegramUserRepository(ITelegramContextAcceptor contextAcceptor)
    {
        _contextAcceptor = contextAcceptor;
    }

    public async Task<TelegramBotUser> InsertOrGetExisted(TelegramBotUser user, TelegramBotType type)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var existedUser = await dbCtx.Users.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.BotType == type);

        if (existedUser is not null)
            return existedUser;

        dbCtx.Add(user);
        await dbCtx.SaveChangesAsync();

        return user;
    }

    public async Task UpdateUser(TelegramBotUser user, TelegramBotType type)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var existedUser = await dbCtx.Users.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.BotType == type);

        if (existedUser is null)
            throw new ArgumentNullException($"user ({user.UserId}) for Bot ({type}) does not exist!");

        user.Id = existedUser.Id;
        user.Modified = DateTime.Now;

        existedUser.IsActive = user.IsActive;
        existedUser.HasAcceptedLawPolicy = user.HasAcceptedLawPolicy;
        existedUser.Lang = user.Lang;
        
        dbCtx.Update(existedUser);
        await dbCtx.SaveChangesAsync();
    }
}