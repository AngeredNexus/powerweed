namespace Database.Context.Interfaces;

public interface IProductDbContextAcceptor
{
    ProductDbContext CreateContext();
}