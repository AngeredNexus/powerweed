// using JFA.Telegram.Login;
// using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json;
// using WeedDatabase.Domain.Common;
// using WeedDatabase.Repositories;
// using WeedDelivery.Backend.Models.Api.Common;
//
// namespace WeedDelivery.Backend.Api.v1;
//
// [ApiVersion("1")]
// [Microsoft.AspNetCore.Mvc.Route("api/v{version:apiVersion}/auth")]
// [ApiController]
// [Tags("Auth")]
// public class AuthController : Controller
// {
//     private readonly ITelegramUserRepository _tgUserRepo;
//     private readonly IUserRepository _userRepository;
//
//     private readonly ILogger _logger;
//
//     public AuthController(ITelegramUserRepository tgUserRepo, IUserRepository userRepository, ILogger<AuthController> logger)
//     {
//         _tgUserRepo = tgUserRepo;
//         _userRepository = userRepository;
//         _logger = logger;
//     }
//
//     // [HttpGet("login")]
//     // public async Task<IActionResult> Login([FromQuery] TelegramUser user, [FromServices] ITelegramUser telegramUser,
//     //     [FromServices] Microsoft.Extensions.Options.IOptions<TelegramOption> options)
//     // {
//     //     // if (telegramUser.Validate(user, out var authRes, options.Value.LoginWidgetBotToken))
//     //     // {
//     //         // 1. Получить мета-инфу от главного бота
//     //
//     //         var longId = Convert.ToInt64(user.id);
//     //         var systemUser = await _tgUserRepo.GetTelegramMainBotUser(longId);
//     //
//     //         // 2. Сгенерировать на её основе куки
//     //         var coockie = new TelegramCoockie()
//     //         {
//     //             Id = longId,
//     //             Name = user.username ?? "unknown",
//     //             Language = systemUser.Lang
//     //         };
//     //
//     //         var coockieJsoned = JsonConvert.SerializeObject(coockie);
//     //
//     //         Response.Cookies.Append("sitg", coockieJsoned, new CookieOptions()
//     //         {
//     //             Expires = DateTime.Today.AddDays(7),
//     //             Path = "/"
//     //         });
//     //         
//     //         return Redirect("/");
//     //     // }
//     //
//     //     return BadRequest();
//     // }
//
//     // [HttpGet("login")]
//     // public async Task<IActionResult> Login([FromQuery] TelegramUser user)
//     // {
//     //     // if (telegramUser.Validate(user, out var authRes, options.Value.LoginWidgetBotToken))
//     //     // {
//     //     // 1. Получить мета-инфу от главного бота
//     //
//     //     var userTgId = user.id ?? string.Empty;
//     //     var longId = Convert.ToInt64(userTgId);
//     //
//     //     var systemUser = await _userRepository.GetUserByIdentity(IdentitySource.Telegram, userTgId);
//     //     var systemTgUser = await _tgUserRepo.GetTelegramMainBotUser(longId);
//     //
//     //     if (systemUser is null)
//     //     {
//     //         var newUser = new SmokiUser()
//     //         {
//     //             Name = user.username,
//     //             Role = SmokiUserRole.Customer,
//     //             Source = IdentitySource.Telegram,
//     //             SourceIdentificator = userTgId
//     //         };
//     //
//     //         await _userRepository.AddUser(newUser);
//     //     }
//     //     
//     //     // 2. Сгенерировать на её основе куки
//     //     var coockie = new TelegramCoockie()
//     //     {
//     //         Id = longId,
//     //         Name = user.username ?? "unknown",
//     //         Language = systemTgUser.Lang
//     //     };
//     //
//     //     var coockieJsoned = JsonConvert.SerializeObject(coockie);
//     //
//     //     Response.Cookies.Append("sitg", coockieJsoned, new CookieOptions()
//     //     {
//     //         Expires = DateTime.Today.AddDays(7),
//     //         Path = "/"
//     //     });
//     //         
//     //     return Redirect("/");
//     //     // }
//     //
//     //     return BadRequest();
//     // }
//     //
//     // public class AuthCheckResult
//     // {
//     //     /// <summary>
//     //     /// 
//     //     /// </summary>
//     //     [JsonProperty("isAuthSuccess")]
//     //     public bool IsAuthSuccess { get; set; }
//     // }
//     //
//     // [HttpGet("auth")]
//     // public async Task<IActionResult> Auth()
//     // {
//     //     if (Request.Cookies.TryGetValue("sitg", out var coockieJsoned))
//     //     {
//     //         if (!string.IsNullOrWhiteSpace(coockieJsoned))
//     //         {
//     //             var coockie = JsonConvert.DeserializeObject<TelegramCoockie>(coockieJsoned);
//     //
//     //             if (coockie is not null)
//     //             {
//     //                 // TODO VALIDATE HERE
//     //                 return new OkObjectResult(new AuthCheckResult()
//     //                 {
//     //                     IsAuthSuccess = true
//     //                 });
//     //             }
//     //         }
//     //     }
//     //
//     //     return new OkObjectResult(new AuthCheckResult());
//     // }
//     //
//     //
//     // public class TestLogin
//     // {
//     //     [JsonProperty("id")] public string Id { get; set; }
//     //
//     //     [JsonProperty("username")] public string Username { get; set; }
//     // }
//     //
//     // [HttpPost("login-test")]
//     // public async Task LoginTest([FromBody] TestLogin user)
//     // {
//     //     // 1. Получить мета-инфу от главного бота
//     //
//     //     var longId = Convert.ToInt64(user.Id);
//     //     var systemUser = await _tgUserRepo.GetTelegramMainBotUser(longId);
//     //
//     //     // 2. Сгенерировать на её основе куки
//     //     var coockie = new TelegramCoockie()
//     //     {
//     //         Id = longId,
//     //         Name = user.Username ?? "unknown",
//     //         Language = systemUser.Lang
//     //     };
//     //
//     //     var coockieJsoned = JsonConvert.SerializeObject(coockie);
//     //
//     //     Response.Cookies.Append("sitg", coockieJsoned, new CookieOptions()
//     //     {
//     //         Expires = DateTime.Today.AddDays(7),
//     //         Path = "/"
//     //     });
//     //
//     //     await Response.CompleteAsync();
//     // }
//     //
//     // [HttpGet("tglg")]
//     // public async Task<IActionResult> LoginTelegram([FromQuery] string tgsh)
//     // {
//     //     
//     //     var sysUser = await _userRepository.GetUserByIdentityHash(tgsh);
//     //     
//     //     if (sysUser is not null)
//     //     {
//     //         
//     //         _logger.LogInformation("Authorized {usr} for identity [{isrc} : {idnt}]", sysUser.Name, sysUser.Source.ToString(), tgsh);
//     //         
//     //         return new OkObjectResult(new AuthCheckResult()
//     //         {
//     //             IsAuthSuccess = true
//     //         });
//     //     }
//     //
//     //     return new OkObjectResult(new AuthCheckResult());
//     // }
// }