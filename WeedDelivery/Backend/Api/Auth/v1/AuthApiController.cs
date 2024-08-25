using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeedDelivery.Backend.Ecosystem.Auth;
using WeedDelivery.Backend.Ecosystem.Users;
using WeedDelivery.Backend.Middleware;

namespace WeedDelivery.Backend.Api.Auth.v1;

[ApiController]
[ApiVersion("1")]
[Microsoft.AspNetCore.Mvc.Route("api/{version}/athy")]
public class AuthApiController : Controller
{
    private readonly IAuthService _authService;
    private readonly IUserCommonService _userCommonService;

    public AuthApiController(IAuthService authService, IUserCommonService userCommonService)
    {
        _authService = authService;
        _userCommonService = userCommonService;
    }
    
    [HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("request")]
    public async Task<ActionResult<AuthRequset>> RequestAuth()
    {
        var registeredRequest = await _authService.CreateRequest();
        return Ok(registeredRequest.Code);
    }

    [HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("check")]
    [Authorize(Roles = "cstmr")]
    public async Task<ActionResult<AuthRequset>> TryAuth()
    {
        return Ok();
    }
    
    [HttpGet]
    [Microsoft.AspNetCore.Mvc.Route("await")]
    public async Task<IActionResult> WaitCodeAccept([FromQuery] string code)
    {
        
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMinutes(2));
        
        var acceptedRequest = await _authService.WaitAuthorizeAsync(code, cts.Token);
        
        if (acceptedRequest is null)
        {
            return BadRequest();
        }

        var existedUser = await _userCommonService.GetExistedUserOrCreateByContactAsync(acceptedRequest.AuthContact?.Id ?? Guid.Empty);
        var token = new AuthMiddlewareService().GenerateToken(existedUser);
        await _authService.SetToken(existedUser, token);
        
        return Ok(token);
    }
}