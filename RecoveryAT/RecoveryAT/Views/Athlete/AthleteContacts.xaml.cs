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
        private IBusinessLogic _businessLogic; // Interface for business logic layer to fetch and modify data.
        private ObservableCollection<AthleteContact> _contacts; // Collection of athlete contacts for data binding.
        private long _formKey; // Identifier for the form to which the contacts belong.
        private User user; // Represents the current user of the application.

        // Public property for Contacts, which supports data binding and notifies of changes.
        public ObservableCollection<AthleteContact> Contacts
        {
            get => _contacts;
            set
            {
                _contacts = value;
                OnPropertyChanged(); // Notify UI of changes.
            }
        }

        // Constructor to initialize the page with the form key.
        public AthleteContacts(long formKey)
        {
            InitializeComponent(); 
            user = ((App)Application.Current).User; // Retrieve the current user from the application instance.
            _businessLogic = MauiProgram.BusinessLogic; 
            _formKey = formKey; 

            LoadContacts(); // Load existing contacts for the given form.
        }

        // Loads contacts from the business logic layer and sets the Contacts property.
        private void LoadContacts()
        {
            Contacts = _businessLogic.GetContactsByFormKey(_formKey);
        }

        // Handles the event when the "Add Contact" button is clicked.
        private async Task<bool> OnAddContactClicked()
        {
            // Retrieve user inputs for contact type and phone number.
            var contactType = RelationshipEntry.Text;
            var phoneNumber = PhoneNumberEntry.Text;

            // Regex pattern for validating phone numbers (e.g., 123-456-7890).
            string phonePattern = @"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$";
            bool isPhoneNumberValid = Regex.IsMatch(phoneNumber ?? string.Empty, phonePattern);

            // Validate inputs and display error messages if necessary.
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

            // Insert the contact using the business logic layer and display the result message.
            var resultMessage = _businessLogic.InsertContact(_formKey, contactType, phoneNumber);
            await DisplayAlert("Add Contact", resultMessage, "OK");

            // Reload the contact list and clear inputs if the insertion was successful.
            if (resultMessage.Contains("successfully"))
            {
                LoadContacts(); // Refresh the contact list.
                ClearEntries(); // Clear input fields.
                return true;
            }

            return false;
        }

        // Clears the input fields for adding a new contact.
        private void OnDeleteContactClicked(object sender, EventArgs e)
        {
            ClearEntries();
        }

        // Helper method to clear text entries.
        private void ClearEntries()
        {
            PhoneNumberEntry.Text = string.Empty;
            RelationshipEntry.Text = string.Empty;
        }

        // Handles the event when the "Finish" button is clicked.
        private async void OnFinishClicked(object sender, EventArgs e)
        {
            // Attempt to add the current contact.
            bool isContactAdded = await OnAddContactClicked();

            // Navigate to the appropriate page based on the user's login status.
            if (isContactAdded && user.IsLoggedIn)
            {
                // Set MainTabbedPage as the root page if logged in.
                Application.Current.MainPage = new MainTabbedPage();
            }
            else
            {
                // Set WelcomeScreen as the root page otherwise.
                Application.Current.MainPage = new WelcomeScreen();
            }
        }
    }
}
