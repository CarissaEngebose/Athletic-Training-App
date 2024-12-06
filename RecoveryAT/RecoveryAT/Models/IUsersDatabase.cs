/**
    Date: 12/06/24
    Description: This interface provides methods to perform operations on user,
                 including retrieval of users, inserting new users, updating 
                 users, and deleting users.
    Bugs: None that we know of.
    Reflection: This was easy to implement.
**/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RecoveryAT
{
    /// <summary>
    /// Interface for managing user data and operations for the RecoveryAT application.
    /// </summary>
    public interface IUsersDatabase
    {
        /// <summary>
        /// Gets the list of all athlete forms in an observable collection.
        /// </summary>
        ObservableCollection<AthleteForm> Forms { get; }

        /// <summary>
        /// Gets the list of all athlete contacts in an observable collection.
        /// </summary>
        ObservableCollection<AthleteContact> Contacts { get; }

        /// <summary>
        /// Inserts a new user into the database.
        /// </summary>
        /// <param name="firstName">User's first name.</param>
        /// <param name="lastName">User's last name.</param>
        /// <param name="email">User's email address.</param>
        /// <param name="hashedPassword">User's hashed password.</param>
        /// <param name="schoolName">User's school name.</param>
        /// <param name="schoolCode">User's school code.</param>
        /// <param name="key">Encryption key for secure storage.</param>
        /// <param name="iv">Initialization vector for encryption.</param>
        /// <returns>A message indicating the result of the operation.</returns>
        string InsertUser(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode, string key, string iv);

        /// <summary>
        /// Deletes a user account based on their email.
        /// </summary>
        /// <param name="email">The email of the user to delete.</param>
        /// <returns>True if the user account was successfully deleted; otherwise, false.</returns>
        bool DeleteUserAccount(string email);

        /// <summary>
        /// Fetches user information based on their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>A dictionary containing user information, or null if not found.</returns>
        Dictionary<string, string> GetUserByEmail(string email);

        /// <summary>
        /// Fetches user information as a User object based on their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>A User object containing user information, or null if not found.</returns>
        User GetUserFromEmail(string email);

        /// <summary>
        /// Checks if a user exists in the database based on their email.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        bool IsEmailRegistered(string email);

        /// <summary>
        /// Updates a user's profile information in the database.
        /// </summary>
        /// <param name="originalEmail">The original email of the user (used as a key).</param>
        /// <param name="firstName">The updated first name.</param>
        /// <param name="lastName">The updated last name.</param>
        /// <param name="schoolName">The updated school name.</param>
        /// <param name="schoolCode">The updated school code.</param>
        /// <param name="email">The updated email.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        bool UpdateUserProfile(string originalEmail, string firstName, string lastName, string schoolName, string schoolCode, string email);
    }
}
