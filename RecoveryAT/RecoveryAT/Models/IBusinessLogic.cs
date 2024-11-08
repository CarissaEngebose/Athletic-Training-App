/**
    Name: Carissa Engebose
    Date: 10/27/24
    Description: Business logic implementation for adding, deleting and updating a form.
    Bugs: None that I know of.
    Reflection: This class didn't take very long to create once I figured out which screens I wanted to have 
    functionality for this sprint. 
**/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RecoveryAT
{

    /// <summary>
    /// Defines the business logic related to managing the airports.
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
        public string IsValidSchoolCode(string codePart1, string codePart2, string codePart3, string codePart4, string codePart5);

        /// <summary>
        /// Gets a list of all forms from the database.
        /// </summary>
        /// <param name="schoolCode">The school code to search for forms.</param>
        /// <returns>A list of forms.</returns>
        ObservableCollection<AthleteForm>? GetForms(string schoolCode);

        /// <summary>
        /// Gets a list of all forms with a date from the database.
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
        /// <param name="grade">The athlete's grade (6 - 12).</param>
        /// <param name="sport">The sport the athlete participates in.</param>
        /// <param name="injuredArea">The location of the athlete's injury.</param>
        /// <param name="injuredSide">The side of the athlete's injury.</param>
        /// <param name="treatmentType">The type of treatment the athlete is receiving.</param>
        /// <param name="athleteComments">Comments from the athlete.</param>
        /// <returns>A message saying if the form was successfully added or not.</returns>
        public string AddForm(string schoolCode, string firstName, string lastName, int? grade, string sport,
                      string injuredArea, string injuredSide, string treatmentType,
                      string? athleteComments, string? trainerComments, string? status, DateTime date);

        /// <summary>
        /// Deletes a form by its key.
        /// </summary>
        /// <param name="formKey">The key of the form to delete.</param>
        /// <returns>A message saying if the form was successfully deleted from the database or not.</returns>
        /// 

        ObservableCollection<AthleteForm> GetFormsFromToday(string schoolCode);
        public string DeleteForm(long formKey);

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
                      string? athleteComments, string trainerComments, string status, DateTime date);

        /// <summary>
        /// Inserts a new user into the database.
        /// </summary>
        /// <param name="firstName">User's first name.</param>
        /// <param name="lastName">User's last name.</param>
        /// <param name="email">User's email address.</param>
        /// <param name="hashedPassword">User's hashed password.</param>
        /// <param name="schoolName">User's school name.</param>
        /// <param name="schoolCode">User's school code.</param>
        /// <returns>A message indicating the result of the insertion.</returns>
        string InsertUser(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode);

        public ObservableCollection<AthleteContact> GetContactsByFormKey(long formKey);
        long GetLastInsertedFormKey(string schoolCode);
    }
}
