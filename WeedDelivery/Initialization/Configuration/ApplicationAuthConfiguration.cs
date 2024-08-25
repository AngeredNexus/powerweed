using System.Text;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WeedDelivery.Backend.Ecosystem.Auth;
using WeedDelivery.Initialization.Configuration.Common;

namespace WeedDelivery.Initialization.Configuration;

public class ApplicationAuthConfiguration : AppConfiguration
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("WhVVHLhihb69VPcTq7zfY3lkNZcT53teYE2q5RGVVsNRacJt41YBl2x0dvZYuq8kkslSXcYQevZtAiCi")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        services.AddAuthorization(options => options.AddPolicy("Customer", policy => policy.RequireRole("cstmr")));
    }

    public override void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterType<AuthService>().As<IAuthService>().SingleInstance();
    }
}