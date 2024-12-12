/*
    Date: 12/06/2024
    Description: This page allows users to add contact information for athletes, 
                 including their phone number and relationship to the athlete.
    Bugs: None Known
    Reflection: This screen was fairly easy as there isn't much going on.
*/

using System.Collections.ObjectModel; 
using System.Text.RegularExpressions;

namespace RecoveryAT
{
    public partial class AthleteContacts : ContentPage
    {
        // Interface for accessing the business logic layer
        private IBusinessLogic _businessLogic; 

        // Collection of athlete contacts for the form, bound to the UI
        private ObservableCollection<AthleteContact> _contacts; 

        // Identifier for the form associated with the contacts
        private long _formKey; 

        // Current user of the application
        private User user; 

        // Public property for Contacts, supporting data binding and notifying UI of changes
        public ObservableCollection<AthleteContact> Contacts
        {
            get => _contacts;
            set
            {
                _contacts = value;
                OnPropertyChanged(); // Notify UI when the property changes
            }
        }

        /// <summary>
        /// Constructor for initializing the AthleteContacts page.
        /// </summary>
        /// <param name="formKey">The unique identifier for the form.</param>
        public AthleteContacts(long formKey)
        {
            InitializeComponent(); 
            
            // Retrieve the current user from the global application instance
            user = ((App)Application.Current).User; 
            
            // Initialize the business logic layer instance
            _businessLogic = MauiProgram.BusinessLogic; 
            
            // Store the form key for loading contacts
            _formKey = formKey; 

            // Load existing contacts associated with the form
            LoadContacts(); 
        }

        /// <summary>
        /// Fetches the contacts from the business logic layer and binds them to the UI.
        /// </summary>
        private void LoadContacts()
        {
            // Retrieve contacts for the current form key and bind to the Contacts property
            Contacts = _businessLogic.GetContactsByFormKey(_formKey);
        }

        /// <summary>
        /// Handles the event for adding a new contact to the form.
        /// </summary>
        /// <returns>A task indicating whether the contact was successfully added.</returns>
        private async Task<bool> OnAddContactClicked()
        {
            // Retrieve inputs for contact type and phone number
            var contactType = RelationshipEntry.Text;
            var phoneNumber = PhoneNumberEntry.Text;

            // Define a regex pattern to validate phone numbers (e.g., 123-456-7890)
            string phonePattern = @"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$";
            bool isPhoneNumberValid = Regex.IsMatch(phoneNumber ?? string.Empty, phonePattern);

            // Validate inputs and show error messages if inputs are missing or invalid
            if (string.IsNullOrWhiteSpace(contactType) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                await DisplayAlert("Validation Error", "Please enter both contact type and phone number.", "OK");
                return false;
            }

            if (!isPhoneNumberValid)
            {
                await DisplayAlert("Validation Error", "Please enter a valid phone number (e.g., 123-456-7890).", "OK");
                return false;
            }

            // Attempt to insert the contact into the database
            var resultMessage = _businessLogic.InsertContact(_formKey, contactType, phoneNumber);
            await DisplayAlert("Add Contact", resultMessage, "OK");

            // If insertion is successful, refresh the contact list and clear the inputs
            if (resultMessage.Contains("successfully"))
            {
                LoadContacts(); // Refresh the displayed contacts
                ClearEntries(); // Clear the input fields
                return true;
            }

            return false;
        }

        /// <summary>
        /// Clears the input fields for adding a new contact.
        /// </summary>
        private void OnDeleteContactClicked(object sender, EventArgs e)
        {
            ClearEntries();
        }

        /// <summary>
        /// Helper method to clear the text entries for contact type and phone number.
        /// </summary>
        private void ClearEntries()
        {
            PhoneNumberEntry.Text = string.Empty; // Clear the phone number input
            RelationshipEntry.Text = string.Empty; // Clear the relationship input
        }

        /// <summary>
        /// Handles the event when the "Finish" button is clicked.
        /// Attempts to add the contact and navigates to the appropriate page.
        /// </summary>
        private async void OnFinishClicked(object sender, EventArgs e)
        {
            // Try to add the contact and determine success
            bool isContactAdded = await OnAddContactClicked();

            // If the user is logged in and a contact was added, navigate to the MainTabbedPage
            if (isContactAdded && user.IsLoggedIn)
            {
                Application.Current.MainPage = new MainTabbedPage();
            }
            else
            {
                // Otherwise, navigate to the WelcomeScreen
                Application.Current.MainPage = new NavigationPage(new WelcomeScreen());
            }
        }
    }
}
