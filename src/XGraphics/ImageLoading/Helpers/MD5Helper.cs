using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace XGraphics.ImageLoading.Helpers
{
    public class MD5Helper
    {
        public static string MD5(Stream stream)
        {
            using (var hashProvider = new MD5CryptoServiceProvider())
            {
                var bytes = hashProvider.ComputeHash(stream);
                return ToSanitizedKey(BitConverter.ToString(bytes));
            }
        }

        public static string MD5(string input)
        {
            using (var hashProvider = new MD5CryptoServiceProvider())
            {
                var bytes = hashProvider.ComputeHash(Encoding.UTF8.GetBytes(input));
                return ToSanitizedKey(BitConverter.ToString(bytes));
            }
        }

        private static string ToSanitizedKey(string key)
        {
            return new string(key.ToCharArray()
                .Where(c => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
                .ToArray());
        }
    }
}
