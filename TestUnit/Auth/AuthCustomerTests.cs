using Autofac;
using Moq;
using WeedDelivery.Backend.Common.Utils;
using WeedDelivery.Backend.Ecosystem.Auth;
using WeedDelivery.Backend.Ecosystem.Discount;
using WeedDelivery.Backend.Ecosystem.Store;

namespace TestUnit.Auth;

public class AuthCustomerTests
{
    private readonly IContainer _container;
    
    private readonly IAuthService _authService;
    
    public AuthCustomerTests()
    {
        var iocContainerBuilder = new ContainerBuilder();

        // register types
        iocContainerBuilder.RegisterType<AuthService>().As<IAuthService>();

        _container = iocContainerBuilder.Build();
        _authService = _container.Resolve<IAuthService>();
    }


    [Fact]
    public async Task AuthFlowOk()
    {
        var authReq = await _authService.CreateRequest();
        var isAuthComplete = await _authService.HandleCode(new ContactAuthData()
        {
            Code = authReq.Code,
            AuthContact = new AuthContact()
            {
                Id = GuidExtensions.Sequential(),
                UserId = GuidExtensions.Sequential(),
                ContactData = "test_unit_auth_tg_0",
                Type = ContactType.Telegram
            }
        });
        
        Assert.True(isAuthComplete);
        Assert.True(authReq.IsReady);
        Assert.False(_authService.IsHaveRequests());
        Assert.NotNull(authReq.AuthContact);
    }
    
}