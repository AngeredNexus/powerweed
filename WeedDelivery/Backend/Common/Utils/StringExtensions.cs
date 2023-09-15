using System.Security.Cryptography;
using System.Text;

namespace WeedDelivery.Backend.Common.Utils;

public static class StringExtensions
{
    public static string ComputeSha512(this string toEncrypt)
    {
        using var sha512 = SHA512.Create();

        byte[] hashValue = sha512.ComputeHash(Encoding.UTF8.GetBytes(toEncrypt));
        return Convert.ToHexString(hashValue);
    }
}