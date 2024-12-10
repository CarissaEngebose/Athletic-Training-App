/**
    Date: 12/06/24
    Description: Implementation of a user for the program.
    Bugs: None that we know of.
    Reflection: This class didn't take long at all and is used in place of the Authentication Service file.
**/

public class User {
    public bool IsLoggedIn { get; private set; }
    public string? FirstName;
    public string? LastName;
    public string? FullName => $"{FirstName} {LastName}";
    public string? Email;
    public string? HashedPassword;
    public string? HashedSecurityQuestions;
    public string? SchoolName; // encrypted school name
    public string? SchoolCode; 
    public string? IV; // initialization vector for encryption/decryption
    public string? Key; // key for encryption/decryption

    /// <summary>
    /// Creates a user with all the values attached.
    /// </summary>
    /// <param name="firstName">User's first name.</param>
    /// <param name="lastName">User's last name.</param>
    /// <param name="email">User's email address.</param>
    /// <param name="hashedPassword">User's hashed password.</param>
    /// <param name="schoolName">User's school name.</param>
    /// <param name="schoolCode">User's school code.</param>
    /// <param name="key">A key to encrypt the school name.</param>
    /// <param name="iv">An iv to encrypt the school name.</param>
    public User(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode, string key, string iv, string hashedSecurityQuestions)
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
        HashedSecurityQuestions = hashedSecurityQuestions;
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
