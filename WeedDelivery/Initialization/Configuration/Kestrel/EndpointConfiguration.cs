namespace WeedDelivery.Initialization.Configuration.Kestrel;

public class EndpointConfiguration
{
    public string Schema { get; set; }
    public string Host { get; set; }
    public int? Port { get; set; }
    public string FilePath { get; set; }
    public string StoreName { get; set; }
    public string Password { get; set; }
    public string StoreLocation { get; set; }
}