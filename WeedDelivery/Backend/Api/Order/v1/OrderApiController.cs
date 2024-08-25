using Database.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeedDelivery.Backend.Common.Utils;
using WeedDelivery.Backend.Ecosystem.Auth;
using WeedDelivery.Backend.Ecosystem.Discount;
using WeedDelivery.Backend.Ecosystem.Store;
using WeedDelivery.Backend.Models.Api.Request;

namespace WeedDelivery.Backend.Api.Order.v1;

[ApiController]
[ApiVersion("1")]
[Microsoft.AspNetCore.Mvc.Route("api/{version}/order")]
public class OrderApiController : Controller
{
    
    private readonly IOrderService _orderService;
    private readonly IDiscountService _discountService;
    private readonly IAuthService _authService;

    public OrderApiController(IOrderService orderService, IDiscountService discountService, IAuthService authService)
    {
        _orderService = orderService;
        _discountService = discountService;
        _authService = authService;
    }

    [HttpPost]
    [Authorize(Roles = "cstmr")]
    [Microsoft.AspNetCore.Mvc.Route("place")]
    public async Task<ActionResult<object>> MakeOrder([ModelBinder(typeof(UserTypeModelBinder))][FromBody] OrderRequestApi requestApi)
    {
        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var user = await _authService.GetUserByToken(token??"");

        if (user == null)
            return BadRequest("User not found!");

        var request = new OrderRequest()
        {
            RelatedUser = user,
            Name = requestApi.Name,
            Phone = requestApi.Phone,
            Comment = requestApi.Comment,
            Address = requestApi.Address,
            Items = requestApi.Items
        };
        
        var orderPlacementResult = await _orderService.TryCreateOrder(request);
        return Ok(orderPlacementResult);
    }

    [HttpPost]
    [Authorize(Roles = "cstmr")]
    [Microsoft.AspNetCore.Mvc.Route("lookup")]
    public async Task<ActionResult<List<DiscountOrder>>> LookupDiscount([FromBody] List<ProductOrderApi> lookupOrders)
    {
        var items = lookupOrders.Select(x => new ProductOrder()
        {
            ProductId = x.Id,
            Amount = x.Amount,
        }).ToList();
        
        var discountOrders = await _discountService.SetupDiscount(items);
        return Ok(discountOrders);
    }
}