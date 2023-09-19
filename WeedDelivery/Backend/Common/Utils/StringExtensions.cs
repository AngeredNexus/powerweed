using System.Security.Cryptography;
using System.Security.Authentication;
using System.Text;
using ServiceStack.Auth;

namespace WeedDelivery.Backend.Common.Utils;

public static class StringExtensions
{
    public static string ComputeSha512(this string toEncrypt)
    {
        using var sha512 = SHA512.Create();

        byte[] hashValue = sha512.ComputeHash(Encoding.UTF8.GetBytes(toEncrypt));
        return Convert.ToHexString(hashValue);
    }
    
    public static string GetRandomAlphanumericString(int length)
    {
        const string alphanumericCharacters =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz" +
            "0123456789";
        return GetRandomString(length, alphanumericCharacters);
    }
    public static string GetRandomString(int length, IEnumerable<char> characterSet)
    {
        if (length < 0)
            throw new ArgumentException("length must not be negative", nameof(length));
        if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
            throw new ArgumentException("length is too big", nameof(length));
        if (characterSet == null)
            throw new ArgumentNullException(nameof(characterSet));
        var characterArray = characterSet.Distinct().ToArray();
        if (characterArray.Length == 0)
            throw new ArgumentException("characterSet must not be empty", nameof(characterSet));

        var bytes = new byte[length * 8];
        new RNGCryptoServiceProvider().GetBytes(bytes);
        var result = new char[length];
        for (int i = 0; i < length; i++)
        {
            ulong value = BitConverter.ToUInt64(bytes, i * 8);
            result[i] = characterArray[value % (uint)characterArray.Length];
        }
        return new string(result);
    }
}