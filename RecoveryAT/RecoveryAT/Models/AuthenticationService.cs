using RecoveryAT;
using BCrypt;
public class AuthenticationService
{
    public bool IsLoggedIn { get; private set; }
    private string _loggedInUserEmail;

    /// <summary>
    /// Logs in a user and sets the logged-in user's email.
    /// </summary>
    /// <param name="email">The email of the user logging in.</param>
    public void Login(string email)
    {
        IsLoggedIn = true;
        _loggedInUserEmail = email;
    }

    public void Logout()
    {
        IsLoggedIn = false;
        _loggedInUserEmail = null;
    }

    /// <summary>
    /// Gets the email of the currently logged-in user.
    /// </summary>
    /// <returns>The logged-in user's email, or null if no user is logged in.</returns>
    public string GetLoggedInUserEmail()
    {
        return _loggedInUserEmail;
    }

    /// <summary>
    /// Gets the SchoolCode for the currently logged-in user.
    /// </summary>
    /// <returns>The SchoolCode for the user, or a default value if not found.</returns>
    public string GetSchoolCode()
    {
        if (string.IsNullOrWhiteSpace(_loggedInUserEmail))
            return "DefaultCode"; // Fallback if no user is logged in

        var businessLogic = ((App)Application.Current).BusinessLogic;
        var userData = businessLogic.GetUserByEmail(_loggedInUserEmail);

        if (userData != null && userData.ContainsKey("SchoolCode"))
        {
            return userData["SchoolCode"];
        }

        return "DefaultCode"; // Fallback if SchoolCode is not available
    }

    /// <summary>
    /// Hash a given password
    /// </summary>
    /// <param name="password">password to hash</param>
    /// <returns>hashed password</returns>
    public static String HashPassword(String password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
