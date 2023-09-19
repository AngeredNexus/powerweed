using Microsoft.AspNetCore.Mvc;
using WeedDelivery.Backend.Systems.App.Ordering.Interfaces;

namespace WeedDelivery.Backend.Api.v1.Order;

[ApiVersion("1")]
[Microsoft.AspNetCore.Mvc.Route("api/v{version:apiVersion}/order")]
[ApiController]
[Tags("Customer API")]
public class OrderController : Controller
{

    private readonly IOrderAdminService _orderAdminService;

    public OrderController(IOrderAdminService orderAdminService)
    {
        _orderAdminService = orderAdminService;
    }

    [MapToApiVersion("1")]
    [HttpGet("all")]
    public async Task<IActionResult> GetOrders([FromQuery] bool isHistoricMode)
    {
        var orders = await _orderAdminService.GetAllOrders(isHistoricMode);
        return Json(orders);
    }
    
    [MapToApiVersion("1")]
    [HttpPost("edit")]
    public async Task<IActionResult> EditOrder([FromBody] WeedDatabase.Domain.App.Order order)
    {
        await _orderAdminService.EditOrder(order);
        return Ok();
    }
    
    [MapToApiVersion("1")]
    [HttpGet("remove")]
    public async Task<IActionResult> RemoveOrder([FromBody] Guid removingItemId)
    {
        await _orderAdminService.RemoveOrder(removingItemId);
        return Ok(); 
    }
}