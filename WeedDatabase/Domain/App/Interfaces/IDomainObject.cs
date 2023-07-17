namespace WeedDatabase.Domain.App.Interfaces;

public interface IDomainObject
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}