
using RecoveryAT;
using BCrypt;
/**
    Name: Carissa Engebose
    Date: 10/24/24
    Description: Implementation for log in and log out services for the user. Right now, it is only used to show
    the main tabbed bar or not because we haven't implemented create account or log in.
    Bugs: None that I know of.
    Reflection: This class didn't take long at all and was only used to display the main tabbed bar if the log in
    or create account buttons were clicked.
**/
public class AuthenticationService
{
public class AuthenticationService {
    public bool IsLoggedIn { get; private set; }
    public string SchoolCode = "THS24"; // just for testing purposes -- need to get from database next milestone

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

    public void Logout() {
        IsLoggedIn = false;
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
