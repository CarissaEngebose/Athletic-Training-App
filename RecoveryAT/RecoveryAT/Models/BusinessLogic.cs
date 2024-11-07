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
    /// Provides business logic operations for managing airports.
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

        /// <summary>
        /// Checks if the provided school code components are valid in the database.
        /// </summary>
        /// <param name="codePart1">First part of the school code.</param>
        /// <param name="codePart2">Second part of the school code.</param>
        /// <param name="codePart3">Third part of the school code.</param>
        /// <param name="codePart4">Fourth part of the school code.</param>
        /// <param name="codePart5">Fifth part of the school code.</param>
        /// <returns>A message indicating the result of the validation.</returns>
        public string IsValidSchoolCode(string codePart1, string codePart2, string codePart3, string codePart4, string codePart5)
        {
            // Check if any of the parts are blank
            if (string.IsNullOrWhiteSpace(codePart1) ||
                string.IsNullOrWhiteSpace(codePart2) ||
                string.IsNullOrWhiteSpace(codePart3) ||
                string.IsNullOrWhiteSpace(codePart4) ||
                string.IsNullOrWhiteSpace(codePart5))
            {
                return "Code must be 5 characters.";
            }

            // Concatenate the parts to form the full school code
            string schoolCode = string.Concat(codePart1, codePart2, codePart3, codePart4, codePart5);

            Console.WriteLine(schoolCode);

            // Check if the school code exists in the database
            if (!_database.IsValidSchoolCode(schoolCode))
            {
                return "Code is not valid.";
            }
            return "Code is valid.";
        }

        /// <summary>
        /// Gets a list of all forms from the database.
        /// </summary>
        /// <param name="schoolCode">The school code to search for forms.</param>
        /// <returns>A list of forms.</returns>
        public ObservableCollection<AthleteForm>? GetForms(string schoolCode)
        {
            return _database.SelectFormsBySchoolCode(schoolCode);
        }

        /// <summary>
        /// Retrieves the list of all forms by date.
        /// </summary>
        /// <param name="schoolCode">The school code to search for forms.</param>
        /// <param name="date">The date of the forms to retrieve.</param>
        /// <returns>A list of forms.</returns>
        public ObservableCollection<AthleteForm>? GetFormsByDate(string schoolCode, DateTime date)
        {
            return _database.SelectFormsByDate(schoolCode, date);
        }

        /// <summary>
        /// Adds a new form to the system.
        /// </summary>
        /// <param name="schoolCode">The identifier of the athlete's school.</param>
        /// <param name="firstName">The athlete's first name.</param>
        /// <param name="lastName">The athlete's last name.</param>
        /// <param name="grade">The athlete's grade (6 - 12).</param>
        /// <param name="sport">The sport the athlete participates in.</param>
        /// <param name="injuredArea">The location of the athlete's injury.</param>
        /// <param name="injuredSide">The side of the athlete's injury.</param>
        /// <param name="treatmentType">The type of treatment the athlete is receiving.</param>
        /// <param name="athleteComments">Comments from the athlete.</param>
        /// <returns>A message saying if the form was successfully added or not.</returns>
        public string AddForm(string schoolCode, string firstName, string lastName, int? grade, string sport,
                      string injuredArea, string injuredSide, string treatmentType,
                      string? athleteComments, string? trainerComments, string? status, DateTime date)
        {
            // Check if any required fields are null or empty
            if (string.IsNullOrWhiteSpace(schoolCode) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(sport) ||
                !grade.HasValue ||
                string.IsNullOrWhiteSpace(injuredArea) ||
                string.IsNullOrWhiteSpace(injuredSide) ||
                string.IsNullOrWhiteSpace(treatmentType))
            {
                return "Error: All fields must be filled in, except for comments.";
            }

            // Create a new AthleteForm with the input values
            var form = new AthleteForm(schoolCode, firstName, lastName, grade.Value, sport,
                                        injuredArea, injuredSide, treatmentType,
                                        date, athleteComments, trainerComments, status);

            // Insert the form into the database and return the result message
            return _database.InsertForm(form);
        }

        /// <summary>
        /// Deletes a form by its key.
        /// </summary>
        /// <param name="formKey">The key of the form to delete.</param>
        /// <returns>A message saying if the form was successfully deleted from the database or not.</returns>
        public string DeleteForm(long formKey)
        {
            return _database.DeleteForm(formKey);
        }

        /// <summary>
        /// Edits an existing athlete form's trainer comments and status.
        /// </summary>
        /// <param name="formKey">The identifier of the athlete's school.</param>
        /// <param name="schoolCode">The identifier of the athlete's school.</param>
        /// <param name="firstName">The athlete's first name.</param>
        /// <param name="lastName">The athlete's last name.</param>
        /// <param name="grade">The athlete's grade (6 - 12).</param>
        /// <param name="sport">The sport the athlete participates in.</param>
        /// <param name="injuredArea">The location of the athlete's injury.</param>
        /// <param name="injuredSide">The side of the athlete's injury.</param>
        /// <param name="treatmentType">The type of treatment the athlete is receiving.</param>
        /// <param name="athleteComments">Comments from the athlete.</param>
        /// <returns>A message indicating if the form was successfully updated.</returns>
        public string EditForm(long formKey, string schoolCode, string firstName, string lastName, int grade, string sport,
                      string injuredArea, string injuredSide, string treatmentType,
                      string? athleteComments, string trainerComments, string status, DateTime date)
        {
            // Validate input parameters
            // Check if any required fields are null or empty
            if (string.IsNullOrWhiteSpace(schoolCode) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(sport) ||
                string.IsNullOrWhiteSpace(injuredArea) ||
                string.IsNullOrWhiteSpace(injuredSide) ||
                string.IsNullOrWhiteSpace(treatmentType) ||
                string.IsNullOrWhiteSpace(status))
            {
                return "Error: All fields must be filled in, except for athlete comments and trainer comments.";
            }
            if (grade < 6 || grade > 12)
            {
                return "Grade must be between 6 and 12.";
            }

            // Create a new AthleteForm with the input values
            var updatedForm = new AthleteForm(formKey, schoolCode, firstName, lastName, grade, sport,
                                        injuredArea, injuredSide, treatmentType,
                                        DateTime.Now, athleteComments, trainerComments, status);

            // Call the database update method and return the result message
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

        /// <summary>
        /// Checks if a school code already exists in the database.
        /// </summary>
        /// <param name="schoolCode">The school code to check.</param>
        /// <returns>True if the school code exists; otherwise, false.</returns>
        internal bool SchoolCodeExists(string schoolCode)
        {
            return _database.IsValidSchoolCode(schoolCode);
        }

        public ObservableCollection<AthleteContact> GetContactsByFormKey(long formKey)
        {
            return _database.SelectContactsByFormKey(formKey);
        }

        public string InsertContact(long formKey, string contactType, string phoneNumber)
        {
            return _database.InsertContact(formKey, contactType, phoneNumber);
        }

        public ObservableCollection<AthleteForm> GetAllForms()
        {
            return _database.SelectAllForms();
        }

        public ObservableCollection<AthleteForm> SearchAthletes(string query)
        {
            return _database.SearchAthletes(query);
        }

        public string UpdateContactStatus(long formKey, string newStatus)
        {
            return _database.UpdateContactStatus(formKey, newStatus);
        }


    }
}
