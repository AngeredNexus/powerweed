using Microsoft.AspNetCore.Mvc;
using WeedDelivery.Backend.App.Ordering;
using WeedDelivery.Backend.App.Ordering.Interfaces;

namespace WeedDelivery.Backend.Api.v1.Order;

public class OrderController : Controller
{

    private readonly IOrderAdminService _orderAdminService;

    public OrderController(IOrderAdminService orderAdminService)
    {
        _orderAdminService = orderAdminService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetOrders([FromQuery] bool isHistoricMode)
    {
        var orders = await _orderAdminService.GetAllOrders(isHistoricMode);
        return Json(orders);
    }
    
    [HttpPost("edit")]
    public async Task<IActionResult> EditOrder([FromBody] WeedDatabase.Domain.App.Order order)
    {
        await _orderAdminService.EditOrder(order);
        return Ok();
    }
    
    [HttpGet("remove")]
    public async Task<IActionResult> RemoveOrder([FromBody] Guid removingItemId)
    {
        await _orderAdminService.RemoveOrder(removingItemId);
        return Ok(); 
    }
}