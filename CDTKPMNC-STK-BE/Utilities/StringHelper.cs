using System.Globalization;
using System.Text;
using System.Security.Cryptography;

namespace CDTKPMNC_STK_BE.Utilities
{
    static public class StringHelper
    {
        static public string ToTitleCase(this string s)
        {
            s = s.ToLower();
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
        }

        static public string ToHashSHA256(this string s)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(s);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            string hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }

        static public Guid? ToGuid(this string s)
        {
            if (s == null) { return null; }
            return new Guid(s);
        }
    }
}
