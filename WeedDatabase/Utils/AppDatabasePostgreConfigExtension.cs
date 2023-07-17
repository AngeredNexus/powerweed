using System.Text;
using WeedDatabase.Models.Configuration.Database;

namespace WeedDatabase.Utils;

public static class AppDatabasePostgreConfigExtension
{
    public static string ConstructConnectionString(this AppDatabasePostgreConfig config)
    {
        var connStringBuilder = new StringBuilder();
        
        connStringBuilder.Append($"Server={config.Host};");
        connStringBuilder.Append($"Port={config.Port};");
        connStringBuilder.Append($"Database={config.DbName};");
        connStringBuilder.Append($"Username={config.User};");
        connStringBuilder.Append($"Password={config.Password};");
        
        if(config.EnableSsl is true)
            connStringBuilder.Append("SslMode=Require;");
        if(config.TrustServerCert is true)
            connStringBuilder.Append("TrustServerCertificate=True;");
        
        
        var connStr = connStringBuilder.ToString();
        return connStr;
    }
}