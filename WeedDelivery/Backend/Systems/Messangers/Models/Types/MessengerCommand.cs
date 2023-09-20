using System.Runtime.Serialization;

namespace WeedDelivery.Backend.Systems.Messangers.Models.Types;

public enum MessengerCommand
{
    [EnumUniqueStringName("unknow")]
    Unknown = 0,
    [EnumUniqueStringName("/start")]
    Start = 1,
    [EnumUniqueStringName("/stop")]
    Stop = 2,
    [EnumUniqueStringName("/lang")]
    Language = 3,
    [EnumUniqueStringName("/passwd")]
    Auth = 4,
    [EnumUniqueStringName("/setlang")]
    SetLanguage = 5,
    [EnumUniqueStringName("/status")]
    Status = 6,
}

public class EnumUniqueStringNameAttribute : Attribute {

    public string Name { get; set; }

    public EnumUniqueStringNameAttribute(string name)
    {
        Name = name;
    }
    
}


