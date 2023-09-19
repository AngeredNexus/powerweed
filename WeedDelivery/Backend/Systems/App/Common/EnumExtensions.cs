using System.Diagnostics.CodeAnalysis;
using WeedDelivery.Backend.Systems.Messangers.Models.Types;

namespace WeedDelivery.Backend.Systems.App.Common;

public static class EnumExtensions
{
    public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
    {
        var enumType = value.GetType();
        var name = Enum.GetName(enumType, value);
        return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
    }
    
    public static T GetEnumValueByUniqueName<T>(this string name) where T : struct, Enum
    {
        var findedEnumValue = Enum.GetValues<T>().FirstOrDefault(x => x.GetAttribute<EnumUniqueStringNameAttribute>().Name == name);
        return findedEnumValue;
        
    }
}