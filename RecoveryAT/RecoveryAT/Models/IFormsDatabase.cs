/**
    Date: 12/06/24
    Description: This interface provides methods to perform operations on forms,
                 including retrieval of forms, inserting new forms, updating 
                 forms, and deleting forms.
    Bugs: None that we know of.
    Reflection: This was easy to implement.
**/

using System.Collections.ObjectModel;

namespace RecoveryAT
{
    /// <summary>
    /// Interface for managing athlete forms and their associated data.
    /// </summary>
    public interface IFormsDatabase
    {
        /// <summary>
        /// Gets the list of all forms in an observable collection.
        /// </summary>
        ObservableCollection<AthleteForm> Forms { get; }

        /// <summary>
        /// Retrieves a list of forms for a specific school code.
        /// </summary>
        /// <param name="schoolCode">The school code to filter forms.</param>
        /// <returns>A collection of forms for the given school code.</returns>
        ObservableCollection<AthleteForm> SelectFormsBySchoolCode(string schoolCode);

        /// <summary>
        /// Retrieves a list of forms filtered by school code and date created.
        /// </summary>
        /// <param name="schoolCode">The school code to filter forms.</param>
        /// <param name="dateCreated">The date the forms were created.</param>
        /// <returns>A collection of forms matching the criteria.</returns>
        ObservableCollection<AthleteForm> SelectFormsByDate(string schoolCode, DateTime dateCreated);

        /// <summary>
        /// Retrieves forms filtered by school code and date of creation.
        /// </summary>
        /// <param name="schoolCode">The school code to filter forms.</param>
        /// <param name="dateCreated">The date the forms were created.</param>
        /// <returns>A collection of forms matching the criteria.</returns>
        ObservableCollection<AthleteForm> SelectFormsByDateCreated(string schoolCode, DateTime dateCreated);

        /// <summary>
        /// Retrieves a specific form by its key.
        /// </summary>
        /// <param name="formKey">The unique identifier of the form.</param>
        /// <returns>A collection containing the specified form.</returns>
        ObservableCollection<AthleteForm> SelectForm(long formKey);

        /// <summary>
        /// Inserts a new athlete form into the database.
        /// </summary>
        /// <param name="form">The form to insert.</param>
        /// <returns>A message indicating the success or failure of the operation.</returns>
        string InsertForm(AthleteForm form);

        /// <summary>
        /// Deletes a specific form by its key.
        /// </summary>
        /// <param name="formKey">The unique identifier of the form to delete.</param>
        /// <returns>A message indicating the success or failure of the operation.</returns>
        string DeleteForm(long formKey);

        /// <summary>
        /// Updates the details of an existing form.
        /// </summary>
        /// <param name="form">The form with updated information.</param>
        /// <returns>A message indicating the success or failure of the operation.</returns>
        string UpdateForm(AthleteForm form);

        /// <summary>
        /// Retrieves all forms from the database.
        /// </summary>
        /// <returns>A collection of all forms.</returns>
        ObservableCollection<AthleteForm> SelectAllForms();

        /// <summary>
        /// Retrieves forms created before today's date.
        /// </summary>
        /// <returns>A collection of forms created before today.</returns>
        ObservableCollection<AthleteForm> SelectFormsBeforeToday();

        /// <summary>
        /// Retrieves the last inserted form key for a specific school code.
        /// </summary>
        /// <param name="schoolCode">The school code to retrieve the last form for.</param>
        /// <returns>The form key of the last inserted form.</returns>
        long GetLastInsertedFormKey(string schoolCode);

        /// <summary>
        /// Saves updated information for a form and its associated contacts.
        /// </summary>
        /// <param name="form">The form with updated details.</param>
        /// <param name="updatedContacts">The updated list of contacts associated with the form.</param>
        /// <returns>A message indicating the success or failure of the operation.</returns>
        string SaveUpdatedForm(AthleteForm form, List<AthleteContact> updatedContacts);
    }
}
