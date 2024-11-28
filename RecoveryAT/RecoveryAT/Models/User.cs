/**
    Name: Carissa Engebose
    Date: 11/26/24
    Description: Implementation of a user for the program.
    Reflection: This class didn't take long at all and is used in place of the Authentication Service file.
**/

public class User {
    public bool IsLoggedIn { get; private set; }
    public string? FirstName;
    public string? LastName;
    public string? FullName => $"{FirstName} {LastName}";
    public string? Email;
    public string? HashedPassword;
    public string? SchoolName;
    public string? SchoolCode; 
    public string? IV;
    public string? Key;

    /// <summary>
    /// Creates a user with all the values attached.
    /// </summary>
    public User(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode, string key, string iv)
    {
        IsLoggedIn = true;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        HashedPassword = hashedPassword;
        SchoolName = schoolName;
        SchoolCode = schoolCode;
        Key = key;
        IV = iv; 
    }

    /// <summary>
    /// Sets logged in to false when a user is created with no parameters
    /// </summary>
    public User()
    {
        IsLoggedIn = false;
    }

    /// <summary>
    /// Logs in a user and sets the logged-in user's email.
    /// </summary>
    /// <param name="email">The email of the user logging in.</param>
    public void Login(string email)
    {
        IsLoggedIn = true;
        Email = email;
    }

    /// <summary>
    /// Sets all the values of the user to null
    /// </summary>
    public void Logout() {
        IsLoggedIn = false;
        FirstName = null;
        LastName = null;
        Email = null;
        HashedPassword = null;
        SchoolName = null;
        SchoolCode = null;
        IV = null;
        Key = null;
    }

    /// <summary>
    /// Gets the email of the currently logged-in user.
    /// </summary>
    /// <returns>The logged-in user's email, or null if no user is logged in.</returns>
    public string GetLoggedInUserEmail()
    {
        return Email;
    }
}
