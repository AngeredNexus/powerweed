using WeedDatabase.Domain;
using WeedDatabase.Domain.App;

namespace WeedDelivery.Backend.Models.Api.Response;

public class ApiWeedSearchResponse
{
    public List<WeedItem> Items { get; set; } = new();
}