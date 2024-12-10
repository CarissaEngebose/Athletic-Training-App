/**
    Date: 12/05/24
    Description: Database implementation for the RecoveryAT app including functionality to insert, update, delete and select forms and users
    from where they are stored in the database.
    Bugs: None that we know of.
    Reflection: This database interface took a little bit of time over the course of all the sprints because we started implementing
    the database screen by screen so it developed over time. 
**/

using System.Collections.ObjectModel;

namespace RecoveryAT
{
    public interface IDatabase
    {
        /// <summary>
        /// Checks if the provided school code is valid in the database.
        /// </summary>
        /// <param name="schoolCode">The school code to validate.</param>
        /// <returns>True if the school code exists, otherwise false.</returns>
        bool IsValidSchoolCode(string schoolCode);

        /// <summary>
        /// Retrieves injury statistics for all sports at a specific school.
        /// </summary>
        /// <param name="schoolCode">The school code to search.</param>
        /// <returns>A collection of injury statistics for the school.</returns>
        ObservableCollection<InjuryStatistic> GetStatisticsForAllSports(string schoolCode);

        /// <summary>
        /// Retrieves injury statistics for a specific sport at a specific school.
        /// </summary>
        /// <param name="schoolCode">The school code to search.</param>
        /// <param name="sport">The sport to filter by.</param>
        /// <returns>A collection of injury statistics for the specified sport.</returns>
        ObservableCollection<InjuryStatistic> GetStatisticsForSport(string schoolCode, string sport);
    }
}
