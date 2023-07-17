namespace WeedDatabase.Models.Configuration.Database;

public class AppDatabasePostgreConfig
{
    public string User { get; set; } = "postgres";
    public string Password { get; set; } = "qwerty1234!";
    public string DbName { get; set; } = "defaul_db";
    public string ConnectionName { get; set; } = "default_conn";
    public string Host { get; set; } = "localhost";
    public string Port { get; set; } = "5432";
    public bool? EnableSsl { get; set; }
    public bool? TrustServerCert { get; set; }
}