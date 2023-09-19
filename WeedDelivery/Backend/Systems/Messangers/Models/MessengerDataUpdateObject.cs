using WeedDatabase.Domain.Common;

namespace WeedDelivery.Backend.Systems.Messangers.Models;

public class MessengerDataUpdateObject
{
    public SmokiUser AppUser { get; set; }
    public string? Message { get; set; }
    public string Hash { get; set; }
}