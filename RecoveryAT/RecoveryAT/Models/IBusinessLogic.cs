/**
    Date: 12/05/24
    Description: Created the business logic implementation to be used when creating/deleting/editing a form and editing/creating/deleting a user. 
                 It also handles communicating with the database and returning values to be displayed in the app.
    Bugs: None that we know of.
    Reflection: This class took a while to fully develop because we only starting implementing a few things at a time for each sprint
    and built it up from there. 
**/

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
        /// <param name="firstName">The first name of the athlete.</param>
        /// <param name="lastName">The last name of the athlete.</param>
        /// <param name="email">The athlete's email.</param>
        /// <param name="hashedPassword">The hashed password of the user's password.</param>
        /// <param name="schoolName">The school name associated with the user.</param>
        /// <param name="schoolCode">The school code associated the the user.</param>
        /// <param name="key">The unique key to encrypt and decrypt the user's school name.</param>
        /// <param name="iv">The unique iv to encrypt and decrypt the user's school name.</param>
        /// <returns>A message indicating if the form was successfully inserted.</returns>
        string InsertUser(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode, string key, string iv, string hashedSecurityQuestions);

        /// <summary>
        /// Retrieves the last inserted form key for a given school code.
        /// </summary>
        /// <param name="schoolCode">The school name to get the last inserted form from the database.</param>
        /// <returns>The form key that was just inserted for a school code.</returns>
        long GetLastInsertedFormKey(string schoolCode);

        /// <summary>
        /// Validates user credentials
        /// </summary>
        /// <param name="email">The email associated with the user.</param>
        /// <param name="password">The password the user just entered.</param>
        /// <returns>True if the credentials are valid.</returns>
        bool ValidateCredentials(string email, string password);

        /// <summary>
        /// Retrieves forms created on a specific date.
        /// </summary>
        /// <param name="schoolCode">The school code of the forms.</param>
        /// <param name="dateCreated">The date the form was created.</param>
        /// <returns>A list of AthleteForms for a certain date.</returns>
        ObservableCollection<AthleteForm> GetFormsByDateCreated(string schoolCode, DateTime dateCreated);

        /// <summary>
        /// Searches for athletes based on a query string.
        /// </summary>
        /// <param name="query">The user's entered search criteria.</param>
        /// <returns>A list of AthleteForms that have the query in it anywhere.</returns>
        ObservableCollection<AthleteForm> SearchAthletes(string query);

        /// <summary>
        /// Searches athletes by contact information.
        /// </summary>
        /// <param name="query">The query to search for the athlete's contact.</param>
        /// <returns>A list of AthleteForms for the specified criteria.</returns>
        ObservableCollection<AthleteForm> SearchAthletesByContact(string query);

        /// <summary>
        /// Retrieves forms created before today.
        /// </summary>
        /// <returns>A list of AthleteForms for a date less than today.</returns>
        ObservableCollection<AthleteForm> GetFormsBeforeToday();

        /// <summary>
        /// Saves an updated form along with associated contacts.
        /// </summary>
        /// <param name="form">The athlete form that was updated.</param>
        /// <param name="updatedContacts">The contact for the athlete that was updated.</param>
        /// <returns>A string stating if the updated form was saved.</returns>
        string SaveUpdatedForm(AthleteForm form, List<AthleteContact> updatedContacts);

        /// <summary>
        /// Retrieves contacts associated with a specific form key.
        /// </summary>
        /// <param name="formKey">The form key to get the contacts by.</param>
        /// <returns>A list of AthleteContacts that correspond to the athlete's form.</returns>
        ObservableCollection<AthleteContact> GetContactsByFormKey(long formKey);

        /// <summary>
        /// Searches athletes based on multiple criteria.
        /// </summary>
        /// <param name="query">The search criteria for the athletes.</param>
        /// <returns>A list of athletes that match the criteria.</returns>
        ObservableCollection<AthleteForm> SearchAthletesByMultipleCriteria(string query);

        /// <summary>
        /// Retrieves injury statistics for all sports within a school.
        /// </summary>
        /// <param name="schoolCode">The school code to search for the forms.</param>
        /// <returns>A list of statistics for all sports for a school.</returns>
        ObservableCollection<InjuryStatistic>? GetStatisticsForAllSports(string schoolCode);

        /// <summary>
        /// Retrieves injury statistics for a specific sport within a school.
        /// </summary>
        /// <param name="schoolCode">The school code to search for the forms.</param>
        /// <param name="sport">The sport to find the statistics for.</param>
        /// <returns>A list of statistics for a certain sport for a school.</returns>
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
        /// Retrieves user information based on their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>A User that stores the information from the database.</returns>
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

        /// <summary>
        /// Selects the all the forms by a school code.
        /// </summary>
        /// <param name="schoolCode">The school to search for the forms.</param>
        /// <returns>A list of all the forms for a specific school.</returns>
        ObservableCollection<AthleteForm> SelectFormsBySchoolCode(string schoolCode);

        /// <summary>
        /// Updates the password for a user.
        /// </summary>
        /// <param name="email">The email of the user whose password is being updated.</param>
        /// <param name="hashedPassword">The new hashed password.</param>
        /// <returns>True if the password was successfully updated; otherwise, false.</returns>
        bool UpdateUserPassword(string email, string hashedPassword);
    }
}