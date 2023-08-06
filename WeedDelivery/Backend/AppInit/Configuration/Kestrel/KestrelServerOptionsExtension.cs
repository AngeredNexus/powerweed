using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using WeedDelivery.Backend.Models.Configuration.Kestrel;
using ILogger = Serilog.ILogger;

namespace WeedDelivery.Backend.AppInit.Configuration.Kestrel
{
    public static class KestrelServerOptionsExtension
    {
        public static void ConfigureEndpoints(this KestrelServerOptions options, ILogger logger)
        {
            var configuration = options.ApplicationServices.GetRequiredService<IConfiguration>();
            var environment = options.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

            var endpoints = configuration.GetSection("HttpServer:Endpoints")
                .GetChildren()
                .ToDictionary(section => section.Key, section =>
                {
                    var endpoint = new EndpointConfiguration();
                    section.Bind(endpoint);
                    return endpoint;
                });

            foreach (var endpoint in endpoints)
            {
                var config = endpoint.Value;
                var port = config.Port ?? (config.Schema == "https" ? 443 : 8080);

                var ipAddresses = new List<IPAddress>();
                if (config.Host is "localhost" or "127.0.0.1")
                {
                    // Any для хоста, особенно docker-compose.
                    ipAddresses.Add(IPAddress.Any);
                }
                else if (IPAddress.TryParse(config.Host, out var address))
                {
                    ipAddresses.Add(address);
                }
                else
                {
                    ipAddresses.Add(IPAddress.IPv6Any);
                }

                foreach (var address in ipAddresses)
                {
                    options.Listen(address, port,
                        listenOptions =>
                        {
                            if (config.Schema != "https") return;
                            
                            // для теста\дева самые простые настройки
                            if (environment.IsDevelopment())
                            {
                                listenOptions.UseHttps();
                                return;
                            }
                            
                            // для прода
                            var certificate = LoadCertificate(config, environment);

                            if (certificate != null)
                            {
                                listenOptions.UseHttps(certificate);
                            }
                            else
                            {
                                // в крайнем случае пытаемся использовать дефолтную конфу без подстановки сертификата для https, но предупреждаем
                                logger.Warning("No valid certificate configuration found for the current production endpoint {Address}:{Port}", address, port);
                                listenOptions.UseHttps();
                            }

                        });
                }
            }
        }

        private static X509Certificate2? LoadCertificate(EndpointConfiguration config, IWebHostEnvironment environment)
        {
            
            if (!string.IsNullOrWhiteSpace(config.FilePath))
            {
                return new X509Certificate2(config.FilePath, config.Password);
            }
            
            if (!string.IsNullOrWhiteSpace(config.StoreName) && !string.IsNullOrWhiteSpace(config.StoreLocation))
            {
                using var store = new X509Store(config.StoreName, Enum.Parse<StoreLocation>(config.StoreLocation));
                store.Open(OpenFlags.ReadOnly);
                var certificate = store.Certificates.Find(
                    X509FindType.FindBySubjectName,
                    config.Host,
                    validOnly: !environment.IsDevelopment());

                if (certificate.Count == 0)
                {
                    throw new InvalidOperationException($"Certificate not found for {config.Host}.");
                }

                return certificate[0];
            }

            return null;
            //throw new InvalidOperationException("No valid certificate configuration found for the current endpoint.");
        }
    }
}