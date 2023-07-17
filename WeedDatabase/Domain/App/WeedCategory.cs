using WeedDatabase.Domain.App.Interfaces;

namespace WeedDatabase.Domain.App;

public class WeedCategory : DomainObject
{
    public string Name { get; set; }
    
    public bool IsChild { get; set; }
    public Guid ParentCategoryId { get; set; }
}