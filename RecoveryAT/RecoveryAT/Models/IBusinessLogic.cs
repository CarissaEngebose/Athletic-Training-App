/**
    Name: Carissa Engebose
    Date: 10/27/24
    Description: Business logic interface for managing forms, users, and related data.
    Bugs: None that I know of.
    Reflection: This interface provides a clear structure for all the required operations in the RecoveryAT app.
**/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RecoveryAT
{
    /// <summary>
    /// Defines the business logic for managing athlete forms and related data.
    /// </summary>
    public interface IBusinessLogic
    {
        /// <summary>
        /// Checks if the provided school code components are valid in the database.
        /// </summary>
        /// <param name="codePart1">First part of the school code.</param>
        /// <param name="codePart2">Second part of the school code.</param>
        /// <param name="codePart3">Third part of the school code.</param>
        /// <param name="codePart4">Fourth part of the school code.</param>
        /// <param name="codePart5">Fifth part of the school code.</param>
        /// <returns>A message indicating the result of the validation.</returns>
        string IsValidSchoolCode(string codePart1, string codePart2, string codePart3, string codePart4, string codePart5);

        /// <summary>
        /// Checks if a school code already exists in the database.
        /// </summary>
        /// <param name="schoolCode">The school code to check.</param>
        /// <returns>True if the school code exists; otherwise, false.</returns>
        bool SchoolCodeExists(string schoolCode);

        /// <summary>
        /// Gets a list of all forms for a given school code.
        /// </summary>
        /// <param name="schoolCode">The school code to search for forms.</param>
        /// <returns>A list of forms.</returns>
        ObservableCollection<AthleteForm>? GetForms(string schoolCode);

        /// <summary>
        /// Retrieves all athlete forms from the database.
        /// </summary>
        /// <returns>An observable collection of all athlete forms.</returns>
        ObservableCollection<AthleteForm> GetAllForms();

        /// <summary>
        /// Retrieves all forms created on a specific date.
        /// </summary>
        /// <param name="schoolCode">The school code to search for forms.</param>
        /// <param name="date">The date to search for forms.</param>
        /// <returns>A list of forms.</returns>
        ObservableCollection<AthleteForm>? GetFormsByDate(string schoolCode, DateTime date);

        /// <summary>
        /// Adds a new form to the system.
        /// </summary>
        /// <param name="schoolCode">The identifier of the athlete's school.</param>
        /// <param name="firstName">The athlete's first name.</param>
        /// <param name="lastName">The athlete's last name.</param>
        /// <param name="sport">The sport the athlete participates in.</param>
        /// <param name="injuredArea">The location of the athlete's injury.</param>
        /// <param name="injuredSide">The side of the athlete's injury.</param>
        /// <param name="treatmentType">The type of treatment the athlete is receiving.</param>
        /// <param name="dateOfBirth">The athlete's date of birth.</param>
        /// <param name="athleteComments">Optional comments from the athlete.</param>
        /// <param name="status">The athlete's current status.</param>
        /// <param name="dateCreated">The date the form was created.</param>
        /// <returns>A message indicating if the form was successfully added.</returns>
        string AddForm(string schoolCode, string firstName, string lastName, string sport,
                       string injuredArea, string injuredSide, string treatmentType, DateTime dateOfBirth,
                       string? athleteComments, string? status, DateTime dateCreated);

        /// <summary>
        /// Deletes a form by its unique key.
        /// </summary>
        /// <param name="formKey">The unique identifier of the form to delete.</param>
        /// <returns>A message indicating if the form was successfully deleted.</returns>
        string DeleteForm(long formKey);

        /// <summary>
        /// Updates an existing form with new details.
        /// </summary>
        /// <param name="formKey">The unique identifier of the form to update.</param>
        /// <param name="schoolCode">The school code associated with the form.</param>
        /// <param name="firstName">The athlete's first name.</param>
        /// <param name="lastName">The athlete's last name.</param>
        /// <param name="sport">The sport the athlete participates in.</param>
        /// <param name="injuredArea">The location of the athlete's injury.</param>
        /// <param name="injuredSide">The side of the athlete's injury.</param>
        /// <param name="treatmentType">The type of treatment the athlete is receiving.</param>
        /// <param name="dateOfBirth">The athlete's date of birth.</param>
        /// <param name="athleteComments">Optional comments from the athlete.</param>
        /// <param name="status">The athlete's current status.</param>
        /// <param name="dateCreated">The date the form was created.</param>
        /// <returns>A message indicating if the form was successfully updated.</returns>
        string EditForm(long formKey, string schoolCode, string firstName, string lastName, string sport,
                        string injuredArea, string injuredSide, string treatmentType, DateTime dateOfBirth,
                        string? athleteComments, string status, DateTime dateCreated);

        /// <summary>
        /// Inserts a new user into the system.
        /// </summary>
        string InsertUser(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode);

        /// <summary>
        /// Retrieves the last inserted form key for a given school code.
        /// </summary>
        long GetLastInsertedFormKey(string schoolCode);

        /// <summary>
        /// Validates user credentials (placeholder).
        /// </summary>
        bool ValidateCredentials(string email, string password);

        /// <summary>
        /// Retrieves forms seen on a specific date.
        /// </summary>
        ObservableCollection<AthleteForm> GetFormsByDateSeen(string schoolCode, DateTime dateSeen);

        /// <summary>
        /// Searches for athletes based on a query string.
        /// </summary>
        ObservableCollection<AthleteForm> SearchAthletes(string query);

        /// <summary>
        /// Searches athletes by contact information.
        /// </summary>
        ObservableCollection<AthleteForm> SearchAthletesByContact(string query);

        /// <summary>
        /// Retrieves forms created before today.
        /// </summary>
        ObservableCollection<AthleteForm> GetFormsBeforeToday();

        /// <summary>
        /// Saves an updated form along with associated contacts.
        /// </summary>
        string SaveUpdatedForm(AthleteForm form, List<AthleteContact> updatedContacts);

        /// <summary>
        /// Retrieves contacts associated with a specific form key.
        /// </summary>
        ObservableCollection<AthleteContact> GetContactsByFormKey(long formKey);

        /// <summary>
        /// Searches athletes based on multiple criteria.
        /// </summary>
        ObservableCollection<AthleteForm> SearchAthletesByMultipleCriteria(string query);

        /// <summary>
        /// Retrieves injury statistics for all sports within a school.
        /// </summary>
        ObservableCollection<InjuryStatistic>? GetStatisticsForAllSports(string schoolCode);

        /// <summary>
        /// Retrieves injury statistics for a specific sport within a school.
        /// </summary>
        ObservableCollection<InjuryStatistic>? GetStatisticsForSport(string schoolCode, string sport);

        /// <summary>
        /// Retrieves all contacts associated with a specific form key.
        /// </summary>
        /// <param name="formKey">The form key associated with the athlete form.</param>
        /// <returns>A collection of AthleteContact objects if they exist; otherwise, an empty collection.</returns>
        ObservableCollection<AthleteContact> SelectContactsByFormKey(long formKey);

        /// <summary>
        /// Inserts a new contact associated with a specific form.
        /// </summary>
        /// <param name="formKey">The form key associated with the contact.</param>
        /// <param name="contactType">The type of contact (e.g., "Guardian").</param>
        /// <param name="phoneNumber">The phone number of the contact.</param>
        /// <returns>A message indicating the result of the operation.</returns>
        string InsertContact(long formKey, string contactType, string phoneNumber);

        /// <summary>
        /// Updates the contact status of an athlete form.
        /// </summary>
        /// <param name="formKey">The unique identifier for the athlete form.</param>
        /// <param name="newStatus">The new contact status to apply.</param>
        /// <returns>A string indicating success or failure of the update.</returns>
        string UpdateContactStatus(long formKey, string newStatus);

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
