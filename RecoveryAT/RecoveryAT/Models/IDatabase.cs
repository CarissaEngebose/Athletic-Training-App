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
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace RecoveryAT
{
    public interface IDatabase
    {
        /// <summary>
        /// Checks if the provided school code is valid in the database.
        /// </summary>
        /// <param name="schoolCode">The school code to validate.</param>
        /// <returns>True if the school code exists, otherwise false.</returns>
        public bool IsValidSchoolCode(string schoolCode);

        /// <summary>
        /// Gets a list of all forms on for a certain school code from the database.
        /// </summary>
        /// <param name="schoolCode">The school code to find all the forms.</param>
        /// <returns>A list of all forms for a school code, or null if none.</returns>
        ObservableCollection<AthleteForm>? SelectFormsBySchoolCode(string schoolCode);

        /// <summary>
        /// Gets a list of all forms on a certain date from the database.
        /// </summary>
        /// <param name="schoolCode">The school code to find all the forms created.</param>
        /// <param name="dateCreated">The date to find all the forms created.</param>
        /// <returns>A list of all forms on a date, or null if none.</returns>
        ObservableCollection<AthleteForm>? SelectFormsByDate(string schoolCode, DateTime dateCreated);

        /// <summary>
        /// Gets a specific form by its form key.
        /// </summary>
        /// <param name="formKey">The form key of the form to find.</param>
        /// <returns>The form object if exists; otherwise, null.</returns>
        ObservableCollection<AthleteForm> SelectForm(long formKey);

        /// <summary>
        /// Inserts a new form into the database.
        /// </summary>
        /// <param name="form">The form to insert.</param>
        /// <returns>A message saying if the form was inserted into the database or not.</returns>
        string InsertForm(AthleteForm form);

        /// <summary>
        /// Deletes a form from the database by its form key.
        /// </summary>
        /// <param name="formKey">The form key of the form to delete.</param>
        /// <returns>A message saying if the form was deleted from the database or not.</returns>
        string DeleteForm(long formKey);

        /// <summary>
        /// Updates an existing form in the database.
        /// </summary>
        /// <param name="form">The form with updated information.</param>
        /// <returns>A message saying if the form was updated in the database or not.</returns>
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
        /// <returns>A message indicating the result of the insertion.</returns>
        string InsertUser(string firstName, string lastName, string email, string hashedPassword, string schoolName, string schoolCode);
        ObservableCollection<AthleteForm> SearchAthletes(string query);

        ObservableCollection<AthleteForm> SelectAllForms();

        /// <summary>
        /// Retrieves all contacts associated with a specific form key.
        /// </summary>
        /// <param name="formKey">The form key associated with the athlete form.</param>
        /// <returns>A collection of AthleteContact objects if they exist; otherwise, an empty collection.</returns>
        ObservableCollection<AthleteContact> SelectContactsByFormKey(long formKey);
        
        long GetLastInsertedFormKey(string schoolCode);

        string InsertContact(long formKey, string contactType, string phoneNumber);
        
        string UpdateContactStatus(long? formKey, string newStatus);
        ObservableCollection<AthleteForm> SelectFormsByDateSeen(string schoolCode, DateTime dateSeen);

        /// <summary>
        /// Searches athletes by name, contact type, phone number, grade, or treatment type.
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

    }
}
