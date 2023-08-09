using Microsoft.EntityFrameworkCore;
using WeedDatabase.Context.Interfaces;
using WeedDatabase.Domain.Common;
using WeedDatabase.Repositories;

namespace WeedDelivery.Backend.App.Common.Repos;

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

    public async Task<SmokiUser?> GetUserByIdentity(IdentitySource source, string token)
    {
        await using var dbCtx = _dbAcceptor.CreateContext();

        var ops = await dbCtx.Users.FirstOrDefaultAsync(x => x.Source == source && x.SourceIdentificator == token);

        return ops;
    }

    public async Task AddUser(SmokiUser user)
    {
        await using var dbCtx = _dbAcceptor.CreateContext();
        dbCtx.Add(user);

        await dbCtx.SaveChangesAsync();
    }
}