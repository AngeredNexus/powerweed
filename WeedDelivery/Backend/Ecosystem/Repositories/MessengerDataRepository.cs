using Database.Context.Interfaces;
using Database.Domain;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace WeedDelivery.Backend.Ecosystem.Repositories;

public class MessengerDataRepository : IMessengerDataRepository
{

    private readonly IProductDbContextAcceptor _contextAcceptor;

    public MessengerDataRepository(IProductDbContextAcceptor contextAcceptor)
    {
        _contextAcceptor = contextAcceptor;
    }

    public async Task<TelegramContact?> GetContactByIdAsync(Guid id)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();
        return await dbCtx.TelegramData.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<AuthContact?> GetAuthByDataIdAsync(Guid id)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();
        return await dbCtx.Contacts.FirstOrDefaultAsync(x => x.DataId == id);
    }

    public async Task<AuthContact?> GetAuthContactByIdAsync(Guid id)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();
        return await dbCtx.Contacts.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TelegramContact?> GetContactByDataAsync(string chatId, string username)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();
        return await dbCtx.TelegramData.FirstOrDefaultAsync(x => x.ChatId == chatId && x.Username == username);
    }

    public async Task InsertTelegramContactAsync(AuthContact baseContact, TelegramContact contact)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();
        
        dbCtx.TelegramData.Add(contact);
        dbCtx.Contacts.Add(baseContact);
        await dbCtx.SaveChangesAsync();
    }

    public async Task<bool> TryInsertAuthContactWithDataAsync(AuthContact baseContact, TelegramContact contact)
    {
        await using var dbCtx = _contextAcceptor.CreateContext();
        
        var isAlreadyExist = await dbCtx.Contacts.AnyAsync(x => x.Id == baseContact.Id);
        
        if(isAlreadyExist)
            return false;
        
        dbCtx.TelegramData.Add(contact);
        dbCtx.Contacts.Add(baseContact);
        await dbCtx.SaveChangesAsync();
        
        return true;
    }

    public async Task<List<Guid>> GetTelegramManagersDataIdsAsync()
    {
        await using var dbCtx = _contextAcceptor.CreateContext();

        var guids = await dbCtx.Contacts.Join(dbCtx.Users, ctc => ctc.Id, usr => usr.ContactId, (c, usr) => new { c, usr })
            .Where(x => x.usr.Roles.Contains("mngr")).Select(x => x.c.DataId).ToListAsync();

        return guids;
    }
}