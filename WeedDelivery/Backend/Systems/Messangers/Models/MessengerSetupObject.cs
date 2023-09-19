namespace WeedDelivery.Backend.Systems.Messangers.Models;

public class MessengerSetupObject
{
    public string Token { get; set; } = String.Empty;

    public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
    // public T? SpecificMessengerData { get; set; }
}