/**
    Name: Carissa Engebose
    Date: 10/27/24
    Description: Created the business logic implementation to be used when creating a form.
    Bugs: None that I know of.
    Reflection: This class didn't take very long once I figured out the methods I wanted to start with and the
    screens I wanted to complete.
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
        private readonly IDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessLogic"/> class.
        /// </summary>
        /// <param name="database">The database to interact with.</param>
        public BusinessLogic(IDatabase database)
        {
            _database = database;
        }

        public string IsValidSchoolCode(string codePart1, string codePart2, string codePart3, string codePart4, string codePart5)
        {
            if (string.IsNullOrWhiteSpace(codePart1) ||
                string.IsNullOrWhiteSpace(codePart2) ||
                string.IsNullOrWhiteSpace(codePart3) ||
                string.IsNullOrWhiteSpace(codePart4) ||
                string.IsNullOrWhiteSpace(codePart5))
            {
                return "Code must be 5 characters.";
            }

            string schoolCode = string.Concat(codePart1, codePart2, codePart3, codePart4, codePart5);
            return _database.IsValidSchoolCode(schoolCode) ? "Code is valid." : "Code is not valid.";
        }

        public bool SchoolCodeExists(string schoolCode)
        {
            return _database.IsValidSchoolCode(schoolCode);
        }

        public ObservableCollection<AthleteForm>? GetForms(string schoolCode)
        {
            return _database.SelectFormsBySchoolCode(schoolCode);
        }

        /// <summary>
        /// Retrieves all athlete forms from the database.
        /// </summary>
        /// <returns>An observable collection of all athlete forms.</returns>
        public ObservableCollection<AthleteForm> GetAllForms()
        {
            return _database.SelectAllForms(); // Ensure SelectAllForms exists in IDatabase
        }

        public ObservableCollection<AthleteForm>? GetFormsByDate(string schoolCode, DateTime date)
        {
            return _database.SelectFormsByDate(schoolCode, date);
        }

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
                formKey: null, // New form, so FormKey is null
                schoolCode: schoolCode,
                firstName: firstName,
                lastName: lastName,
                sport: sport,
                injuredArea: injuredArea,
                injuredSide: injuredSide,
                treatmentType: treatmentType,
                dateCreated: dateCreated,
                dateSeen: null, // DateSeen is optional and can be null for a new form
                dateOfBirth: dateOfBirth,
                athleteComments: athleteComments,
                status: status
            );

            // Insert the form into the database and return the result
            return _database.InsertForm(form);
        }

        public string DeleteForm(long formKey)
        {
            return _database.DeleteForm(formKey);
        }

        public string EditForm(long formKey, string schoolCode, string firstName, string lastName, string sport,
                       string injuredArea, string injuredSide, string treatmentType, DateTime dateOfBirth,
                       string? athleteComments, string status, DateTime dateCreated)
        {
            // Validate input parameters
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
                dateSeen: null, // Assuming DateSeen is not updated here; set to null
                dateOfBirth: dateOfBirth,
                athleteComments: athleteComments,
                status: status
            );

            // Call the database method to update the form and return the result
            return _database.UpdateForm(updatedForm);
        }

        public string InsertUser(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode)
        {
            return _database.InsertUser(firstName, lastName, email, hashedPassword, schoolName, schoolCode);
        }

        public long GetLastInsertedFormKey(string schoolCode)
        {
            return _database.GetLastInsertedFormKey(schoolCode);
        }

        public bool ValidateCredentials(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return true; // Placeholder validation logic.
        }

        public ObservableCollection<AthleteForm> GetFormsByDateSeen(string schoolCode, DateTime dateSeen)
        {
            return _database.SelectFormsByDateSeen(schoolCode, dateSeen);
        }

        public ObservableCollection<AthleteForm> SearchAthletes(string query)
        {
            return _database.SearchAthletes(query);
        }

        public ObservableCollection<AthleteForm> SearchAthletesByContact(string query)
        {
            return _database.SearchAthletesByContact(query);
        }

        public ObservableCollection<AthleteForm> GetFormsBeforeToday()
        {
            return _database.SelectFormsBeforeToday();
        }

        public string SaveUpdatedForm(AthleteForm form, List<AthleteContact> updatedContacts)
        {
            return _database.SaveUpdatedForm(form, updatedContacts);
        }

        public ObservableCollection<AthleteContact> GetContactsByFormKey(long formKey)
        {
            return _database.SelectContactsByFormKey(formKey);
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

            return _database.SelectContactsByFormKey(formKey);
        }


        public ObservableCollection<AthleteForm> SearchAthletesByMultipleCriteria(string query)
        {
            return _database.SearchAthletesByMultipleCriteria(query);
        }

        public ObservableCollection<InjuryStatistic>? GetStatisticsForAllSports(string schoolCode)
        {
            return _database.GetStatisticsForAllSports(schoolCode);
        }

        public ObservableCollection<InjuryStatistic>? GetStatisticsForSport(string schoolCode, string sport)
        {
            return _database.GetStatisticsForSport(schoolCode, sport);
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
            if (formKey <= 0 || string.IsNullOrWhiteSpace(contactType) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return "Error: All fields must be filled in.";
            }

            return _database.InsertContact(formKey, contactType, phoneNumber);
        }

        /// <summary>
        /// Updates the contact status of an athlete form.
        /// </summary>
        /// <param name="formKey">The unique identifier for the athlete form.</param>
        /// <param name="newStatus">The new contact status to apply.</param>
        /// <returns>A string indicating success or failure of the update.</returns>
        public string UpdateContactStatus(long formKey, string newStatus)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(newStatus))
            {
                return "Error: Status cannot be null or empty.";
            }

            // Delegate to the database layer
            return _database.UpdateContactStatus(formKey, newStatus);
        }

        public Dictionary<string, string> GetUserByEmail(string email)
        {
            return _database.GetUserByEmail(email);
        }

        public bool DeleteUserAccount(string email)
        {
            return _database.DeleteUserAccount(email); // Pass the request to the Database class
        }

        /// <summary>
        /// Checks if a user exists in the database based on their email.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        public bool IsEmailRegistered(string email)
        {
            return _database.IsEmailRegistered(email);
        }

        public bool UpdateUserProfile(string originalEmail, string firstName, string lastName, string schoolName, string schoolCode, string email)
        {
            return _database.UpdateUserProfile(originalEmail, firstName, lastName, schoolName, schoolCode, email);
        }

        public ObservableCollection<AthleteForm> SelectFormsBySchoolCode(string schoolCode)
        {
            return _database.SelectFormsBySchoolCode(schoolCode);
        }
    }
}