namespace WeedDatabase.Context.Interfaces;

public interface IWeedContextAcceptor
{
    WeedContext CreateContext();
}