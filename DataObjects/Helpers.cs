using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    static public class Helpers
    {
        public static string HashSha256(string source)
        {
            string hashValue = "";

            byte[] data;

            using (SHA256 sha256Hasher = SHA256.Create())
            {
                data = sha256Hasher.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            var s = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            hashValue = s.ToString();
            return hashValue;
        }
        public static bool IsValidPassword(this string password)
        {
            var isValid = false;

            if (password.Length >= 7)
            {
                isValid = true;
            }

            return isValid;
        }
    }
}
