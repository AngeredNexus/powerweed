using Funq;
using WeedDelivery.Backend.AppInit;

[assembly: HostingStartup(typeof(AppHost))]

namespace WeedDelivery;

public class AppHost //: AppHostBase, IHostingStartup
{
    // public void Configure(IWebHostBuilder builder) => 
    //     builder
    //         .UseStartup(typeof(Startup))
    //         .UseUrls("http://localhost:55525", "https://localhost:55526");
    //
    // public AppHost() : base("WeedDelivery") {}
    //
    // public override void Configure(Container container)
    // {
    //     // enable server-side rendering, see: https://sharpscript.net/docs/sharp-pages
    //     Plugins.Add(new SharpPagesFeature {
    //         EnableSpaFallback = true
    //     }); 
    //
    //     SetConfig(new HostConfig {
    //         AddRedirectParamsToQueryString = true,
    //     });
    // }
}
