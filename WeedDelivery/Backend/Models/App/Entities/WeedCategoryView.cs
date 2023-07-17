using WeedDatabase.Domain;
using WeedDatabase.Domain.App;

namespace WeedDelivery.Backend.Models.App.Entities;

public class WeedCategoryView
{
    public WeedCategory Branch { get; set; } = new();
    private List<WeedCategory> Childs { get; set; } = new();
}