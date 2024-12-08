/**
    Date: 12/05/24
    Description: Database implementation for the RecoveryAT app including functionality to insert, update, delete and select forms and users
    from where they are stored in the database.
    Bugs: None that I know of.
    Reflection: The database took a quite a bit of time because we started going screen by screen and implementing the database that way
    which can take a bit of time for each sprint but overall we think it went well.
**/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace RecoveryAT
{
    /// <summary>
    /// Provides methods to interact with the form data stored in a PostgreSQL database.
    /// </summary>
    public class UsersDatabase : IUsersDatabase
    {
        // Connection string to the PostgreSQL database.
        private readonly string connString;

        // ObservableCollection to store the list of forms.
        private ObservableCollection<AthleteForm> forms = new ObservableCollection<AthleteForm>();

        // Property to expose the list of forms.
        public ObservableCollection<AthleteForm> Forms => forms;

        // ObservableCollection to store the list of contacts.
        private ObservableCollection<AthleteContact> contacts = new ObservableCollection<AthleteContact>();

        // Property to expose the list of contacts.
        public ObservableCollection<AthleteContact> Contacts => contacts;


        // Constructor that sets up the connection string and fetches all forms.
        public UsersDatabase()
        {
            connString = GetConnectionString();
            Console.WriteLine($"Connected to database: {connString}");
        }

        /// <summary>
        /// Inserts a new user into the database.
        /// </summary>
        /// <param name="firstName">User's first name.</param>
        /// <param name="lastName">User's last name.</param>
        /// <param name="email">User's email address.</param>
        /// <param name="hashedPassword">User's hashed password.</param>
        /// <param name="key">A key used for encryption.</param>
        /// <param name="iv">An iv used for encryption.</param>
        /// <returns>A message indicating the result of the insertion.</returns>
        public string InsertUser(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode, string key, string iv, string hashedSecurityQuestions)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // insert the user using the parameters
                using var cmd = new NpgsqlCommand(@"
                INSERT INTO users (first_name, last_name, email, hashed_password, school_name, school_code, encryption_key, encryption_iv)
                VALUES (@firstName, @lastName, @email, @hashedPassword, @schoolName, @schoolCode, @key, @iv)", conn);

                cmd.Parameters.AddWithValue("firstName", firstName); // set the parameters to insert the form into the database
                cmd.Parameters.AddWithValue("lastName", lastName);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("hashedPassword", hashedPassword);
                cmd.Parameters.AddWithValue("schoolName", schoolName);
                cmd.Parameters.AddWithValue("schoolCode", schoolCode);
                cmd.Parameters.AddWithValue("key", key);
                cmd.Parameters.AddWithValue("iv", iv);

                int rowsAffected = cmd.ExecuteNonQuery();

                // if rows were affected, return a successful message, otherwise return failed message
                return rowsAffected > 0 ? "User account created successfully." : "Failed to create user account.";
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return "An error occurred while creating the user account.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return "Error creating user account.";
            }
        }

        /// <summary>
        /// Deletes a user account based on their email.
        /// </summary>
        /// <param name="email">The email of the user to delete.</param>
        /// <returns>True if the user account was successfully deleted, otherwise false.</returns>
        public bool DeleteUserAccount(string email)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // delete the user where the email matches the parameter
                using var cmd = new NpgsqlCommand("DELETE FROM users WHERE email = @Email", conn);

                cmd.Parameters.AddWithValue("Email", email); // set the parameter for email to delete by

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0; // Return true if the account was deleted
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Fetches user information based on their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>A dictionary containing user information, or null if not found.</returns>
        public Dictionary<string, string> GetUserByEmail(string email)
        {
            var userInfo = new Dictionary<string, string>();

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // selects user information from the database using the email
                using var cmd = new NpgsqlCommand(@"
                SELECT first_name, last_name, email, school_name, school_code
                FROM public.users
                WHERE email = @Email", conn);

                cmd.Parameters.AddWithValue("Email", email);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                { // creates a dictionary to store the user information
                    userInfo["FirstName"] = reader.GetString(0);
                    userInfo["LastName"] = reader.GetString(1);
                    userInfo["Email"] = reader.GetString(2);
                    userInfo["SchoolName"] = reader.GetString(3);
                    userInfo["SchoolCode"] = reader.GetString(4);
                }
                else
                {
                    return null; // User not found
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user data: {ex.Message}");
                return null;
            }

            return userInfo;
        }

        /// <summary>
        /// Fetches user information based on their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>A User containing user information, or null if not found.</returns>
        public User GetUserFromEmail(string email)
        {
            User user;

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // selects the user information using the email
                using var cmd = new NpgsqlCommand(@"
                SELECT first_name, last_name, email, hashed_password, school_name, school_code, encryption_key, encryption_iv, hashed_security_questions
                FROM public.users
                WHERE email = @Email", conn);

                cmd.Parameters.AddWithValue("Email", email); // set the email parameter to search the database

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User( // create a new user using the information returned by the database
                        firstName: reader.GetString(0),
                        lastName: reader.GetString(1),
                        email: reader.GetString(2),
                        hashedPassword: reader.GetString(3),
                        schoolName: reader.GetString(4),
                        schoolCode: reader.GetString(5),
                        key: reader.GetString(6),
                        iv: reader.GetString(7),
                        hashedSecurityQuestions: reader.GetString(8)
                    );
                    return user;
                }
                else
                {
                    return null; // User not found
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user data: {ex.Message}");
                return null;
            }

        }

        /// <summary>
        /// Checks if a user exists in the database based on their email.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        public bool IsEmailRegistered(string email)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // determines if the email is in the database
                using var cmd = new NpgsqlCommand(@"
                SELECT COUNT(1)
                FROM public.users
                WHERE email = @Email", conn);

                cmd.Parameters.AddWithValue("Email", email); // sets the parameter to search the database for the email

                var result = cmd.ExecuteScalar();
                return result != null && (long)result > 0; // True if email exists, false otherwise
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return false;
            }
        }

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
        public bool UpdateUserProfile(string originalEmail, string firstName, string lastName, string schoolName, string schoolCode, string email)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // updates the database using the parameters
                using var cmd = new NpgsqlCommand(@"
                UPDATE public.users
                SET first_name = @FirstName, 
                    last_name = @LastName, 
                    school_name = @SchoolName, 
                    school_code = @SchoolCode, 
                    email = @Email
                WHERE email = @OriginalEmail", conn);

                cmd.Parameters.AddWithValue("FirstName", firstName); // set the parameters used to update the user information
                cmd.Parameters.AddWithValue("LastName", lastName);
                cmd.Parameters.AddWithValue("SchoolName", schoolName);
                cmd.Parameters.AddWithValue("SchoolCode", schoolCode);
                cmd.Parameters.AddWithValue("Email", email);
                cmd.Parameters.AddWithValue("OriginalEmail", originalEmail);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Updates the password for a user in the database.
        /// </summary>
        /// <param name="email">The email of the user whose password is being updated.</param>
        /// <param name="hashedPassword">The new hashed password to store for the user.</param>
        /// <returns>
        /// True if the password was successfully updated in the database; otherwise, false.
        /// </returns>
        public bool UpdateUserPassword(string email, string hashedPassword)
        {
            try
            {
                // Define the SQL query to update the user's hashed password based on their email.
                string query = "UPDATE users SET hashed_password = @HashedPassword WHERE email = @Email";

                // Use NpgsqlConnection to connect to the PostgreSQL database.
                using var connection = new NpgsqlConnection(connString); // Initialize the connection to the database.
                connection.Open(); // Open the database connection.

                // Use NpgsqlCommand to execute the SQL query.
                using var command = new NpgsqlCommand(query, connection);

                // Add parameters to prevent SQL injection.
                command.Parameters.AddWithValue("@Email", email); // Bind the email parameter to the query.
                command.Parameters.AddWithValue("@HashedPassword", hashedPassword); // Bind the hashed password parameter to the query.

                // Execute the SQL query and get the number of rows affected.
                int rowsAffected = command.ExecuteNonQuery();

                // Return true if one or more rows were updated; otherwise, return false.
                return rowsAffected > 0;
            }
            catch (Npgsql.PostgresException ex)
            {
                // Log PostgreSQL-specific errors.
                Console.WriteLine($"PostgreSQL error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Log general errors.
                Console.WriteLine($"General error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Builds a ConnectionString, which is used to connect to the database.
        /// </summary>
        /// <returns>The connection string to the PostgreSQL database.</returns>
        static string GetConnectionString()
        {
            var connStringBuilder = new NpgsqlConnectionStringBuilder();

            connStringBuilder.Host = "posh-bulldog-13394.5xj.gcp-us-central1.cockroachlabs.cloud";
            connStringBuilder.Port = 26257;  // Default CockroachDB port
            connStringBuilder.SslMode = SslMode.Require;  // SSL mode to ensure security
            connStringBuilder.Username = "carissae";  // Your CockroachDB username
            connStringBuilder.Password = "newpassword123!";  // Your CockroachDB password
            connStringBuilder.Database = "recoveryat";  // Database name
            connStringBuilder.ApplicationName = "RecoveryAT";  // You can set this to your app name

            return connStringBuilder.ConnectionString;
        }
    }
}