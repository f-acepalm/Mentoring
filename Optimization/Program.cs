using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    class Program
    {
        static void Main(string[] args)
        {
            var salt = new byte[30];
            var password = "123sdfas123fsdfs";
            var hash = GeneratePasswordHashUsingSalt(password, salt);
        }

        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            byte[] hash = GetBytes(passwordText, 20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }

        public static byte[] GetBytes(string password, int count)
        {
            var result = new byte[count];
            var passwordBytes = new UTF8Encoding(false).GetBytes(password);
            var currentIndex = 0;
            while (currentIndex < count)
            {
                result[currentIndex] = passwordBytes[currentIndex % passwordBytes.Length];
                currentIndex++;
            }
            
            return result;
        }
    }
}
