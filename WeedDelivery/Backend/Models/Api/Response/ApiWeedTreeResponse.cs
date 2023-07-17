using WeedDelivery.Backend.Models.App.Entities;

namespace WeedDelivery.Backend.Models.Api.Response;

public class ApiWeedTreeResponse
{
    public List<WeedCategoryView> Tree { get; set; } = new();
}