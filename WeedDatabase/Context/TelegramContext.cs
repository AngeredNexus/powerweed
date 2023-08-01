using Microsoft.EntityFrameworkCore;
using WeedDatabase.Domain.Telegram;
using WeedDatabase.Models.Configuration.Database;
using WeedDatabase.Utils;

namespace WeedDatabase.Context;

public class TelegramContext : DbContext
{
    private readonly AppDatabasePostgreConfig _config;

    public TelegramContext(DbContextOptions<TelegramContext> options) : base(options)
    {

    }

    public TelegramContext(AppDatabasePostgreConfig config)
    {
        _config = config;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(_config is not null)
            optionsBuilder.UseNpgsql(_config.ConstructConnectionString());
    }
    
    public DbSet<TelegramBotUser> Users { get; set; }
    
   
}