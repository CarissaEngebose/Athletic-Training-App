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
                    SELECT form_key, school_code, first_name, last_name, grade, sport,
                        injured_area, injured_side, treatment_type, athlete_comments,
                        trainer_comments, athlete_status, date_created
                    FROM athlete_forms
                    WHERE school_code = @school_code",
                    conn
                );
                cmd.Parameters.AddWithValue("school_code", schoolCode); // Set the parameter value.
                using var reader = cmd.ExecuteReader();

                // Loop through each row in the result and add it to the forms collection.
                while (reader.Read())
                {
                    var formKey = reader.GetInt64(0); // unique identifier for the form
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
                    SELECT form_key, school_code, first_name, last_name, grade, sport,
                        injured_area, injured_side, treatment_type, athlete_comments,
                        trainer_comments, athlete_status, date_created
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
            SELECT form_key, school_code, first_name, last_name, grade, sport,
                injured_area, injured_side, treatment_type, athlete_comments,
                trainer_comments, athlete_status, date_created, date_seen
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
                using var cmd = new NpgsqlCommand("SELECT form_key, school_code, first_name, last_name, grade, sport, injured_area, injured_side, treatment_type, athlete_comments, trainer_comments, athlete_status, date_created FROM athlete_forms WHERE form_key = @formKey", conn);
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

        public ObservableCollection<AthleteForm> SearchAthletes(string query)
        {
            var searchResults = new ObservableCollection<AthleteForm>();
            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                var cmd = new NpgsqlCommand(@"
            SELECT form_key, school_code, first_name, last_name, grade, sport,
                injured_area, injured_side, treatment_type, athlete_comments,
                trainer_comments, athlete_status, date_created
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
                        grade: reader.GetInt16(4),
                        sport: reader.GetString(5),
                        injuredArea: reader.GetString(6),
                        injuredSide: reader.GetString(7),
                        treatmentType: reader.GetString(8),
                        athleteComments: reader.IsDBNull(9) ? null : reader.GetString(9),
                        trainerComments: reader.IsDBNull(10) ? null : reader.GetString(10),
                        status: reader.IsDBNull(11) ? null : reader.GetString(11),
                        date: reader.GetDateTime(12)
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
            forms.Clear(); // Clear the local collection

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                using var cmd = new NpgsqlCommand(@"
            SELECT form_key, school_code, first_name, last_name, grade, sport,
                injured_area, injured_side, treatment_type, athlete_comments,
                trainer_comments, athlete_status, date_created
            FROM athlete_forms", conn);

                using var reader = cmd.ExecuteReader();

                // Loop through each row in the result and add it to the forms collection.
                while (reader.Read())
                {
                    var form = new AthleteForm(
                        formKey: reader.GetInt64(0),              // form_key as INT8 -> GetInt64
                        schoolCode: reader.GetString(1),          // school_code as VARCHAR -> GetString
                        firstName: reader.GetString(2),           // first_name as VARCHAR -> GetString
                        lastName: reader.GetString(3),            // last_name as VARCHAR -> GetString
                        grade: reader.GetInt16(4),                // grade as INT2 -> GetInt16
                        sport: reader.GetString(5),               // sport as VARCHAR -> GetString
                        injuredArea: reader.GetString(6),         // injured_area as VARCHAR -> GetString
                        injuredSide: reader.GetString(7),         // injured_side as VARCHAR -> GetString
                        treatmentType: reader.GetString(8),       // treatment_type as VARCHAR -> GetString
                        athleteComments: reader.IsDBNull(9) ? null : reader.GetString(9), // Nullable STRING
                        trainerComments: reader.IsDBNull(10) ? null : reader.GetString(10), // Nullable STRING
                        status: reader.IsDBNull(11) ? null : reader.GetString(11), // Nullable STRING (athlete_status)
                        date: reader.GetDateTime(12)              // Assuming date_created is a TIMESTAMP or DATE -> GetDateTime
                    );
                    forms.Add(form); // Add to the collection
                }

                Console.WriteLine($"Number of athletes retrieved: {forms.Count}");
            }
            catch (Npgsql.PostgresException ex)
            {
                // Handle any database-specific errors.
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return forms;
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