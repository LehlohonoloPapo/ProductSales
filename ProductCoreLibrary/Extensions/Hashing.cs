using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ProductCoreLibrary.Extensions
{
    public static class Hashing
    {
        public static string CreatePasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Compute hash value from the password
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert the hash bytes to a hexadecimal string
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
        /// <summary>
        /// Compare password with a user input password to verify if they match
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        public static bool CheckPasswordHash(string password, string hashedPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] userInputBytes = Encoding.UTF8.GetBytes(password);
                byte[] userInputHashBytes = sha256.ComputeHash(userInputBytes);
                string userInputHashedPassword = BitConverter.ToString(userInputHashBytes).Replace("-", string.Empty);

                if (hashedPassword.Equals(userInputHashedPassword, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
