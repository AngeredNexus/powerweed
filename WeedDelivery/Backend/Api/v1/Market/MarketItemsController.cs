using Microsoft.AspNetCore.Mvc;
using WeedDatabase.Domain;
using WeedDatabase.Domain.App;

namespace WeedDelivery.Backend.Api.v1.Market;

[ApiVersion("1")]
[Microsoft.AspNetCore.Mvc.Route("api/v{version:apiVersion}/market")]
[ApiController]
[Tags("Customer API")]
public class MarketItemsController : Controller
{

    // CRUD
    
    [MapToApiVersion("1")]
    [HttpGet("get-all")]
    public async Task<IAsyncResult> GetAllItems([FromQuery] bool isHistoricMode)
    {
        return null;
    }
    
    [MapToApiVersion("1")]
    [HttpPost("create")]
    public async Task<IAsyncResult> CreateItem([FromBody] WeedItem item)
    {
        return null;
    }
    
    [MapToApiVersion("1")]
    [HttpPost("edit")]
    public async Task<IAsyncResult> EditItem([FromBody] WeedItem item)
    {
        return null;
    }
    
    [MapToApiVersion("1")]
    [HttpPost("remove")]
    public async Task<IAsyncResult> RemoveItem([FromQuery] Guid itemId)
    {
        return null;
    }
}