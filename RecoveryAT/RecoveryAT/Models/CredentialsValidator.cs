/**
    Date: 12/05/24
    Description: Created a class to ensure that an email is in the right format and a password contains a symbol, a number, and is at least
    8 characters to ensure security.
    Reflection: This class didn't take very long and was relatively easy to implement.
**/

using System.Text.RegularExpressions;

namespace RecoveryAT
{
    public class CredentialsValidator
    {
        /// <summary>
        /// Verifies if the password is in the right format.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <returns>True if the email is valid, otherwise false.</returns>
        public static bool isValidPassword(String password)
        {
            if (ValidatePassword(password) == PasswordStatus.Good)
            { // if password status is good
                return true; // is a valid password
            }
            return false; // is not a valid password
        }

        /// <summary>
        /// Verifies if the email is in the right format.
        /// </summary>
        /// <param name="email">The email to verify.</param>
        /// <returns>True if the email is valid, false otherwise.</returns>
        public static bool isValidEmail(String email)
        {
            if (string.IsNullOrEmpty(email))
            { // if email is empty
                return false; // is not valid
            }
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"; // Regular expression pattern to validate email format
            return Regex.IsMatch(email, emailPattern); // is of form name@address
        }

        /// <summary>
        /// Checks to see if the password if validated.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>A password status determining if the password is good or needs to be edited.</returns>
        public static PasswordStatus ValidatePassword(String password)
        {
            if (String.IsNullOrWhiteSpace(password) || password.Length < 8) // has to be 8 or more characters
            {
                return PasswordStatus.TooShort;
            }
            else if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]")) // has to have a symbol
            {
                return PasswordStatus.NoSymbol;
            }
            else if (!Regex.IsMatch(password, @"\d")) // needs to have a number
            {
                return PasswordStatus.NoNumber;
            }
            return PasswordStatus.Good;
        }

        public enum PasswordStatus
        {
            TooShort, // password is shorter than 8 characters
            NoSymbol, // password doesn't contain atleast 1 symbol
            NoNumber, // password doesn't contain atleast 1 number
            Good // password is good
        }

        /// <summary>
        /// Returns a message for the password status.
        /// </summary>
        /// <param name="status">The status of the password.</param>
        /// <returns>The message of whether the password is good or what requirements it doesn't meet.</returns>
        public static string GetMessage(PasswordStatus status) => status switch
        {
            PasswordStatus.TooShort => "The password entered is too short.",
            PasswordStatus.NoSymbol => "The password must contain at least one symbol.",
            PasswordStatus.NoNumber => "The password must contain at least one number.",
            PasswordStatus.Good => "The password is good.",
            _ => "Unknown password status. How?"
        };
    }

}