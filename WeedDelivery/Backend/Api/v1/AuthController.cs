using JFA.Telegram.Login;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeedDatabase.Domain.Common;
using WeedDatabase.Repositories;
using WeedDelivery.Backend.Models.Api.Common;

namespace WeedDelivery.Backend.Api.v1;

[ApiVersion("1")]
[Microsoft.AspNetCore.Mvc.Route("api/v{version:apiVersion}/auth")]
[ApiController]
[Tags("Auth")]
public class AuthController : Controller
{
    private readonly ITelegramUserRepository _tgUserRepo;
    private readonly IUserRepository _userRepository;

    private readonly ILogger _logger;

    public AuthController(ITelegramUserRepository tgUserRepo, IUserRepository userRepository, ILogger<AuthController> logger)
    {
        _tgUserRepo = tgUserRepo;
        _userRepository = userRepository;
        _logger = logger;
    }


    public class SmokeIslandAuthRequestModel
    {
        [JsonProperty("idCode")]
        public string Code { get; set; }
    }
    
    public class SmokeIslandAuthResponseModel
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }
        
        [JsonProperty("isCodeConfirmed")]
        public bool IsCodeConfirmed { get; set; }
    }
    
    [HttpPost("confirm")]
    public async Task<IActionResult> Auth([FromBody] SmokeIslandAuthRequestModel authRequest)
    {

        var code = authRequest.Code;

        var appUser = await _userRepository.GetUserByIdentityCode(code);

        if (appUser is null)
        {
            return Json(new SmokeIslandAuthResponseModel());
        }
        
        return Json(new SmokeIslandAuthResponseModel() { Hash = appUser.IdentityHash, IsCodeConfirmed = true });
    }
    
    
}