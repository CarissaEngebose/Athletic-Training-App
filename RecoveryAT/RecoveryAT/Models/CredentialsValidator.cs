using System.Text.RegularExpressions;

namespace RecoveryAT
{
    public class CredentialsValidator
    {
        public static bool isValidPassword(String password)
        {
            if (ValidatePassword(password) == PasswordStatus.Good)
            { // if password status is good
                return true; // is a valid password
            }
            return false; // is not a valid password
        }

        public static bool isValidEmail(String email)
        {
            if (string.IsNullOrEmpty(email))
            { // if email is empty
                return false; // is not valid
            }
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"; // Regular expression pattern to validate email format
            return Regex.IsMatch(email, emailPattern); // is of form name@address
        }

        public static PasswordStatus ValidatePassword(String password)
        {
            if (String.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                return PasswordStatus.TooShort;
            }
            else if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
            {
                return PasswordStatus.NoSymbol;
            }
            else if (!Regex.IsMatch(password, @"\d"))
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

        // a collection of messages associated with each status to make alerts easier
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