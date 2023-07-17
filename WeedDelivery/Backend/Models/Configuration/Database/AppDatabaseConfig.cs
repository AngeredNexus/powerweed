using WeedDatabase.Models.Configuration.Database;

namespace WeedDelivery.Backend.Models.Configuration.Database;

public class AppDatabaseConfig
{
    public AppDatabasePostgreConfig Main { get; set; } = new AppDatabasePostgreConfig();
    public AppDatabasePostgreConfig Telegram { get; set; } = new AppDatabasePostgreConfig();
}