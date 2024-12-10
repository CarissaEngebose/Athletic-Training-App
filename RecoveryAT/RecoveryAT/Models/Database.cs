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
                _ = cmd.Parameters.AddWithValue("@schoolCode", schoolCode); // set parameter for school code

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
        /// Retrieves injury statistics for all sports within a school.
        /// </summary>
        /// <param name="schoolCode">The school code to search for the forms.</param>
        /// <returns>A list of statistics for all sports for a school.</returns>
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
                _ = cmd.Parameters.AddWithValue("schoolCode", schoolCode);

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

        /// <summary>
        /// Retrieves injury statistics for a specific sport within a school.
        /// </summary>
        /// <param name="schoolCode">The school code to search for the forms.</param>
        /// <param name="sport">The sport to find the statistics for.</param>
        /// <returns>A list of statistics for a certain sport for a school.</returns>
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

                // add parameters to search by
                _ = cmd.Parameters.AddWithValue("schoolCode", schoolCode);
                _ = cmd.Parameters.AddWithValue("sport", sport);

                using var reader = cmd.ExecuteReader();

                // read the results and add them to the statistics collection
                while (reader.Read())
                {
                    injuryStatistics.Add(new InjuryStatistic // create new injury statistic with results returned from database
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