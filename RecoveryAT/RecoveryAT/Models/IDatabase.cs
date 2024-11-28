/**
    Name: Carissa Engebose
    Date: 10/27/24
    Description: Database implementation for the RecoveryAT app including functionality to insert, update, delete and select forms from where they are
    stored in the database.
    Bugs: None that I know of.
    Reflection: This database interface didn't take too much time once I figured out what functions I wanted to start with.
**/

using System;
using System.Collections.Generic;
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
        /// Gets a list of all forms for a certain school code from the database.
        /// </summary>
        /// <param name="schoolCode">The school code to find all the forms.</param>
        /// <returns>A list of all forms for a school code, or null if none.</returns>
        ObservableCollection<AthleteForm>? SelectFormsBySchoolCode(string schoolCode);

        /// <summary>
        /// Gets a list of all forms created on a certain date from the database.
        /// </summary>
        /// <param name="schoolCode">The school code to find all the forms created.</param>
        /// <param name="dateCreated">The date to find all the forms created.</param>
        /// <returns>A list of all forms on a date, or null if none.</returns>
        ObservableCollection<AthleteForm>? SelectFormsByDate(string schoolCode, DateTime dateCreated);

        /// <summary>
        /// Gets a specific form by its form key.
        /// </summary>
        /// <param name="formKey">The form key of the form to find.</param>
        /// <returns>The form object if it exists; otherwise, null.</returns>
        ObservableCollection<AthleteForm> SelectForm(long formKey);

        /// <summary>
        /// Retrieves all athlete forms from the database.
        /// </summary>
        /// <returns>An observable collection of all athlete forms.</returns>
        ObservableCollection<AthleteForm> SelectAllForms();

        /// <summary>
        /// Inserts a new form into the database.
        /// </summary>
        /// <param name="form">The form to insert.</param>
        /// <returns>A message indicating if the form was inserted successfully.</returns>
        string InsertForm(AthleteForm form);

        /// <summary>
        /// Deletes a form from the database by its form key.
        /// </summary>
        /// <param name="formKey">The form key of the form to delete.</param>
        /// <returns>A message indicating if the form was deleted successfully.</returns>
        string DeleteForm(long formKey);

        /// <summary>
        /// Updates an existing form in the database.
        /// </summary>
        /// <param name="form">The form with updated information.</param>
        /// <returns>A message indicating if the form was updated successfully.</returns>
        string UpdateForm(AthleteForm form);

        /// <summary>
        /// Inserts a new user into the database.
        /// </summary>
        /// <param name="firstName">User's first name.</param>
        /// <param name="lastName">User's last name.</param>
        /// <param name="email">User's email address.</param>
        /// <param name="hashedPassword">User's hashed password.</param>
        /// <param name="schoolName">User's school name.</param>
        /// <param name="schoolCode">User's school code.</param>
        /// <param name="key">A key used for encryption.</param>
        /// <param name="iv">An iv used for encryption.</param>
        /// <returns>A message indicating the result of the insertion.</returns>
        string InsertUser(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode, string key, string iv);

        /// <summary>
        /// Searches athletes based on a query string.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns>A collection of athlete forms matching the query.</returns>
        ObservableCollection<AthleteForm> SearchAthletes(string query);

        /// <summary>
        /// Retrieves all contacts associated with a specific form key.
        /// </summary>
        /// <param name="formKey">The form key associated with the athlete form.</param>
        /// <returns>A collection of AthleteContact objects if they exist; otherwise, an empty collection.</returns>
        ObservableCollection<AthleteContact> SelectContactsByFormKey(long formKey);

        /// <summary>
        /// Retrieves the most recently inserted form key for a specific school code.
        /// </summary>
        /// <param name="schoolCode">The school code to search for the last inserted form.</param>
        /// <returns>The form key of the most recently inserted form.</returns>
        long GetLastInsertedFormKey(string schoolCode);

        /// <summary>
        /// Inserts a new contact into the database.
        /// </summary>
        /// <param name="formKey">The form key associated with the contact.</param>
        /// <param name="contactType">The type of contact (e.g., "Parent", "Coach").</param>
        /// <param name="phoneNumber">The phone number of the contact.</param>
        /// <returns>A message indicating if the contact was inserted successfully.</returns>
        string InsertContact(long formKey, string contactType, string phoneNumber);

        /// <summary>
        /// Updates the contact status for a specific form.
        /// </summary>
        /// <param name="formKey">The form key associated with the contact.</param>
        /// <param name="newStatus">The new status to set.</param>
        /// <returns>A message indicating if the status was updated successfully.</returns>
        string UpdateContactStatus(long? formKey, string newStatus);

        /// <summary>
        /// Retrieves forms by date last seen.
        /// </summary>
        /// <param name="schoolCode">The school code to filter by.</param>
        /// <param name="dateSeen">The date to search for.</param>
        /// <returns>A collection of athlete forms last seen on the specified date.</returns>
        ObservableCollection<AthleteForm> SelectFormsByDateSeen(string schoolCode, DateTime dateSeen);

        /// <summary>
        /// Searches athletes by name, contact type, phone number, or other attributes.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns>A list of athlete forms matching the search criteria.</returns>
        ObservableCollection<AthleteForm> SearchAthletesByContact(string query);

        /// <summary>
        /// Saves the updated form and associated contacts to the database.
        /// </summary>
        /// <param name="form">The form with updated details.</param>
        /// <param name="updatedContacts">A list of updated contacts associated with the form.</param>
        /// <returns>A message indicating whether the update was successful.</returns>
        string SaveUpdatedForm(AthleteForm form, List<AthleteContact> updatedContacts);

        /// <summary>
        /// Searches athletes using multiple criteria.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns>A collection of athlete forms matching the criteria.</returns>
        ObservableCollection<AthleteForm> SearchAthletesByMultipleCriteria(string query);

        /// <summary>
        /// Retrieves all forms created before today.
        /// </summary>
        /// <returns>A collection of athlete forms created before today.</returns>
        ObservableCollection<AthleteForm> SelectFormsBeforeToday();

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

        /// <summary>
        /// Deletes a user account based on their email.
        /// </summary>
        /// <param name="email">The email of the user to delete.</param>
        /// <returns>True if the user account was successfully deleted, otherwise false.</returns>
        bool DeleteUserAccount(string email);

        /// <summary>
        /// Retrieves user information based on their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>A dictionary containing user information, or null if not found.</returns>
        Dictionary<string, string> GetUserByEmail(string email);

        /// <summary>
        /// Fetches user information based on their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>A User containing user information, or null if not found.</returns>
        User GetUserFromEmail(string email);

        /// <summary>
        /// Checks if a user exists in the database based on their email.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        bool IsEmailRegistered(string email);

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
        bool UpdateUserProfile(string originalEmail, string firstName, string lastName, string schoolName, string schoolCode, string email);
    }
}
