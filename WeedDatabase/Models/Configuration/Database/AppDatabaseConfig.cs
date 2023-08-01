namespace WeedDatabase.Models.Configuration.Database;

public class AppDatabaseConfig
{
    public AppDatabasePostgreConfig Main { get; set; } = new AppDatabasePostgreConfig();
    public AppDatabasePostgreConfig Telegram { get; set; } = new AppDatabasePostgreConfig();
}