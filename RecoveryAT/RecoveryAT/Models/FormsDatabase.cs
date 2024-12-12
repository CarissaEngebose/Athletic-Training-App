/**
    Date: 12/05/24
    Description: Database implementation for the RecoveryAT app including functionality to insert, update, delete and select forms and users
    from where they are stored in the database.
    Bugs: None that I know of.
    Reflection: The database took a quite a bit of time because we started going screen by screen and implementing the database that way
    which can take a bit of time for each sprint but overall we think it went well.
**/

using System.Collections.ObjectModel;
using Npgsql;

namespace RecoveryAT
{
    /// <summary>
    /// Provides methods to interact with the form data stored in a PostgreSQL database.
    /// </summary>
    public class FormsDatabase : IFormsDatabase
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
        public FormsDatabase()
        {
            connString = GetConnectionString();
            Console.WriteLine($"Connected to database: {connString}");
        }

        /// <summary>
        /// Gets a list of forms for a school code.
        /// </summary>
        /// <param name="schoolCode">The school code of the forms to retrieve.</param>
        /// <returns>The list of forms if they exists; otherwise, null.</returns>
        public ObservableCollection<AthleteForm> SelectFormsBySchoolCode(string schoolCode)
        {
            forms.Clear(); // clear any forms that may be in the list

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                // get the information from the database for all forms using the school code
                using var cmd = new NpgsqlCommand(@"
                SELECT form_key, school_code, first_name, last_name, sport, injured_area,
                       injured_side, treatment_type, athlete_comments, athlete_status, 
                       date_created, date_seen, date_of_birth
                FROM athlete_forms
                WHERE school_code = @school_code", conn);
                _ = cmd.Parameters.AddWithValue("school_code", schoolCode);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var form = new AthleteForm( // creates a new athlete form with the information from the database
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
                        dateOfBirth: reader.GetDateTime(12)
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
                // get the information from the database for all forms using the school code and the date created
                using var cmd = new NpgsqlCommand(@"
            SELECT form_key, school_code, first_name, last_name, sport,
                   injured_area, injured_side, treatment_type, athlete_comments,
                   athlete_status, date_created, date_seen, date_of_birth
            FROM athlete_forms
            WHERE school_code = @school_code
            AND date_created = @dateCreated",
                    conn
                );
                _ = cmd.Parameters.AddWithValue("school_code", schoolCode); // set the parameter value
                _ = cmd.Parameters.AddWithValue("dateCreated", dateCreated);
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
                        status: status
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
        /// <param name="dateCreated">The date when the forms were seen.</param>
        /// <returns>An ObservableCollection of forms that were seen on the specified date.</returns>
        public ObservableCollection<AthleteForm> SelectFormsByDateCreated(string schoolCode, DateTime dateCreated)
        {
            var formsByDateCreated = new ObservableCollection<AthleteForm>(); // Create a new collection to hold the results

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                // gets the form information using the school code and the date created
                using var cmd = new NpgsqlCommand(@"
            SELECT form_key, school_code, first_name, last_name, sport,
                   injured_area, injured_side, treatment_type, athlete_comments,
                   athlete_status, date_created, date_seen, date_of_birth
            FROM athlete_forms
            WHERE school_code = @school_code
            AND date_created = @dateCreated", conn);

                _ = cmd.Parameters.AddWithValue("school_code", schoolCode); // Set the school code parameter
                _ = cmd.Parameters.AddWithValue("dateCreated", dateCreated);      // Set the date_created parameter

                using var reader = cmd.ExecuteReader();

                // Loop through each row in the result and add it to the formsByDateCreated collection
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
                    var dateSeen = reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11);
                    var dateOfBirth = reader.GetDateTime(12);

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
                        status: status
                    );
                    formsByDateCreated.Add(form); // Add to the collection
                }
            }
            catch (Npgsql.PostgresException ex)
            {
                // Handle any database-specific errors.
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return formsByDateCreated; // Return the collection of forms filtered by date_created
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
                // get the form information using the form key
                using var cmd = new NpgsqlCommand(@"
            SELECT form_key, school_code, first_name, last_name, sport, injured_area, 
                   injured_side, treatment_type, athlete_comments, athlete_status, 
                   date_created, date_seen, date_of_birth
            FROM athlete_forms 
            WHERE form_key = @formKey", conn);
                _ = cmd.Parameters.AddWithValue("formKey", formKey); // Set the parameter value.
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
                        status: status
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
                // inserts the athlete form into the database using the parameters
                using var cmd = new NpgsqlCommand(@"
                INSERT INTO athlete_forms 
                (school_code, first_name, last_name, sport, injured_area, injured_side, treatment_type,
                 athlete_comments, athlete_status, date_created, date_seen, date_of_birth) 
                VALUES (@schoolCode, @firstName, @lastName, @sport, @injuredArea, @injuredSide, @treatmentType,
                        @athleteComments, @status, @dateCreated, @dateSeen, @dateOfBirth)", conn);

                _ = cmd.Parameters.AddWithValue("schoolCode", form.SchoolCode); // set the parameter values
                _ = cmd.Parameters.AddWithValue("firstName", form.FirstName);
                _ = cmd.Parameters.AddWithValue("lastName", form.LastName);
                _ = cmd.Parameters.AddWithValue("sport", form.Sport);
                _ = cmd.Parameters.AddWithValue("injuredArea", form.InjuredArea);
                _ = cmd.Parameters.AddWithValue("injuredSide", form.InjuredSide);
                _ = cmd.Parameters.AddWithValue("treatmentType", form.TreatmentType);
                _ = cmd.Parameters.AddWithValue("athleteComments", (object?)form.AthleteComments ?? DBNull.Value);
                _ = cmd.Parameters.AddWithValue("status", (object?)form.Status ?? DBNull.Value);
                _ = cmd.Parameters.AddWithValue("dateCreated", form.DateCreated);
                _ = cmd.Parameters.AddWithValue("dateSeen", form.DateSeen.HasValue ? form.DateSeen.Value : DBNull.Value);
                _ = cmd.Parameters.AddWithValue("dateOfBirth", form.DateOfBirth);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // if there are rows effected, the form was added successfully
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
                // delete the form from the database using the form key
                using var cmd = new NpgsqlCommand("DELETE FROM athlete_forms WHERE form_key = @formKey", conn);
                _ = cmd.Parameters.AddWithValue("formKey", formKey);

                int numRowsAffected = cmd.ExecuteNonQuery();

                // If the deletion is successful, remove the form from the local collection.
                if (numRowsAffected > 0)
                {
                    var formToDelete = forms.FirstOrDefault(a => a.FormKey == formKey);
                    if (formToDelete != null)
                    {
                        _ = forms.Remove(formToDelete); // Update the local collection.
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

                // update the table in the database for the form 
                using var cmd = new NpgsqlCommand(@"
                UPDATE athlete_forms 
                SET treatment_type = @treatmentType, 
                    athlete_comments = @athleteComments, 
                    athlete_status = @status, 
                    date_seen = @dateSeen,
                    date_of_birth = @dateOfBirth
                WHERE form_key = @formKey", conn);

                _ = cmd.Parameters.AddWithValue("formKey", form.FormKey); // set the parameters to update a specific form
                _ = cmd.Parameters.AddWithValue("treatmentType", form.TreatmentType ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("athleteComments", form.AthleteComments ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("status", form.Status ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("dateSeen", form.DateSeen.HasValue ? form.DateSeen.Value : DBNull.Value);
                _ = cmd.Parameters.AddWithValue("dateOfBirth", form.DateOfBirth);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // if the form was updated return that the form updated successfully
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
        /// Retrieves all athlete forms from the database.
        /// </summary>
        /// <returns>An observable collection of all athlete forms.</returns>
        public ObservableCollection<AthleteForm> SelectAllForms()
        {
            forms.Clear();

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                // selects all forms from the database
                using var cmd = new NpgsqlCommand(@"
                SELECT form_key, school_code, first_name, last_name, sport, injured_area,
                       injured_side, treatment_type, athlete_comments, athlete_status, 
                       date_created, date_seen, date_of_birth
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
                        dateOfBirth: reader.GetDateTime(12)
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
                   athlete_status, date_created, date_seen, date_of_birth
            FROM athlete_forms 
            WHERE date_created < @today", conn);

                _ = cmd.Parameters.AddWithValue("today", DateTime.Today);

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
                        dateOfBirth: reader.GetDateTime(12)
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
        /// Retrieves the last inserted form key for a given school code.
        /// </summary>
        /// <param name="schoolCode">The school name to get the last inserted form from the database.</param>
        /// <returns>The form key that was just inserted for a school code.</returns>
        public long GetLastInsertedFormKey(string schoolCode)
        {
            long lastFormKey = 0;

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // selects the form key that was most recently created
                using var cmd = new NpgsqlCommand(@"
                SELECT form_key 
                FROM public.athlete_forms
                WHERE school_code = @schoolCode
                ORDER BY date_created DESC, created_at DESC
                LIMIT 1", conn);

                _ = cmd.Parameters.AddWithValue("schoolCode", schoolCode); // adds the school code as the parameter to search by

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
                _ = formCmd.Parameters.AddWithValue("formKey", form.FormKey);
                _ = formCmd.Parameters.AddWithValue("treatmentType", form.TreatmentType ?? (object)DBNull.Value);
                _ = formCmd.Parameters.AddWithValue("athleteStatus", form.Status ?? (object)DBNull.Value);

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
                    _ = contactCmd.Parameters.AddWithValue("contactID", contact.ContactID);
                    _ = contactCmd.Parameters.AddWithValue("contactType", contact.ContactType ?? (object)DBNull.Value);
                    _ = contactCmd.Parameters.AddWithValue("phoneNumber", contact.PhoneNumber ?? (object)DBNull.Value);

                    _ = contactCmd.ExecuteNonQuery();
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