namespace TestUnit.Auth
{
    public class AuthServiceTests
    {
        private readonly IContainer _container;
        private readonly IAuthService _authService;

        public AuthServiceTests()
        {
            var iocContainerBuilder = new ContainerBuilder();
            iocContainerBuilder.RegisterType<AuthService>().As<IAuthService>();
            _container = iocContainerBuilder.Build();
            _authService = _container.Resolve<IAuthService>();
        }

        [Fact]
        public async Task CreateRequest_ShouldReturnAuthRequest()
        {
            var authReq = await _authService.CreateRequest();
            Assert.NotNull(authReq);
            Assert.False(authReq.IsReady);
        }

        [Fact]
        public async Task WaitAuthorizeAsync_ShouldReturnNull_WhenCodeNotExist()
        {
            var result = await _authService.WaitAuthorizeAsync("invalid-code");
            Assert.Null(result);
        }

        [Fact]
        public async Task HandleCode_ShouldSetAuthContact()
        {
            var authReq = await _authService.CreateRequest();
            var contactAuthData = new ContactAuthData
            {
                Code = authReq.Code,
                AuthContact = new AuthContact
                {
                    Id = GuidExtensions.Sequential(),
                    UserId = GuidExtensions.Sequential(),
                    ContactData = "test@domain.com",
                    Type = ContactType.Email
                }
            };
            var isHandled = await _authService.HandleCode(contactAuthData);
            Assert.True(isHandled);
        }
    }
}
