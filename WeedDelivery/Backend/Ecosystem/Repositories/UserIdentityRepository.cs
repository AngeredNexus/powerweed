using Database.Context.Interfaces;
using Database.Domain;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WeedDelivery.Backend.Ecosystem.Repositories;

public class UserIdentityRepository : IUserIdentityRepository
{

    private readonly IProductDbContextAcceptor _dbContextAcceptor;

    public UserIdentityRepository(IProductDbContextAcceptor dbContextAcceptor)
    {
        _dbContextAcceptor = dbContextAcceptor;
    }

    public async Task<User?> FindByIdAsync(Guid id)
    {
        var dbCtx = _dbContextAcceptor.CreateContext();
        var user = await dbCtx.Users.FirstOrDefaultAsync(x => x.Id == id);

        return user;
    }

    public async Task<User?> FindByContactIdAsync(Guid id)
    {
        var dbCtx = _dbContextAcceptor.CreateContext();
        var user = await dbCtx.Users.FirstOrDefaultAsync(x => x.ContactId == id);

        return user;
    }

    public async Task<User?> FindByTokenAsync(string token)
    {
        var dbCtx = _dbContextAcceptor.CreateContext();
        var user = await dbCtx.Users.FirstOrDefaultAsync(x => x.TokenSync == token);

        return user;
    }

    public async Task AddUserAsync(User user)
    {
        var dbCtx = _dbContextAcceptor.CreateContext();
        
        dbCtx.Users.Add(user);
        await dbCtx.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        var dbCtx = _dbContextAcceptor.CreateContext();
        
        dbCtx.Users.Update(user);
        await dbCtx.SaveChangesAsync();
    }

    public async Task UpdateContactAsync(AuthContact? contact)
    {
        var dbCtx = _dbContextAcceptor.CreateContext();
        
        dbCtx.Contacts.Update(contact);
        await dbCtx.SaveChangesAsync();
    }
    
    public async Task<AuthContact?> GetContactByUserAsync(User user)
    {
        var dbCtx = _dbContextAcceptor.CreateContext();
        var contact = await dbCtx.Contacts.FirstOrDefaultAsync(x => x.Id == user.ContactId);

        return contact;
    }
}