using Microsoft.EntityFrameworkCore;
using WeedDatabase.Context.Interfaces;
using WeedDatabase.Domain.Common;
using WeedDatabase.Domain.Telegram.Types;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Systems.Messangers.Models.Types;

namespace WeedDelivery.Backend.Systems.App.Common.Repos;

public class UserRepository : IUserRepository
{

    private readonly IWeedContextAcceptor _dbAcceptor;

    public UserRepository(IWeedContextAcceptor dbAcceptor)
    {
        _dbAcceptor = dbAcceptor;
    }

    public async Task<List<SmokiUser>> GetAllUsers()
    {
        await using var dbCtx = _dbAcceptor.CreateContext();

        var ops = await dbCtx.Users.ToListAsync();

        return ops;
    }

    public async Task<List<SmokiUser>> GetOps()
    {
        await using var dbCtx = _dbAcceptor.CreateContext();

        var ops = await dbCtx.Users.Where(x => x.Role == SmokiUserRole.Operator).ToListAsync();

        return ops;
    }

    public async Task<SmokiUser?> GetUserByIdentity(MessengerSourceType source, string token)
    {
        await using var dbCtx = _dbAcceptor.CreateContext();

        var user = await dbCtx.Users.FirstOrDefaultAsync(x => x.Source == source && x.SourceIdentificator == token);

        return user;
    }

    public async Task<SmokiUser?> GetUserByIdentityHash(string hash)
    {
        await using var dbCtx = _dbAcceptor.CreateContext();

        var user = await dbCtx.Users.FirstOrDefaultAsync(x => x.IdentityHash == hash);

        return user;
    }

    public async Task<SmokiUser?> GetUserByIdentityCode(string code)
    {
        await using var dbCtx = _dbAcceptor.CreateContext();

        var user = await dbCtx.Users.FirstOrDefaultAsync(x => x.Code == code);

        return user;
    }

    public async Task<SmokiUser?> GetUserById(Guid id)
    {
        await using var dbCtx = _dbAcceptor.CreateContext();

        var user = await dbCtx.Users.FirstOrDefaultAsync(x => x.Id == id);

        return user;
    }

    public async Task AddUser(SmokiUser user)
    {
        await using var dbCtx = _dbAcceptor.CreateContext();

        var now = DateTime.Now;

        user.Created = now;
        user.Modified = now;
        
        dbCtx.Add(user);

        await dbCtx.SaveChangesAsync();
    }

    public async Task UpdateUser(SmokiUser user)
    {
        await using var dbCtx = _dbAcceptor.CreateContext();

        var now = DateTime.Now;

        user.Modified = now;
        
        dbCtx.Update(user);

        await dbCtx.SaveChangesAsync();
    }
}