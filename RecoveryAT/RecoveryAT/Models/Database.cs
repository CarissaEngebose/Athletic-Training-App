/**
    Name: Carissa Engebose
    Date: 10/27/24
    Description: Database implementation for the RecoveryAT app including functionality to insert, update, delete and select forms from where they are
    stored in the database.
    Bugs: None that I know of.
    Reflection: The database took a little bit of time to set up because of all of the parameters it has but other than
    that it was fine.
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
    public class Database : IDatabase
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
        public Database()
        {
            connString = GetConnectionString();
            Console.WriteLine($"Connected to database: {connString}");
        }

        /// <summary>
        /// Checks if the provided school code is valid in the database.
        /// </summary>
        /// <param name="schoolCode">The school code to validate.</param>
        /// <returns>True if the school code exists, otherwise false.</returns>
        public bool IsValidSchoolCode(string schoolCode)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);

                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT school_code FROM users WHERE school_code = @schoolCode", conn);
                cmd.Parameters.AddWithValue("@schoolCode", schoolCode); // set parameter for school code

                var result = cmd.ExecuteScalar(); // Execute the command and get the result

                // Check if result is not null
                return result != null; // return true if a matching school code was found

            }
            catch (Npgsql.PostgresException ex)
            {
                // Handle any database-specific errors.
                Console.WriteLine($"Database error: {ex.Message}");
            }
            return false;
        }

        /// <summary>
        /// Gets a list of forms for a school code.
        /// </summary>
        /// <param name="schoolCode">The school code of the forms to retrieve.</param>
        /// <returns>The list of forms if they exists; otherwise, null.</returns>
        public ObservableCollection<AthleteForm> SelectFormsBySchoolCode(string schoolCode)
        {
            forms.Clear(); // Clear the local collection

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                using var cmd = new NpgsqlCommand(@"
                    SELECT form_key, schoolCode, first_name, last_name, grade, sport,
                        injured_area, injured_side, treatment_type, athlete_comments,
                        trainer_comments, athlete_status, date_created
                    FROM athlete_forms
                    WHERE school_code = @schoolCode",
                    conn
                );
                cmd.Parameters.AddWithValue("schoolCode", schoolCode); // Set the parameter value.
                using var reader = cmd.ExecuteReader();

                // Loop through each row in the result and add it to the forms collection.
                while (reader.Read())
                {
                    var formKey = reader.GetString(0); // unique identifier for the form
                    var firstName = reader.GetString(2);
                    var lastName = reader.GetString(3);
                    var grade = reader.GetInt16(4);
                    var sport = reader.GetString(5);
                    var injuredArea = reader.GetString(6);
                    var injuredSide = reader.GetString(7);
                    var treatmentType = reader.GetString(8);
                    var athleteComments = reader.IsDBNull(9) ? null : reader.GetString(9);
                    var trainerComments = reader.IsDBNull(10) ? null : reader.GetString(10);
                    var status = reader.IsDBNull(11) ? null : reader.GetString(11);
                    var dateCreated = reader.GetDateTime(12);

                    // Create a new instance of AthleteForm with the retrieved data
                    var form = new AthleteForm(
                        formKey,
                        schoolCode,
                        firstName,
                        lastName,
                        grade,
                        sport,
                        injuredArea,
                        injuredSide,
                        treatmentType,
                        dateCreated,
                        athleteComments,
                        trainerComments,
                        status
                    );
                    forms.Add(form); // Add to the collection
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                // Handle any database-specific errors.
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return forms;
        }

        /// <summary>
        /// Gets a list of forms for a school code.
        /// </summary>
        /// <param name="schoolCode">The school code of the forms to retrieve.</param>
        /// <param name="dateCreated">The date of the forms to retrieve.</param>
        /// <returns>The list of forms if they exists; otherwise, null.</returns>
        public ObservableCollection<AthleteForm> SelectFormsByDate(string schoolCode, DateTime dateCreated)
        {
            forms.Clear(); // Clear the local collection

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                using var cmd = new NpgsqlCommand(@"
                    SELECT form_key, schoolCode, first_name, last_name, grade, sport,
                        injured_area, injured_side, treatment_type, athlete_comments,
                        trainer_comments, athlete_status, date_created
                    FROM athlete_forms
                    WHERE school_code = @schoolCode
                    AND date_created = @dateCreated",
                    conn
                );
                cmd.Parameters.AddWithValue("schoolCode", schoolCode); // Set the parameter value.
                cmd.Parameters.AddWithValue("dateCreated", dateCreated);
                using var reader = cmd.ExecuteReader();

                // Loop through each row in the result and add it to the forms collection.
                while (reader.Read())
                {
                    var formKey = reader.GetString(0); // unique identifier for the form
                    var firstName = reader.GetString(2);
                    var lastName = reader.GetString(3);
                    var grade = reader.GetInt16(4);
                    var sport = reader.GetString(5);
                    var injuredArea = reader.GetString(6);
                    var injuredSide = reader.GetString(7);
                    var treatmentType = reader.GetString(8);
                    var athleteComments = reader.IsDBNull(9) ? null : reader.GetString(9);
                    var trainerComments = reader.IsDBNull(10) ? null : reader.GetString(10);
                    var status = reader.IsDBNull(11) ? null : reader.GetString(11);

                    // Create a new instance of AthleteForm with the retrieved data
                    var form = new AthleteForm(
                        formKey,
                        schoolCode,
                        firstName,
                        lastName,
                        grade,
                        sport,
                        injuredArea,
                        injuredSide,
                        treatmentType,
                        dateCreated,
                        athleteComments,
                        trainerComments,
                        status
                    );
                    forms.Add(form); // Add to the collection
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                // Handle any database-specific errors.
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return forms;
        }

        /// <summary>
        /// Gets a specific form by its key.
        /// </summary>
        /// <param name="formKey">The key of the form to retrieve.</param>
        /// <returns>The form if it exists; otherwise, null.</returns>
        public ObservableCollection<AthleteForm> SelectForm(string formKey)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                using var cmd = new NpgsqlCommand("SELECT form_key, schoolCode, first_name, last_name, grade, sport, injured_area, injured_side, treatment_type, athlete_comments, trainer_comments, athlete_status, date_created FROM athlete_forms WHERE form_key = @formKey", conn);
                cmd.Parameters.AddWithValue("formKey", formKey); // Set the parameter value.
                using var reader = cmd.ExecuteReader();

                // If a form is found, add it to the local collection.
                if (reader.Read())
                {
                    var schoolCode = reader.GetString(1);
                    var firstName = reader.GetString(2);
                    var lastName = reader.GetString(3);
                    var grade = reader.GetInt16(4);
                    var sport = reader.GetString(5);
                    var injuredArea = reader.GetString(6);
                    var injuredSide = reader.GetString(7);
                    var treatmentType = reader.GetString(8);
                    var athleteComments = reader.IsDBNull(9) ? null : reader.GetString(9);
                    var trainerComments = reader.IsDBNull(10) ? null : reader.GetString(10);
                    var status = reader.IsDBNull(11) ? null : reader.GetString(11);
                    var dateCreated = reader.GetDateTime(12);

                    // Create a new instance of AthleteForm with the retrieved data
                    var form = new AthleteForm(
                        formKey,
                        schoolCode,
                        firstName,
                        lastName,
                        grade,
                        sport,
                        injuredArea,
                        injuredSide,
                        treatmentType,
                        dateCreated,
                        athleteComments,
                        trainerComments,
                        status
                    );
                    forms.Add(form);
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                // Handle any database-specific errors.
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return forms; // Return the collection of forms.
        }

        /// <summary>
        /// Inserts a new form into the database with simplified error handling.
        /// </summary>
        /// <param name="form">The form to insert.</param>
        /// <returns>A message indicating if the form was successfully inserted or not.</returns>
        public string InsertForm(AthleteForm form)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // Start a transaction for the insertion.
                using var transaction = conn.BeginTransaction();
                using var cmd = new NpgsqlCommand
                {
                    Connection = conn,
                    CommandText = @"INSERT INTO athlete_forms 
                            (school_code, first_name, last_name, grade, sport, injured_area, injured_side, 
                            treatment_type, athlete_comments, trainer_comments, athlete_status, date_created) 
                            VALUES 
                            (@schoolCode, @firstName, @lastName, @grade, @sport, @injuredArea, @injuredSide, 
                            @treatmentType, @athleteComments, @trainerComments, @status, @dateCreated)"
                };

                // Set parameter values for the insert query.
                cmd.Parameters.AddWithValue("schoolCode", form.SchoolCode);
                cmd.Parameters.AddWithValue("firstName", form.FirstName);
                cmd.Parameters.AddWithValue("lastName", form.LastName);
                cmd.Parameters.AddWithValue("grade", form.Grade);
                cmd.Parameters.AddWithValue("sport", form.Sport);
                cmd.Parameters.AddWithValue("injuredArea", form.InjuredArea);
                cmd.Parameters.AddWithValue("injuredSide", form.InjuredSide);
                cmd.Parameters.AddWithValue("treatmentType", form.TreatmentType);
                cmd.Parameters.AddWithValue("athleteComments", (object?)form.AthleteComments ?? DBNull.Value);
                cmd.Parameters.AddWithValue("trainerComments", (object?)form.TrainerComments ?? DBNull.Value);
                cmd.Parameters.AddWithValue("status", (object?)form.Status ?? DBNull.Value);
                cmd.Parameters.AddWithValue("dateCreated", form.Date); // set to current date and time

                // Execute the command and check the number of rows affected.
                int numRowsAffected = cmd.ExecuteNonQuery();

                // Commit the transaction if the insertion is successful.
                transaction.Commit();

                if (numRowsAffected > 0)
                {
                    forms.Add(form); // Add the form to the local collection.
                    return "Form added successfully.";
                }
                else
                {
                    return "No rows were affected. The form was not added.";
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                // Handle any database-specific errors.
                Console.WriteLine($"Database error: {ex.Message}");
                return "An error occurred while adding the form.";
            }
            catch (Exception ex)
            {
                // Handle general errors.
                Console.WriteLine($"General error: {ex.Message}");
                return "Error adding form.";
            }
        }

        /// <summary>
        /// Deletes a form from the database by its form key
        /// </summary>
        /// <param name="formKey">The key of the form to delete.</param>
        /// <returns>A message indicating whether the form was deleted from the database.</returns>
        public string DeleteForm(string formKey)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand("DELETE FROM athlete_forms WHERE form_key = @formKey", conn);
                cmd.Parameters.AddWithValue("formKey", formKey);

                int numRowsAffected = cmd.ExecuteNonQuery();

                // If the deletion is successful, remove the airport from the local collection.
                if (numRowsAffected > 0)
                {
                    var formToDelete = forms.FirstOrDefault(a => a.FormKey == formKey);
                    if (formToDelete != null)
                    {
                        forms.Remove(formToDelete); // Update the local collection.
                    }
                    return "Form deleted successfully.";
                }
                else
                {
                    return "Form not found in the database.";
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                // Handle any database-specific errors.
                Console.WriteLine($"Database error: {ex.Message}");
                return $"Error while deleting Form from the database: {ex.Message}";
            }
            catch (Exception ex)
            {
                // Handle general errors.
                Console.WriteLine($"General error: {ex.Message}");
                return $"An unexpected error occurred while deleting the Form: {ex.Message}";
            }
        }

        /// <summary>
        /// Updates an existing form's details in the database.
        /// </summary>
        /// <param name="form">The form object with updated information.</param>
        /// <returns>A message indicating whether the form was successfully updated or not.</returns>
        public string UpdateForm(AthleteForm form)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // Set up the update command.
                var cmd = new NpgsqlCommand
                {
                    Connection = conn,
                    CommandText = "UPDATE athlete_forms SET trainer_comments = @trainerComments, athlete_status = @status WHERE form_key = @formKey"
                };
                cmd.Parameters.AddWithValue("formKey", form.FormKey);
                cmd.Parameters.AddWithValue("trainerComments", form.TrainerComments ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("status", form.Status ?? (object)DBNull.Value);

                // Execute the update command and check if rows were affected.
                int numRowsAffected = cmd.ExecuteNonQuery();
                if (numRowsAffected > 0)
                {
                    var index = forms.IndexOf(forms.First(a => a.FormKey == form.FormKey));
                    if (index >= 0)
                    {
                        forms[index] = form; // Update the local collection.
                    }
                    return "Athlete form updated successfully.";
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                // Handle any database-specific errors.
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return "Error while updating athlete form.";
        }

        /// <summary>
        /// Inserts a new user into the database.
        /// </summary>
        /// <param name="firstName">User's first name.</param>
        /// <param name="lastName">User's last name.</param>
        /// <param name="email">User's email address.</param>
        /// <param name="hashedPassword">User's hashed password.</param>
        /// <returns>A message indicating the result of the insertion.</returns>
        public string InsertUser(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
                INSERT INTO users (first_name, last_name, email, hashed_password, school_name, school_code)
                VALUES (@firstName, @lastName, @email, @hashedPassword, @schoolName, @schoolCode)", conn);

                cmd.Parameters.AddWithValue("firstName", firstName);
                cmd.Parameters.AddWithValue("lastName", lastName);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("hashedPassword", hashedPassword);
                cmd.Parameters.AddWithValue("schoolName", schoolName);
                cmd.Parameters.AddWithValue("schoolCode", schoolCode);

                int rowsAffected = cmd.ExecuteNonQuery();

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
