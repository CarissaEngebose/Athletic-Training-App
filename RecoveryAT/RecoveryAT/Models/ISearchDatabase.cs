/**
    Date: 12/06/24
    Description: This interface provides methods to perform search operations on athletes.
    Bugs: None that we know of.
    Reflection: This was easy to implement.
**/

using System.Collections.ObjectModel;

namespace RecoveryAT
{
    /// <summary>
    /// Interface for search-related database operations in the RecoveryAT application.
    /// </summary>
    public interface ISearchDatabase
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
        /// Searches athletes based on a query.
        /// </summary>
        /// <param name="query">The search query for athletes.</param>
        /// <returns>A collection of AthleteForms that match the query.</returns>
        ObservableCollection<AthleteForm> SearchAthletes(string query);

        /// <summary>
        /// Searches athletes based on multiple criteria.
        /// </summary>
        /// <param name="query">The search criteria for the athletes.</param>
        /// <returns>A collection of AthleteForms that match the criteria.</returns>
        ObservableCollection<AthleteForm> SearchAthletesByMultipleCriteria(string query);

        /// <summary>
        /// Searches athletes by contact information.
        /// </summary>
        /// <param name="query">The query to search for athlete contact information.</param>
        /// <returns>A collection of AthleteForms for the specified criteria.</returns>
        ObservableCollection<AthleteForm> SearchAthletesByContact(string query);
    }
}
