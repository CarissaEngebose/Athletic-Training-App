/**
    Date: 12/06/24
    Description: This interface provides methods to perform operations on athlete contacts,
                 including retrieval of contacts by form keys, inserting new contacts, updating 
                 contact statuses, and deleting existing contacts.
    Bugs: None that we know of.
    Reflection: This was easy to implement.
**/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RecoveryAT
{
    /// <summary>
    /// Interface for managing contact-related data and operations for the RecoveryAT application.
    /// Provides methods for interacting with athlete contact data, including retrieval, insertion, deletion, and updates.
    /// </summary>
    public interface IContactsDatabase
    {
        /// <summary>
        /// Gets the list of all athlete forms in an observable collection.
        /// This property allows access to all forms stored in the database.
        /// </summary>
        ObservableCollection<AthleteForm> Forms { get; }

        /// <summary>
        /// Gets the list of all athlete contacts in an observable collection.
        /// This property allows access to all contacts stored in the database.
        /// </summary>
        ObservableCollection<AthleteContact> Contacts { get; }

        /// <summary>
        /// Selects all contacts from the database.
        /// Retrieves an observable collection of all athlete contacts stored in the database.
        /// </summary>
        /// <returns>An observable collection of <see cref="AthleteContact"/> objects representing all contacts.</returns>
        ObservableCollection<AthleteContact> SelectAllContacts();

        /// <summary>
        /// Inserts a new contact into the athlete_contacts table.
        /// Adds a new contact associated with a specific form to the database.
        /// </summary>
        /// <param name="formKey">The form key associated with the athlete form.</param>
        /// <param name="contactType">The type of contact (e.g., "Parent", "Coach").</param>
        /// <param name="phoneNumber">The phone number of the contact.</param>
        /// <returns>A message indicating the result of the insertion (e.g., "Contact added successfully" or an error message).</returns>
        string InsertContact(long formKey, string contactType, string phoneNumber);

        /// <summary>
        /// Deletes a contact from the database by contact ID.
        /// Removes the contact record associated with the specified contact ID.
        /// </summary>
        /// <param name="contactID">The unique identifier of the contact to delete.</param>
        /// <returns>A message indicating whether the contact was successfully deleted.</returns>
        string DeleteContact(long contactID);

        /// <summary>
        /// Updates the contact status of an athlete form.
        /// Modifies the contact status of a specific athlete form in the database.
        /// </summary>
        /// <param name="formKey">The unique identifier for the athlete form.</param>
        /// <param name="newStatus">The new contact status to apply (e.g., "Contacted", "Pending").</param>
        /// <returns>A string indicating success or failure of the update operation.</returns>
        string UpdateContactStatus(long? formKey, string newStatus);

        /// <summary>
        /// Retrieves all contacts associated with a specific athlete form.
        /// Fetches all contacts that are linked to a given form based on its unique identifier.
        /// </summary>
        /// <param name="formKey">The unique identifier of the form whose contacts are to be retrieved.</param>
        /// <returns>An observable collection of <see cref="AthleteContact"/> objects associated with the specified form.</returns>
        ObservableCollection<AthleteContact> SelectContactsByFormKey(long formKey);
    }
}
