using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProductCoreLibrary.Validations
{
    public static class InputValidation
    {
        /// <summary>
        /// Check the length requirement
        /// Check for complexity (example: at least one uppercase letter, one lowercase letter, one digit, and one special character)
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsPasswordValid(string password)
        {
            
            if (password.Length < 8)
            {
                return false;
            }

            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$"))
            {
                return false;
            }

            //// Check if the password contains common words, phrases, or patterns (customize this list)
            //var commonWords = new List<string> { "password", "123456", "qwerty", "abc123" };
            //if (commonWords.Any(commonWord => password.Contains(commonWord, StringComparison.OrdinalIgnoreCase)))
            //{
            //    return false;
            //}
            return true;
        }

        /// <summary>
        ///  Check the length requirement (e.g., between 8 and 20 characters)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool IsUsernameValid(string username)
        {
            
            if (username.Length < 8 || username.Length > 20)
            {
                return false;
            }

            // Check the character set (alphanumeric, underscores, and hyphens allowed)
            if (!Regex.IsMatch(username, "^[a-zA-Z0-9_-]+$"))
            {
                return false;
            }

            // Check for reserved usernames
            var reservedUsernames = new List<string> { "admin", "root" };
            if (reservedUsernames.Contains(username.ToLower()))
            {
                return false;
            }

            return true;
        }
    }

    public class InvalidInputResponse
    {
        public string? ErrorMessage { get; init; }    
    }
}
