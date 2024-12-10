/**
    Date: 12/05/24
    Description: Created the business logic implementation to be used when creating/deleting/editing a form and editing/creating/deleting a user. 
                 It also handles communicating with the database and returning values to be displayed in the app.
    Bugs: None that we know of.
    Reflection: This class took a while to fully develop because we only starting implementing a few things at a time for each sprint
    and built it up from there. 
**/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RecoveryAT
{
    /// <summary>
    /// Provides business logic operations for managing athlete forms and related data.
    /// </summary>
    public class BusinessLogic : IBusinessLogic
    {
        private readonly IContactsDatabase _contactsDatabase;
        private readonly IFormsDatabase _formsDatabase;
        private readonly IUsersDatabase _usersDatabase;
        private readonly ISearchDatabase _searchDatabase;
        private readonly IDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessLogic"/> class.
        /// </summary>
        /// <param name="database">The database to interact with.</param>
        public BusinessLogic(IContactsDatabase contactsDatabase,
                             IFormsDatabase formsDatabase,
                             IUsersDatabase usersDatabase,
                             ISearchDatabase searchDatabase,
                             IDatabase database)
        {
            _contactsDatabase = contactsDatabase;
            _formsDatabase = formsDatabase;
            _usersDatabase = usersDatabase;
            _searchDatabase = searchDatabase;
            _database = database;
        }

        /// <summary>
        /// Determines if the character parts given will be a valid school code.
        /// </summary>
        /// <param name="codePart1">The first character of the school code.</param>
        /// <param name="codePart2">The second character of the school code.</param>
        /// <param name="codePart3">The third character of the school code.</param>
        /// <param name="codePart4">The fourth character of the school code.</param>
        /// <param name="codePart5">The fifth character of the school code.</param>
        /// <returns>A string of whether the code is valid or invalid.</returns>
        public string IsValidSchoolCode(string codePart1, string codePart2, string codePart3, string codePart4, string codePart5)
        {
            // if any values are blank, return that there should be 5 characters
            if (string.IsNullOrWhiteSpace(codePart1) ||
                string.IsNullOrWhiteSpace(codePart2) ||
                string.IsNullOrWhiteSpace(codePart3) ||
                string.IsNullOrWhiteSpace(codePart4) ||
                string.IsNullOrWhiteSpace(codePart5))
            {
                return "Code must be 5 characters.";
            }
            string schoolCode = string.Concat(codePart1, codePart2, codePart3, codePart4, codePart5); // put all the values together to store in the database

            return _database.IsValidSchoolCode(schoolCode) ? "Code is valid." : "Code is not valid."; // return if code is valid or not
        }

        /// <summary>
        /// Determines if a given school code exists in the database.
        /// </summary>
        /// <param name="schoolCode">The school code to check the database for.</param>
        /// <returns>True if the school exists, false otherwise.</returns>
        public bool SchoolCodeExists(string schoolCode)
        {
            return _database.IsValidSchoolCode(schoolCode);
        }

        /// <summary>
        /// Returns the list of athlete forms associated with a user's school code.
        /// </summary>
        /// <param name="schoolCode">The school code to get the forms for.</param>
        /// <returns>A list of athlete forms for a school code.</returns>
        public ObservableCollection<AthleteForm>? GetForms(string schoolCode)
        {
            return _formsDatabase.SelectFormsBySchoolCode(schoolCode);
        }

        /// <summary>
        /// Retrieves all athlete forms from the database.
        /// </summary>
        /// <returns>An observable collection of all athlete forms.</returns>
        public ObservableCollection<AthleteForm> GetAllForms()
        {
            return _formsDatabase.SelectAllForms();
        }

        /// <summary>
        /// Retrieves all forms created on a specific date.
        /// </summary>
        /// <param name="schoolCode">The school code to search for forms.</param>
        /// <param name="date">The date to search for forms.</param>
        /// <returns>A list of forms.</returns>
        public ObservableCollection<AthleteForm>? GetFormsByDate(string schoolCode, DateTime date)
        {
            return _formsDatabase.SelectFormsByDate(schoolCode, date);
        }

        /// <summary>
        /// Adds a new form to the database.
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
        public string AddForm(string schoolCode, string firstName, string lastName, string sport,
                      string injuredArea, string injuredSide, string treatmentType, DateTime dateOfBirth,
                      string? athleteComments, string? status, DateTime dateCreated)
        {
            if (string.IsNullOrWhiteSpace(schoolCode) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(sport) ||
                string.IsNullOrWhiteSpace(injuredArea) ||
                string.IsNullOrWhiteSpace(injuredSide) ||
                string.IsNullOrWhiteSpace(treatmentType))
            {
                return "Error: All fields must be filled in, except for comments.";
            }

            // Create a new AthleteForm using the full constructor
            var form = new AthleteForm(
                formKey: null, // new form, so FormKey is null
                schoolCode: schoolCode,
                firstName: firstName,
                lastName: lastName,
                sport: sport,
                injuredArea: injuredArea,
                injuredSide: injuredSide,
                treatmentType: treatmentType,
                dateCreated: dateCreated,
                dateSeen: null, // date seen is optional and can be null for a new form
                dateOfBirth: dateOfBirth,
                athleteComments: athleteComments,
                status: status
            );

            // Insert the form into the database and return the result
            return _formsDatabase.InsertForm(form);
        }

        /// <summary>
        /// Deletes a form by its unique key.
        /// </summary>
        /// <param name="formKey">The unique identifier of the form to delete.</param>
        /// <returns>A message indicating if the form was successfully deleted.</returns>
        public string DeleteForm(long formKey)
        {
            return _formsDatabase.DeleteForm(formKey);
        }

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
        public string EditForm(long formKey, string schoolCode, string firstName, string lastName, string sport,
                       string injuredArea, string injuredSide, string treatmentType, DateTime dateOfBirth,
                       string? athleteComments, string status, DateTime dateCreated)
        {
            // make sure all values that are not optional are not blank
            if (string.IsNullOrWhiteSpace(schoolCode) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(sport) ||
                string.IsNullOrWhiteSpace(injuredArea) ||
                string.IsNullOrWhiteSpace(injuredSide) ||
                string.IsNullOrWhiteSpace(treatmentType) ||
                string.IsNullOrWhiteSpace(status))
            {
                return "Error: All fields must be filled in.";
            }

            // Create an updated AthleteForm object with the provided details
            var updatedForm = new AthleteForm(
                formKey: formKey,
                schoolCode: schoolCode,
                firstName: firstName,
                lastName: lastName,
                sport: sport,
                injuredArea: injuredArea,
                injuredSide: injuredSide,
                treatmentType: treatmentType,
                dateCreated: dateCreated,
                dateSeen: null,
                dateOfBirth: dateOfBirth,
                athleteComments: athleteComments,
                status: status
            );

            // Call the database method to update the form and return the result
            return _formsDatabase.UpdateForm(updatedForm);
        }

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
        public string InsertUser(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode, string key, string iv, string hashedSecurityQuestions)
        {
            return _usersDatabase.InsertUser(firstName, lastName, email, hashedPassword, schoolName, schoolCode, key, iv, hashedSecurityQuestions);
        }

        /// <summary>
        /// Retrieves the last inserted form key for a given school code.
        /// </summary>
        /// <param name="schoolCode">The school name to get the last inserted form from the database.</param>
        /// <returns>The form key that was just inserted for a school code.</returns>
        public long GetLastInsertedFormKey(string schoolCode)
        {
            return _formsDatabase.GetLastInsertedFormKey(schoolCode);
        }

        /// <summary>
        /// Validates user credentials
        /// </summary>
        /// <param name="email">The email associated with the user.</param>
        /// <param name="password">The password the user just entered.</param>
        /// <returns>True if the credentials are valid.</returns>
        public bool ValidateCredentials(string email, string password)
        {
            // checks to see if email and password are valid
            if (CredentialsValidator.isValidEmail(email) && CredentialsValidator.isValidPassword(password))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieves forms created on a specific date.
        /// </summary>
        /// <param name="schoolCode">The school code of the forms.</param>
        /// <param name="dateCreated">The date the form was created.</param>
        /// <returns>A list of AthleteForms for a certain date.</returns>
        public ObservableCollection<AthleteForm> GetFormsByDateCreated(string schoolCode, DateTime dateCreated)
        {
            return _formsDatabase.SelectFormsByDateCreated(schoolCode, dateCreated);
        }

        /// <summary>
        /// Searches for athletes based on a query string.
        /// </summary>
        /// <param name="query">The user's entered search criteria.</param>
        /// <returns>A list of AthleteForms that have the query in it anywhere.</returns>
        public ObservableCollection<AthleteForm> SearchAthletes(string query)
        {
            return _searchDatabase.SearchAthletes(query);
        }

        /// <summary>
        /// Searches athletes by contact information.
        /// </summary>
        /// <param name="query">The query to search for the athlete's contact.</param>
        /// <returns>A list of AthleteForms for the specified criteria.</returns>
        public ObservableCollection<AthleteForm> SearchAthletesByContact(string query)
        {
            return _searchDatabase.SearchAthletesByContact(query);
        }

        /// <summary>
        /// Retrieves forms created before today.
        /// </summary>
        /// <returns>A list of AthleteForms for a date less than today.</returns>
        public ObservableCollection<AthleteForm> GetFormsBeforeToday()
        {
            return _formsDatabase.SelectFormsBeforeToday();
        }

        /// <summary>
        /// Saves an updated form along with associated contacts.
        /// </summary>
        /// <param name="form">The athlete form that was updated.</param>
        /// <param name="updatedContacts">The contact for the athlete that was updated.</param>
        /// <returns>A string stating if the updated form was saved.</returns>
        public string SaveUpdatedForm(AthleteForm form, List<AthleteContact> updatedContacts)
        {
            return _formsDatabase.SaveUpdatedForm(form, updatedContacts);
        }

        /// <summary>
        /// Retrieves contacts associated with a specific form key.
        /// </summary>
        /// <param name="formKey">The form key to get the contacts by.</param>
        /// <returns>A list of AthleteContacts that correspond to the athlete's form.</returns>
        public ObservableCollection<AthleteContact> GetContactsByFormKey(long formKey)
        {
            return _contactsDatabase.SelectContactsByFormKey(formKey);
        }

        /// <summary>
        /// Searches athletes based on multiple criteria.
        /// </summary>
        /// <param name="query">The search criteria for the athletes.</param>
        /// <returns>A list of athletes that match the criteria.</returns>
        public ObservableCollection<AthleteForm> SearchAthletesByMultipleCriteria(string query)
        {
            return _searchDatabase.SearchAthletesByMultipleCriteria(query);
        }

        /// <summary>
        /// Retrieves injury statistics for all sports within a school.
        /// </summary>
        /// <param name="schoolCode">The school code to search for the forms.</param>
        /// <returns>A list of statistics for all sports for a school.</returns>
        public ObservableCollection<InjuryStatistic>? GetStatisticsForAllSports(string schoolCode)
        {
            return _database.GetStatisticsForAllSports(schoolCode);
        }

        /// <summary>
        /// Retrieves injury statistics for a specific sport within a school.
        /// </summary>
        /// <param name="schoolCode">The school code to search for the forms.</param>
        /// <param name="sport">The sport to find the statistics for.</param>
        /// <returns>A list of statistics for a certain sport for a school.</returns>
        public ObservableCollection<InjuryStatistic>? GetStatisticsForSport(string schoolCode, string sport)
        {
            return _database.GetStatisticsForSport(schoolCode, sport);
        }

        /// <summary>
        /// Retrieves all contacts associated with a specific form key.
        /// </summary>
        /// <param name="formKey">The form key associated with the contacts.</param>
        /// <returns>A collection of AthleteContact objects if they exist; otherwise, an empty collection.</returns>
        public ObservableCollection<AthleteContact> SelectContactsByFormKey(long formKey)
        {
            if (formKey <= 0)
            {
                return new ObservableCollection<AthleteContact>(); // Return an empty collection for invalid formKey.
            }

            return _contactsDatabase.SelectContactsByFormKey(formKey);
        }

        /// <summary>
        /// Inserts a new contact associated with a specific form.
        /// </summary>
        /// <param name="formKey">The form key associated with the contact.</param>
        /// <param name="contactType">The type of contact (e.g., "Guardian").</param>
        /// <param name="phoneNumber">The phone number of the contact.</param>
        /// <returns>A message indicating the result of the operation.</returns>
        public string InsertContact(long formKey, string contactType, string phoneNumber)
        {
            // check if any contact values are blank
            if (formKey <= 0 || string.IsNullOrWhiteSpace(contactType) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return "Error: All fields must be filled in.";
            }

            return _contactsDatabase.InsertContact(formKey, contactType, phoneNumber);
        }

        /// <summary>
        /// Updates the contact status of an athlete form.
        /// </summary>
        /// <param name="formKey">The unique identifier for the athlete form.</param>
        /// <param name="newStatus">The new contact status to apply.</param>
        /// <returns>A string indicating success or failure of the update.</returns>
        public string UpdateContactStatus(long formKey, string newStatus)
        {
            // validate that the new status is not null
            if (string.IsNullOrWhiteSpace(newStatus))
            {
                return "Error: Status cannot be null or empty.";
            }

            // Delegate to the database layer
            return _contactsDatabase.UpdateContactStatus(formKey, newStatus);
        }

        /// <summary>
        /// Deletes a user account based on their email.
        /// </summary>
        /// <param name="email">The email of the user to delete.</param>
        /// <returns>True if the user account was successfully deleted, otherwise false.</returns>
        public bool DeleteUserAccount(string email)
        {
            return _usersDatabase.DeleteUserAccount(email); // Pass the request to the Database class
        }

        /// <summary>
        /// Retrieves user information based on their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>A dictionary containing user information, or null if not found.</returns>
        public Dictionary<string, string> GetUserByEmail(string email)
        {
            return _usersDatabase.GetUserByEmail(email);
        }

        /// <summary>
        /// Retrieves user information based on their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>A User that stores the information from the database.</returns>
        public User GetUserFromEmail(string email)
        {
            return _usersDatabase.GetUserFromEmail(email);
        }

        /// <summary>
        /// Checks if a user exists in the database based on their email.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        public bool IsEmailRegistered(string email)
        {
            return _usersDatabase.IsEmailRegistered(email);
        }

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
        public bool UpdateUserProfile(string originalEmail, string firstName, string lastName, string schoolName, string schoolCode, string email)
        {
            return _usersDatabase.UpdateUserProfile(originalEmail, firstName, lastName, schoolName, schoolCode, email);
        }

        /// <summary>
        /// Selects the all the forms by a school code.
        /// </summary>
        /// <param name="schoolCode">The school to search for the forms.</param>
        /// <returns>A list of all the forms for a specific school.</returns>
        public ObservableCollection<AthleteForm> SelectFormsBySchoolCode(string schoolCode)
        {
            return _formsDatabase.SelectFormsBySchoolCode(schoolCode);
        }

        /// <summary>
        /// Updates the password for a user.
        /// </summary>
        /// <param name="email">The email of the user whose password is being updated.</param>
        /// <param name="hashedPassword">The new hashed password to store for the user.</param>
        /// <returns>True if the password was successfully updated; otherwise, false.</returns>
        public bool UpdateUserPassword(string email, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(hashedPassword))
            {
                throw new ArgumentException("Email and password cannot be null or empty.");
            }

            // Delegate the operation to the UsersDatabase instance.
            return _usersDatabase.UpdateUserPassword(email, hashedPassword);
        }
    }
}