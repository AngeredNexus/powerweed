using Autofac;
using WeedDelivery.Backend.Systems.App.Market.Admin.Interfaces;
using WeedDelivery.Backend.Systems.App.Market.Admin.Services;
using WeedDelivery.Backend.Systems.App.Market.Customer.Interfaces;
using WeedDelivery.Backend.Systems.App.Market.Customer.Services;
using WeedDelivery.Backend.Systems.App.Ordering.Interfaces;
using WeedDelivery.Backend.Systems.App.Ordering.Services;

namespace WeedDelivery.Backend.AppInit.Deps;

public class AppDependencyMainServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MarketAdminItemsService>().As<IMarketAdminItemsService>();
        
        builder.RegisterType<OrderAdminService>().As<IOrderAdminService>();
        builder.RegisterType<MarketCustomerOrderService>().As<IMarketCustomerOrderService>();
        
        builder.RegisterType<MarketCustomerSearchService>().As<IMarketCustomerSearchService>();
        builder.RegisterType<MarketCustomerCategoryService>().As<IMarketCustomerCategoryService>();
    }
}