using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WeedDatabase.Domain;
using WeedDatabase.Domain.App;
using WeedDatabase.Models.Configuration.Database;
using WeedDatabase.Utils;

namespace WeedDatabase.Context;

public class WeedContext : DbContext
{
    private readonly AppDatabasePostgreConfig _config;

    public WeedContext(DbContextOptions<WeedContext> options) : base(options)
    {

    }

    public WeedContext(AppDatabasePostgreConfig config)
    {
        _config = config;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(_config is not null)
            optionsBuilder.UseNpgsql(_config.ConstructConnectionString());
    }


    public DbSet<WeedItem> Weed { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

}