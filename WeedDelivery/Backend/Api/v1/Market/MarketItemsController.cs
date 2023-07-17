using Microsoft.AspNetCore.Mvc;
using WeedDatabase.Domain;
using WeedDatabase.Domain.App;

namespace WeedDelivery.Backend.Api.v1.Market;

public class MarketItemsController : Controller
{

    // CRUD
    
    [HttpGet("get-all")]
    public async Task<IAsyncResult> GetAllItems([FromQuery] bool isHistoricMode)
    {
        return null;
    }
    
    [HttpPost("create")]
    public async Task<IAsyncResult> CreateItem([FromBody] WeedItem item)
    {
        return null;
    }
    
    [HttpPost("edit")]
    public async Task<IAsyncResult> EditItem([FromBody] Guid oldItemId, [FromBody] WeedItem item)
    {
        return null;
    }
    
    [HttpPost("remove")]
    public async Task<IAsyncResult> RemoveItem([FromQuery] Guid itemId)
    {
        return null;
    }
}