using Newtonsoft.Json;

namespace Database.Utils;

public static class TypeFunctions
{
    public static K Cast<T, K>(T origin) where T: class
    {
        var serialiazed = JsonConvert.SerializeObject(origin);
        K? casted = JsonConvert.DeserializeObject<K>(serialiazed);

        if (casted is not null)
            return casted;

        throw new TypeAccessException($"Requested obj of ({typeof(T)}) cannot be casted to ({typeof(K)})!");
    }
}