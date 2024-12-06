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
    public class ContactsDatabase : IContactsDatabase
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
        public ContactsDatabase()
        {
            connString = GetConnectionString();
            Console.WriteLine($"Connected to database: {connString}");
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
                // inserts the new contact into the database
                using var cmd = new NpgsqlCommand(@"
                INSERT INTO public.athlete_contacts (form_key, contact_type, phone_number)
                VALUES (@formKey, @contactType, @phoneNumber)", conn);

                cmd.Parameters.AddWithValue("formKey", formKey); // set parameters to insert contacts in the database
                cmd.Parameters.AddWithValue("contactType", contactType);
                cmd.Parameters.AddWithValue("phoneNumber", phoneNumber);

                int rowsAffected = cmd.ExecuteNonQuery();

                // if rows were affected, return successful message, otherwise return failed message
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
                // delete a contact from the database using the contact id
                using var cmd = new NpgsqlCommand("DELETE FROM public.athlete_contacts WHERE contact_id = @contactID", conn);
                cmd.Parameters.AddWithValue("contactID", contactID);

                int rowsAffected = cmd.ExecuteNonQuery();

                // if rows were affected, return successful message, otherwise return not found message
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
                // selects the contact information using the form key parameter
                using var cmd = new NpgsqlCommand(@"
                SELECT contact_id, form_key, contact_type, phone_number
                FROM public.athlete_contacts
                WHERE form_key = @formKey", conn);

                cmd.Parameters.AddWithValue("formKey", formKey); // set the parameter to select contacts using the form key

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var contact = new AthleteContact( // create a new athlete contact using the information from the database
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
        /// Updates the contact status of an athlete form.
        /// </summary>
        /// <param name="formKey">The unique identifier for the athlete form.</param>
        /// <param name="newStatus">The new contact status to apply.</param>
        /// <returns>A string indicating success or failure of the update.</returns>
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

                // update the table in the database and set the status to the new status
                using var cmd = new NpgsqlCommand(
                    "UPDATE athlete_forms SET athlete_status = @newStatus WHERE form_key = @formKey", conn);
                cmd.Parameters.AddWithValue("formKey", formKey.Value); // sets parameters for status and form key
                cmd.Parameters.AddWithValue("newStatus", newStatus);

                int rowsAffected = cmd.ExecuteNonQuery();

                // if rows were affected, return successful message, otherwise return not exist method
                return rowsAffected > 0 ? "Contact status updated successfully." : "No rows affected. The specified formKey may not exist.";
            }
            catch (Exception ex)
            {
                // catch database errors
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