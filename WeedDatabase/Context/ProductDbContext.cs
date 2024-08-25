using Database.Domain;
using Database.Models.Configuration.Database;
using Microsoft.EntityFrameworkCore;

namespace Database.Context;

public class ProductDbContext : DbContext
{
    private readonly AppDatabasePostgreConfig? _config;

    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {

    }

    public ProductDbContext(AppDatabasePostgreConfig config)
    {
        _config = config;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(_config is not null)
            optionsBuilder.UseNpgsql(_config.ConstructConnectionString());
    }
    
    public DbSet<ProductOrder> ProductOrders { get; set; }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> Images { get; set; }
    public DbSet<ProductSpecification> Specifications { get; set; }
    
    public DbSet<UserOrder> UserOrders { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AuthContact> Contacts { get; set; }
    public DbSet<TelegramContact> TelegramData { get; set; }
}