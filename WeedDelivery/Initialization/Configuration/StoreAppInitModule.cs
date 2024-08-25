using System.Text;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WeedDelivery.Backend.Ecosystem.Auth;
using WeedDelivery.Backend.Ecosystem.Discount;
using WeedDelivery.Backend.Ecosystem.Store;
using WeedDelivery.Initialization.Configuration.Common;

namespace WeedDelivery.Initialization.Configuration;

public class StoreAppInitModule: AppConfiguration
{
    public override void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<ProductViewService>().As<IProductViewService>();
        builder.RegisterType<ProductAdditionalInfoService>().As<IProductAdditionalInfoService>();
        builder.RegisterType<BaseDiscountService>().As<IDiscountService>();
        builder.RegisterType<ProductOrderService>().As<IOrderService>();
    }
}