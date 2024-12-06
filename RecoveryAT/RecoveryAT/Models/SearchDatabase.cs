/**
    Date: 12/06/24
    Description: This interface provides methods to perform search operations.
    Bugs: None that we know of.
    Reflection: This was easy to implement.
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
    public class SearchDatabase : ISearchDatabase
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
        public SearchDatabase()
        {
            connString = GetConnectionString();
            Console.WriteLine($"Connected to database: {connString}");
        }

        /// <summary>
        /// Searches for athletes in the database based on a query string.
        /// The query matches any records where the first name, last name, sport, or injured area
        /// contains the query string, case-insensitively.
        /// </summary>
        /// <param name="query">The search query to filter athlete forms.</param>
        /// <returns>An observable collection of AthleteForm objects that match the search criteria.</returns>
        public ObservableCollection<AthleteForm> SearchAthletes(string query)
        {
            // Initialize an empty collection to hold the search results
            var searchResults = new ObservableCollection<AthleteForm>();

            try
            {
                // Establish a connection to the PostgreSQL database
                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                // Define the SQL query to search for matching athlete forms
                // Matches records where the first name, last name, sport, or injured area contains the query
                var cmd = new NpgsqlCommand(@"
            SELECT form_key, school_code, first_name, last_name, sport,
                   injured_area, injured_side, treatment_type, athlete_comments,
                   athlete_status, date_created, date_seen, date_of_birth
            FROM athlete_forms 
            WHERE LOWER(first_name) LIKE @query 
            OR LOWER(last_name) LIKE @query 
            OR LOWER(sport) LIKE @query 
            OR LOWER(injured_area) LIKE @query", conn);

                // Add the query parameter to the SQL command
                // The parameter is case-insensitively matched using the LOWER function
                cmd.Parameters.AddWithValue("query", $"%{query.ToLower()}%");

                // Execute the SQL command and retrieve the results
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // Create a new AthleteForm object for each row in the result set
                    var form = new AthleteForm(
                        formKey: reader.GetInt64(0),                      // Form key (unique identifier)
                        schoolCode: reader.GetString(1),                 // School code
                        firstName: reader.GetString(2),                  // Athlete's first name
                        lastName: reader.GetString(3),                   // Athlete's last name
                        sport: reader.GetString(4),                      // Sport the athlete participates in
                        injuredArea: reader.GetString(5),                // Area of injury
                        injuredSide: reader.GetString(6),                // Side of injury
                        treatmentType: reader.GetString(7),              // Type of treatment
                        athleteComments: reader.IsDBNull(8) ? null : reader.GetString(8), // Optional comments
                        status: reader.IsDBNull(9) ? null : reader.GetString(9),          // Athlete status
                        dateCreated: reader.GetDateTime(10),             // Form creation date
                        dateSeen: reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11), // Optional date seen
                        dateOfBirth: reader.GetDateTime(12)              // Athlete's date of birth
                    );

                    // Add the AthleteForm object to the results collection
                    searchResults.Add(form);
                }
            }
            catch (Exception ex)
            {
                // Log any database-related errors to the console
                Console.WriteLine($"Database error: {ex.Message}");
            }

            // Return the collection of matching athlete forms
            return searchResults;
        }

        /// <summary>
        /// Searches athletes based on multiple criteria.
        /// </summary>
        /// <param name="query">The search criteria for the athletes.</param>
        /// <returns>A list of athletes that match the criteria.</returns>
        public ObservableCollection<AthleteForm> SearchAthletesByMultipleCriteria(string query)
        {
            var searchResults = new ObservableCollection<AthleteForm>();

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                // searches all columns like the query provdided
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

                cmd.Parameters.AddWithValue("query", $"%{query.ToLower()}%"); // set the query parameter to find the results

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
                        dateOfBirth: reader.GetDateTime(12)
                    );
                    searchResults.Add(form); // adds the results to the list
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return searchResults;
        }

        /// <summary>
        /// Searches athletes by contact information.
        /// </summary>
        /// <param name="query">The query to search for the athlete's contact.</param>
        /// <returns>A list of AthleteForms for the specified criteria.</returns>
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
                   athlete_status, date_created, date_seen, date_of_birth
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
                        dateOfBirth: reader.GetDateTime(12)
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