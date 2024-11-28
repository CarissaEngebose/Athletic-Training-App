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
            forms.Clear();

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                using var cmd = new NpgsqlCommand(@"
                SELECT form_key, school_code, first_name, last_name, sport, injured_area,
                       injured_side, treatment_type, athlete_comments, athlete_status, 
                       date_created, date_seen, date_of_birth, key, iv
                FROM athlete_forms
                WHERE school_code = @school_code", conn);
                cmd.Parameters.AddWithValue("school_code", schoolCode);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var form = new AthleteForm(
                        formKey: reader.GetInt64(0),
                        schoolCode: reader.GetString(1),
                        firstName: reader.GetString(2),
                        lastName: reader.GetString(3),
                        sport: reader.GetString(4),
                        injuredArea: reader.GetString(5),
                        injuredSide: reader.GetString(6),
                        treatmentType: reader.GetString(7),
                        athleteComments: reader.IsDBNull(8) ? null : reader.GetString(8),
                        status: reader.IsDBNull(9) ? null : reader.GetString(9),
                        dateCreated: reader.IsDBNull(10) ? DateTime.Now : reader.GetDateTime(10),
                        dateSeen: reader.IsDBNull(11) ? null : reader.GetDateTime(11),
                        dateOfBirth: reader.GetDateTime(12),
                        key: reader.GetString(13),
                        iv: reader.GetString(14)
                    );
                    forms.Add(form);
                }
            }
            catch (Npgsql.PostgresException ex)
            {
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
            SELECT form_key, school_code, first_name, last_name, sport,
                   injured_area, injured_side, treatment_type, athlete_comments,
                   athlete_status, date_created, date_seen, date_of_birth, key, iv
            FROM athlete_forms
            WHERE school_code = @school_code
            AND date_created = @dateCreated",
                    conn
                );
                cmd.Parameters.AddWithValue("school_code", schoolCode); // Set the parameter value.
                cmd.Parameters.AddWithValue("dateCreated", dateCreated);
                using var reader = cmd.ExecuteReader();

                // Loop through each row in the result and add it to the forms collection.
                while (reader.Read())
                {
                    var formKey = reader.GetInt64(0); // unique identifier for the form
                    var firstName = reader.GetString(2);
                    var lastName = reader.GetString(3);
                    var sport = reader.GetString(4);
                    var injuredArea = reader.GetString(5);
                    var injuredSide = reader.GetString(6);
                    var treatmentType = reader.GetString(7);
                    var athleteComments = reader.IsDBNull(8) ? null : reader.GetString(8);
                    var status = reader.IsDBNull(9) ? null : reader.GetString(9);
                    var dateSeen = reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11);
                    var dateOfBirth = reader.GetDateTime(12);
                    var key = reader.GetString(13);
                    var iv = reader.GetString(14);

                    // Create a new instance of AthleteForm with the retrieved data
                    var form = new AthleteForm(
                        formKey: formKey,
                        schoolCode: schoolCode,
                        firstName: firstName,
                        lastName: lastName,
                        sport: sport,
                        injuredArea: injuredArea,
                        injuredSide: injuredSide,
                        treatmentType: treatmentType,
                        dateCreated: dateCreated,
                        dateSeen: dateSeen,
                        dateOfBirth: dateOfBirth,
                        athleteComments: athleteComments,
                        status: status,
                        key: key,
                        iv: iv
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
        /// Gets a list of forms for a school code filtered by the date_seen parameter.
        /// </summary>
        /// <param name="schoolCode">The school code of the forms to retrieve.</param>
        /// <param name="dateSeen">The date when the forms were seen.</param>
        /// <returns>An ObservableCollection of forms that were seen on the specified date.</returns>
        public ObservableCollection<AthleteForm> SelectFormsByDateSeen(string schoolCode, DateTime dateSeen)
        {
            var formsByDateSeen = new ObservableCollection<AthleteForm>(); // Create a new collection to hold the results

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
            SELECT form_key, school_code, first_name, last_name, sport,
                   injured_area, injured_side, treatment_type, athlete_comments,
                   athlete_status, date_created, date_seen, date_of_birth, key, iv
            FROM athlete_forms
            WHERE school_code = @school_code
            AND date_seen = @dateSeen", conn);

                cmd.Parameters.AddWithValue("school_code", schoolCode); // Set the school code parameter
                cmd.Parameters.AddWithValue("dateSeen", dateSeen);      // Set the date_seen parameter

                using var reader = cmd.ExecuteReader();

                // Loop through each row in the result and add it to the formsByDateSeen collection
                while (reader.Read())
                {
                    var formKey = reader.GetInt64(0);
                    var firstName = reader.GetString(2);
                    var lastName = reader.GetString(3);
                    var sport = reader.GetString(4);
                    var injuredArea = reader.GetString(5);
                    var injuredSide = reader.GetString(6);
                    var treatmentType = reader.GetString(7);
                    var athleteComments = reader.IsDBNull(8) ? null : reader.GetString(8);
                    var status = reader.IsDBNull(9) ? null : reader.GetString(9);
                    var dateCreated = reader.GetDateTime(10);
                    var dateOfBirth = reader.GetDateTime(12);
                    var key = reader.GetString(13);
                    var iv = reader.GetString(14);

                    // Create a new instance of AthleteForm with the retrieved data
                    var form = new AthleteForm(
                        formKey: formKey,
                        schoolCode: schoolCode,
                        firstName: firstName,
                        lastName: lastName,
                        sport: sport,
                        injuredArea: injuredArea,
                        injuredSide: injuredSide,
                        treatmentType: treatmentType,
                        dateCreated: dateCreated,
                        dateSeen: dateSeen,
                        dateOfBirth: dateOfBirth,
                        athleteComments: athleteComments,
                        status: status,
                        key: key,
                        iv: iv
                    );
                    formsByDateSeen.Add(form); // Add to the collection
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                // Handle any database-specific errors.
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return formsByDateSeen; // Return the collection of forms filtered by date_seen
        }

        /// <summary>
        /// Gets a specific form by its key.
        /// </summary>
        /// <param name="formKey">The key of the form to retrieve.</param>
        /// <returns>The form if it exists; otherwise, null.</returns>
        public ObservableCollection<AthleteForm> SelectForm(long formKey)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                using var cmd = new NpgsqlCommand(@"
            SELECT form_key, school_code, first_name, last_name, sport, injured_area, 
                   injured_side, treatment_type, athlete_comments, athlete_status, 
                   date_created, date_seen, date_of_birth, key, iv
            FROM athlete_forms 
            WHERE form_key = @formKey", conn);
                cmd.Parameters.AddWithValue("formKey", formKey); // Set the parameter value.
                using var reader = cmd.ExecuteReader();

                // If a form is found, add it to the local collection.
                if (reader.Read())
                {
                    var schoolCode = reader.GetString(1);
                    var firstName = reader.GetString(2);
                    var lastName = reader.GetString(3);
                    var sport = reader.GetString(4);
                    var injuredArea = reader.GetString(5);
                    var injuredSide = reader.GetString(6);
                    var treatmentType = reader.GetString(7);
                    var athleteComments = reader.IsDBNull(8) ? null : reader.GetString(8);
                    var status = reader.IsDBNull(9) ? null : reader.GetString(9);
                    var dateCreated = reader.GetDateTime(10);
                    var dateSeen = reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11);
                    var dateOfBirth = reader.GetDateTime(12);
                    var key = reader.GetString(13);
                    var iv = reader.GetString(14);

                    // Create a new instance of AthleteForm with the retrieved data
                    var form = new AthleteForm(
                        formKey: formKey,
                        schoolCode: schoolCode,
                        firstName: firstName,
                        lastName: lastName,
                        sport: sport,
                        injuredArea: injuredArea,
                        injuredSide: injuredSide,
                        treatmentType: treatmentType,
                        dateCreated: dateCreated,
                        dateSeen: dateSeen,
                        dateOfBirth: dateOfBirth,
                        athleteComments: athleteComments,
                        status: status,
                        key: key,
                        iv: iv
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

                using var cmd = new NpgsqlCommand(@"
                INSERT INTO athlete_forms 
                (school_code, first_name, last_name, sport, injured_area, injured_side, treatment_type,
                 athlete_comments, athlete_status, date_created, date_seen, date_of_birth) 
                VALUES (@schoolCode, @firstName, @lastName, @sport, @injuredArea, @injuredSide, @treatmentType,
                        @athleteComments, @status, @dateCreated, @dateSeen, @dateOfBirth)", conn);

                cmd.Parameters.AddWithValue("schoolCode", form.SchoolCode);
                cmd.Parameters.AddWithValue("firstName", form.FirstName);
                cmd.Parameters.AddWithValue("lastName", form.LastName);
                cmd.Parameters.AddWithValue("sport", form.Sport);
                cmd.Parameters.AddWithValue("injuredArea", form.InjuredArea);
                cmd.Parameters.AddWithValue("injuredSide", form.InjuredSide);
                cmd.Parameters.AddWithValue("treatmentType", form.TreatmentType);
                cmd.Parameters.AddWithValue("athleteComments", (object?)form.AthleteComments ?? DBNull.Value);
                cmd.Parameters.AddWithValue("status", (object?)form.Status ?? DBNull.Value);
                cmd.Parameters.AddWithValue("dateCreated", form.DateCreated);
                cmd.Parameters.AddWithValue("dateSeen", form.DateSeen.HasValue ? form.DateSeen.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("dateOfBirth", form.DateOfBirth);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    forms.Add(form);
                    return "Form added successfully.";
                }
                else
                {
                    return "No rows were affected. The form was not added.";
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return "An error occurred while adding the form.";
            }
        }

        /// <summary>
        /// Deletes a form from the database by its form key
        /// </summary>
        /// <param name="formKey">The key of the form to delete.</param>
        /// <returns>A message indicating whether the form was deleted from the database.</returns>
        public string DeleteForm(long formKey)
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

                using var cmd = new NpgsqlCommand(@"
                UPDATE athlete_forms 
                SET treatment_type = @treatmentType, 
                    athlete_comments = @athleteComments, 
                    athlete_status = @status, 
                    date_seen = @dateSeen,
                    date_of_birth = @dateOfBirth
                WHERE form_key = @formKey", conn);

                cmd.Parameters.AddWithValue("formKey", form.FormKey);
                cmd.Parameters.AddWithValue("treatmentType", form.TreatmentType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("athleteComments", form.AthleteComments ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("status", form.Status ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("dateSeen", form.DateSeen.HasValue ? form.DateSeen.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("dateOfBirth", form.DateOfBirth);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    var index = forms.IndexOf(forms.First(a => a.FormKey == form.FormKey));
                    if (index >= 0)
                    {
                        forms[index] = form;
                    }
                    return "Form updated successfully.";
                }
            }
            catch (Npgsql.PostgresException ex)
            {
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
        /// <param name="key">A key used for encryption.</param>
        /// <param name="iv">An iv used for encryption.</param>
        /// <returns>A message indicating the result of the insertion.</returns>
        public string InsertUser(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode, string key, string iv)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
                INSERT INTO users (first_name, last_name, email, hashed_password, school_name, school_code, encryption_key, encryption_iv)
                VALUES (@firstName, @lastName, @email, @hashedPassword, @schoolName, @schoolCode, @key, @iv)", conn);

                cmd.Parameters.AddWithValue("firstName", firstName);
                cmd.Parameters.AddWithValue("lastName", lastName);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("hashedPassword", hashedPassword);
                cmd.Parameters.AddWithValue("schoolName", schoolName);
                cmd.Parameters.AddWithValue("schoolCode", schoolCode);
                cmd.Parameters.AddWithValue("key", key);
                cmd.Parameters.AddWithValue("iv", iv);

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

        public ObservableCollection<AthleteForm> SearchAthletes(string query)
        {
            var searchResults = new ObservableCollection<AthleteForm>();
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                var cmd = new NpgsqlCommand(@"
            SELECT form_key, school_code, first_name, last_name, sport,
                   injured_area, injured_side, treatment_type, athlete_comments,
                   athlete_status, date_created, date_seen, date_of_birth, key, iv
            FROM athlete_forms 
            WHERE LOWER(first_name) LIKE @query 
            OR LOWER(last_name) LIKE @query 
            OR LOWER(sport) LIKE @query 
            OR LOWER(injured_area) LIKE @query", conn);

                cmd.Parameters.AddWithValue("query", $"%{query.ToLower()}%");

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var form = new AthleteForm(
                        formKey: reader.GetInt64(0),
                        schoolCode: reader.GetString(1),
                        firstName: reader.GetString(2),
                        lastName: reader.GetString(3),
                        sport: reader.GetString(4),
                        injuredArea: reader.GetString(5),
                        injuredSide: reader.GetString(6),
                        treatmentType: reader.GetString(7),
                        athleteComments: reader.IsDBNull(8) ? null : reader.GetString(8),
                        status: reader.IsDBNull(9) ? null : reader.GetString(9),
                        dateCreated: reader.GetDateTime(10),
                        dateSeen: reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11),
                        dateOfBirth: reader.GetDateTime(12),
                        key: reader.GetString(13),
                        iv: reader.GetString(14)
                    );
                    searchResults.Add(form);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return searchResults;
        }

        public ObservableCollection<AthleteForm> SearchAthletesByMultipleCriteria(string query)
        {
            var searchResults = new ObservableCollection<AthleteForm>();

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                var cmd = new NpgsqlCommand(@"
            SELECT form_key, school_code, first_name, last_name, sport,
                   injured_area, injured_side, treatment_type, athlete_comments,
                   athlete_status, date_created, date_seen, date_of_birth
            FROM athlete_forms
            WHERE LOWER(first_name) LIKE @query 
            OR LOWER(last_name) LIKE @query 
            OR LOWER(sport) LIKE @query 
            OR LOWER(athlete_status) LIKE @query
            OR LOWER(treatment_type) LIKE @query
            OR CAST(date_created AS TEXT) LIKE @query
            OR CAST(date_of_birth AS TEXT) LIKE @query", conn);

                cmd.Parameters.AddWithValue("query", $"%{query.ToLower()}%");

                using var reader = cmd.ExecuteReader();

                // Read each result and add it to the ObservableCollection
                while (reader.Read())
                {
                    var form = new AthleteForm(
                        formKey: reader.GetInt64(0),
                        schoolCode: reader.GetString(1),
                        firstName: reader.GetString(2),
                        lastName: reader.GetString(3),
                        sport: reader.GetString(4),
                        injuredArea: reader.GetString(5),
                        injuredSide: reader.GetString(6),
                        treatmentType: reader.GetString(7),
                        athleteComments: reader.IsDBNull(8) ? null : reader.GetString(8),
                        status: reader.IsDBNull(9) ? null : reader.GetString(9),
                        dateCreated: reader.GetDateTime(10),
                        dateSeen: reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11),
                        dateOfBirth: reader.GetDateTime(12),
                        key: reader.GetString(13),
                        iv: reader.GetString(14)
                    );
                    searchResults.Add(form);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return searchResults;
        }

        public ObservableCollection<AthleteForm> SelectAllForms()
        {
            forms.Clear();

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
                SELECT form_key, school_code, first_name, last_name, sport, injured_area,
                       injured_side, treatment_type, athlete_comments, athlete_status, 
                       date_created, date_seen, date_of_birth, key, iv
                FROM athlete_forms", conn);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var form = new AthleteForm(
                        formKey: reader.GetInt64(0),
                        schoolCode: reader.GetString(1),
                        firstName: reader.GetString(2),
                        lastName: reader.GetString(3),
                        sport: reader.GetString(4),
                        injuredArea: reader.GetString(5),
                        injuredSide: reader.GetString(6),
                        treatmentType: reader.GetString(7),
                        athleteComments: reader.IsDBNull(8) ? null : reader.GetString(8),
                        status: reader.IsDBNull(9) ? null : reader.GetString(9),
                        dateCreated: reader.IsDBNull(10) ? DateTime.Now : reader.GetDateTime(10),
                        dateSeen: reader.IsDBNull(11) ? null : reader.GetDateTime(11),
                        dateOfBirth: reader.GetDateTime(12),
                        key: reader.GetString(13),
                        iv: reader.GetString(14)
                    );
                    forms.Add(form);
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return forms;
        }

        /// <summary>
        /// Retrieves all athlete forms with a date_created less than today's date.
        /// </summary>
        /// <returns>An ObservableCollection of AthleteForm objects created before today.</returns>
        public ObservableCollection<AthleteForm> SelectFormsBeforeToday()
        {
            var pastForms = new ObservableCollection<AthleteForm>();

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
            SELECT form_key, school_code, first_name, last_name, sport,
                   injured_area, injured_side, treatment_type, athlete_comments,
                   athlete_status, date_created, date_seen, date_of_birth, key, iv
            FROM athlete_forms 
            WHERE date_created < @today", conn);

                cmd.Parameters.AddWithValue("today", DateTime.Today);

                using var reader = cmd.ExecuteReader();

                // Populate results
                while (reader.Read())
                {
                    var form = new AthleteForm(
                        formKey: reader.GetInt64(0),
                        schoolCode: reader.GetString(1),
                        firstName: reader.GetString(2),
                        lastName: reader.GetString(3),
                        sport: reader.GetString(4),
                        injuredArea: reader.GetString(5),
                        injuredSide: reader.GetString(6),
                        treatmentType: reader.GetString(7),
                        athleteComments: reader.IsDBNull(8) ? null : reader.GetString(8),
                        status: reader.IsDBNull(9) ? null : reader.GetString(9),
                        dateCreated: reader.GetDateTime(10),
                        dateSeen: reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11),
                        dateOfBirth: reader.GetDateTime(12),
                        key: reader.GetString(13),
                        iv: reader.GetString(14)
                    );
                    pastForms.Add(form);
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving past forms: {ex.Message}");
            }

            return pastForms;
        }

        /// <summary>
        /// selects all contacts from the database
        /// </summary>
        /// <returns>ObservableCollection of AthleteContact objects</returns>
        public ObservableCollection<AthleteContact> SelectAllContacts()
        {
            contacts.Clear(); // Clear the local collection

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                using var cmd = new NpgsqlCommand(@"
            SELECT contact_id, form_key, contact_type, phone_number
            FROM athlete_contacts", conn);

                using var reader = cmd.ExecuteReader();

                // Loop through each row in the result and add it to the contacts collection.
                while (reader.Read())
                {
                    var contact = new AthleteContact(
                        contactID: reader.GetInt64(0),              // contact_id as INT8 -> GetInt64
                        formKey: reader.GetInt64(1),                // form_key as INT8 -> GetInt64
                        contactType: reader.GetString(2),           // contact_type as VARCHAR -> GetString
                        phoneNumber: reader.GetString(3)            // phone_number as VARCHAR -> GetString
                    );
                    contacts.Add(contact); // Add to the collection
                }

                Console.WriteLine($"Number of contacts retrieved: {contacts.Count}");
            }
            catch (Npgsql.PostgresException ex)
            {
                // Handle any database-specific errors.
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return contacts;
        }

        /// <summary>
        /// Inserts a new contact into the athlete_contacts table.
        /// </summary>
        /// <param name="formKey">The form key associated with the athlete form.</param>
        /// <param name="contactType">The type of contact (e.g., "Parent", "Coach").</param>
        /// <param name="phoneNumber">The phone number of the contact.</param>
        /// <returns>A message indicating the result of the insertion.</returns>
        public string InsertContact(long formKey, string contactType, string phoneNumber)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
                INSERT INTO public.athlete_contacts (form_key, contact_type, phone_number)
                VALUES (@formKey, @contactType, @phoneNumber)", conn);

                cmd.Parameters.AddWithValue("formKey", formKey);
                cmd.Parameters.AddWithValue("contactType", contactType);
                cmd.Parameters.AddWithValue("phoneNumber", phoneNumber);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0 ? "Contact added successfully." : "Failed to add contact.";
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return "An error occurred while adding the contact.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return "Error adding contact.";
            }
        }

        /// <summary>
        /// Retrieves all contacts associated with a specific form key.
        /// </summary>
        /// <param name="formKey">The form key associated with the athlete form.</param>
        /// <returns>A collection of AthleteContact objects if they exist; otherwise, an empty collection.</returns>
        public ObservableCollection<AthleteContact> SelectContactsByFormKey(long formKey)
        {
            var contacts = new ObservableCollection<AthleteContact>();

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
                SELECT contact_id, form_key, contact_type, phone_number
                FROM public.athlete_contacts
                WHERE form_key = @formKey", conn);

                cmd.Parameters.AddWithValue("formKey", formKey);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var contact = new AthleteContact(
                        reader.GetInt64(0),     // contact_id
                        reader.GetInt64(1),     // form_key
                        reader.GetString(2),    // contact_type
                        reader.GetString(3)     // phone_number
                    );
                    contacts.Add(contact);
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return contacts;
        }

        /// <summary>
        /// Deletes a contact by contact ID.
        /// </summary>
        /// <param name="contactID">The contact ID to delete.</param>
        /// <returns>A message indicating whether the contact was deleted successfully.</returns>
        public string DeleteContact(long contactID)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand("DELETE FROM public.athlete_contacts WHERE contact_id = @contactID", conn);
                cmd.Parameters.AddWithValue("contactID", contactID);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0 ? "Contact deleted successfully." : "Contact not found.";
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return $"Error while deleting contact: {ex.Message}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return $"An unexpected error occurred while deleting the contact: {ex.Message}";
            }
        }

        public long GetLastInsertedFormKey(string schoolCode)
        {
            long lastFormKey = 0;

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
                SELECT form_key 
                FROM public.athlete_forms
                WHERE school_code = @schoolCode
                ORDER BY date_created DESC
                LIMIT 1", conn);

                cmd.Parameters.AddWithValue("schoolCode", schoolCode);

                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    lastFormKey = (long)result;
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return lastFormKey;
        }

        public string UpdateContactStatus(long? formKey, string newStatus)
        {
            if (!formKey.HasValue)
            {
                return "Error: formKey cannot be null.";
            }

            // Proceed with updating, using formKey.Value
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand(
                    "UPDATE athlete_forms SET athlete_status = @newStatus WHERE form_key = @formKey", conn);
                cmd.Parameters.AddWithValue("formKey", formKey.Value);
                cmd.Parameters.AddWithValue("newStatus", newStatus);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0 ? "Contact status updated successfully." : "No rows affected. The specified formKey may not exist.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating contact status: {ex.Message}");
                return "An error occurred while updating the contact status.";
            }
        }

        public ObservableCollection<AthleteForm> SearchAthletesByContact(string query)
        {
            var searchResults = new ObservableCollection<AthleteForm>();
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // Modify the query to include additional search criteria
                var cmd = new NpgsqlCommand(@"
            SELECT form_key, school_code, first_name, last_name, sport,
                   injured_area, injured_side, treatment_type, athlete_comments,
                   athlete_status, date_created, date_seen, date_of_birth, key, iv
            FROM athlete_forms 
            WHERE LOWER(first_name) LIKE @query 
            OR LOWER(last_name) LIKE @query 
            OR LOWER(sport) LIKE @query 
            OR LOWER(injured_area) LIKE @query
            OR LOWER(treatment_type) LIKE @query
            OR EXISTS (
                SELECT 1 FROM athlete_contacts c 
                WHERE c.form_key = athlete_forms.form_key 
                AND (LOWER(c.contact_type) LIKE @query OR c.phone_number LIKE @query)
            )", conn);

                cmd.Parameters.AddWithValue("query", $"%{query.ToLower()}%");

                using var reader = cmd.ExecuteReader();

                // Populate results
                while (reader.Read())
                {
                    var form = new AthleteForm(
                        formKey: reader.GetInt64(0),
                        schoolCode: reader.GetString(1),
                        firstName: reader.GetString(2),
                        lastName: reader.GetString(3),
                        sport: reader.GetString(4),
                        injuredArea: reader.GetString(5),
                        injuredSide: reader.GetString(6),
                        treatmentType: reader.GetString(7),
                        athleteComments: reader.IsDBNull(8) ? null : reader.GetString(8),
                        status: reader.IsDBNull(9) ? null : reader.GetString(9),
                        dateCreated: reader.GetDateTime(10),
                        dateSeen: reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11),
                        dateOfBirth: reader.GetDateTime(12),
                        key: reader.GetString(13),
                        iv: reader.GetString(14)
                    );
                    searchResults.Add(form);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return searchResults;
        }

        /// <summary>
        /// Saves the updated information for a form and associated contacts to the database.
        /// </summary>
        /// <param name="form">The form with updated details.</param>
        /// <param name="updatedContacts">A list of updated contacts associated with the form.</param>
        /// <returns>A message indicating whether the update was successful.</returns>
        public string SaveUpdatedForm(AthleteForm form, List<AthleteContact> updatedContacts)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                using var transaction = conn.BeginTransaction();

                // Update the form's details
                using var formCmd = new NpgsqlCommand
                {
                    Connection = conn,
                    CommandText = @"
                        UPDATE athlete_forms 
                        SET treatment_type = @treatmentType, 
                            athlete_status = @athleteStatus 
                        WHERE form_key = @formKey"
                };
                formCmd.Parameters.AddWithValue("formKey", form.FormKey);
                formCmd.Parameters.AddWithValue("treatmentType", form.TreatmentType ?? (object)DBNull.Value);
                formCmd.Parameters.AddWithValue("athleteStatus", form.Status ?? (object)DBNull.Value);

                int formRowsAffected = formCmd.ExecuteNonQuery();

                // Update each contact associated with the form
                foreach (var contact in updatedContacts)
                {
                    using var contactCmd = new NpgsqlCommand
                    {
                        Connection = conn,
                        CommandText = @"
                            UPDATE athlete_contacts 
                            SET contact_type = @contactType, 
                                phone_number = @phoneNumber 
                            WHERE contact_id = @contactID"
                    };
                    contactCmd.Parameters.AddWithValue("contactID", contact.ContactID);
                    contactCmd.Parameters.AddWithValue("contactType", contact.ContactType ?? (object)DBNull.Value);
                    contactCmd.Parameters.AddWithValue("phoneNumber", contact.PhoneNumber ?? (object)DBNull.Value);

                    contactCmd.ExecuteNonQuery();
                }

                transaction.Commit();

                if (formRowsAffected > 0)
                {
                    // Update the local collection for real-time sync
                    var formIndex = forms.IndexOf(forms.FirstOrDefault(f => f.FormKey == form.FormKey));
                    if (formIndex >= 0)
                    {
                        forms[formIndex] = form; // Update the form in the local ObservableCollection
                    }

                    // Update contacts in the local ObservableCollection
                    foreach (var contact in updatedContacts)
                    {
                        var contactIndex = contacts.IndexOf(contacts.FirstOrDefault(c => c.ContactID == contact.ContactID));
                        if (contactIndex >= 0)
                        {
                            contacts[contactIndex] = contact;
                        }
                    }

                    return "Form and contacts updated successfully.";
                }
                else
                {
                    return "No rows were affected. The form may not exist.";
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return "An error occurred while updating the form.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return "An unexpected error occurred while updating the form.";
            }
        }

        // Method to retrieve injury statistics for a specific school
        public ObservableCollection<InjuryStatistic> GetStatisticsForAllSports(string schoolCode)
        {
            var injuryStatistics = new ObservableCollection<InjuryStatistic>();

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // query to get injury statistics
                using var cmd = new NpgsqlCommand(@"
                    SELECT injured_area, ROUND(COUNT(*) * 100.0 / SUM(COUNT(*)) OVER(), 2) AS percentage
                    FROM athlete_forms
                    WHERE school_code = @schoolCode
                    GROUP BY injured_area", conn);

                // add parameters
                cmd.Parameters.AddWithValue("schoolCode", schoolCode);

                using var reader = cmd.ExecuteReader();

                // read the results and add them to the statistics collection
                while (reader.Read())
                {
                    injuryStatistics.Add(new InjuryStatistic
                    {
                        InjuryType = reader.GetString(0),
                        Percentage = reader.GetFloat(1),
                    });
                }
                // return the generated statistics
                return injuryStatistics;
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return null;
            }
        }

        // Method to retrieve injury statistics for a specific school and sport
        public ObservableCollection<InjuryStatistic> GetStatisticsForSport(string schoolCode, string sport)
        {
            var injuryStatistics = new ObservableCollection<InjuryStatistic>();

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // query to get injury statistics
                using var cmd = new NpgsqlCommand(@"
                    SELECT injured_area, ROUND(COUNT(*) * 100.0 / SUM(COUNT(*)) OVER(), 2) AS percentage
                    FROM athlete_forms
                    WHERE school_code = @schoolCode AND sport = @sport
                    GROUP BY injured_area", conn);

                // add parameters
                cmd.Parameters.AddWithValue("schoolCode", schoolCode);
                cmd.Parameters.AddWithValue("sport", sport);

                using var reader = cmd.ExecuteReader();

                // read the results and add them to the statistics collection
                while (reader.Read())
                {
                    injuryStatistics.Add(new InjuryStatistic
                    {
                        InjuryType = reader.GetString(0),
                        Percentage = reader.GetFloat(1),
                    });
                }
                // return the generated statistics
                return injuryStatistics;
            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                return null;
            }
        }

        public bool DeleteUserAccount(string email)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand("DELETE FROM users WHERE email = @Email", conn);
                cmd.Parameters.AddWithValue("Email", email);

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

                using var cmd = new NpgsqlCommand(@"
                SELECT first_name, last_name, email, school_name, school_code
                FROM public.users
                WHERE email = @Email", conn);

                cmd.Parameters.AddWithValue("Email", email);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
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
            User user = null; 

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
                SELECT first_name, last_name, email, hashed_password, school_name, school_code, encryption_key, encryption_iv
                FROM public.users
                WHERE email = @Email", conn);

                cmd.Parameters.AddWithValue("Email", email);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User(
                        firstName: reader.GetString(0),
                        lastName: reader.GetString(1),
                        email: reader.GetString(2),
                        hashedPassword: reader.GetString(3),
                        schoolName: reader.GetString(4),
                        schoolCode: reader.GetString(5),
                        key: reader.GetString(6),
                        iv: reader.GetString(7)
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

                using var cmd = new NpgsqlCommand(@"
        SELECT COUNT(1)
        FROM public.users
        WHERE email = @Email", conn);

                cmd.Parameters.AddWithValue("Email", email);

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

        public bool UpdateUserProfile(string originalEmail, string firstName, string lastName, string schoolName, string schoolCode, string email)
        {
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                using var cmd = new NpgsqlCommand(@"
                UPDATE public.users
                SET first_name = @FirstName, 
                    last_name = @LastName, 
                    school_name = @SchoolName, 
                    school_code = @SchoolCode, 
                    email = @Email
                WHERE email = @OriginalEmail", conn);

                cmd.Parameters.AddWithValue("FirstName", firstName);
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