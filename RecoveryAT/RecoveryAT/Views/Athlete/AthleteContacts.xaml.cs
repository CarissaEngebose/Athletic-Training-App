/*
    Name: Hannah Hotchkiss
    Date: 10/14/2024
    Description: AthleteContacts - This page allows users to add contact information for athletes, 
                 including their phone number and relationship to the athlete.
    Bugs: None Known
    Reflection: This screen was fairly easy as there isn't much going on. 
                The hardest part was getting the image of the garbage can for the button 
                to be exactly where I wanted it inside the frame.
*/

using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;

namespace RecoveryAT
{
    public partial class AthleteContacts : ContentPage
    {
        private readonly Database _database;
        private ObservableCollection<AthleteContact> _contacts;
        private long _formKey;

        private AuthenticationService authService;


        // Property to bind the ListView to
        public ObservableCollection<AthleteContact> Contacts
        {
            get => _contacts;
            set
            {
                _contacts = value;
                OnPropertyChanged();
            }
        }

        public AthleteContacts(long formKey)
        {
            InitializeComponent();
            authService = ((App)Application.Current).AuthService;

            _database = new Database();
            _formKey = formKey;

            // Load contacts for the specific form key
            LoadContacts();
        }

        // Loads contacts from the database
        private void LoadContacts()
        {
            Contacts = _database.SelectContactsByFormKey(_formKey);
            // Assuming there's a ListView in the XAML named ContactsListView (Add it if necessary)
            // ContactsListView.ItemsSource = Contacts;
        }

        // Event handler to add a new contact
        // Event handler to add a new contact
        private async Task<bool> OnAddContactClicked()
        {
            // Retrieve input values from Entry fields
            var contactType = RelationshipEntry.Text; // Entry for contact type
            var phoneNumber = PhoneNumberEntry.Text; // Entry for phone number

            // Regular expression for validating phone numbers (US format)
            string phonePattern = @"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$";
            bool isPhoneNumberValid = Regex.IsMatch(phoneNumber ?? string.Empty, phonePattern);

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

            // Insert new contact into the database if validation passes
            var resultMessage = _database.InsertContact(_formKey, contactType, phoneNumber);
            await DisplayAlert("Add Contact", resultMessage, "OK");

            // Refresh the contacts list if the insertion was successful
            if (resultMessage.Contains("successfully"))
            {
                LoadContacts(); // Reload contacts from the database
                ClearEntries(); // Clear the Entry fields after successful addition
                return true; // Indicate success
            }

            return false; // Indicate failure if insertion wasn't successful
        }

        // Event handler to clear the entries when delete is clicked
        private void OnDeleteContactClicked(object sender, EventArgs e)
        {
            ClearEntries();
        }

        // Clears the phone number and relationship entry fields
        private void ClearEntries()
        {
            PhoneNumberEntry.Text = string.Empty;
            RelationshipEntry.Text = string.Empty;
        }

        // Event handler to finish and navigate to TrainerHomeScreen
        private async void OnFinishClicked(object sender, EventArgs e)
        {
            bool isContactAdded = await OnAddContactClicked(); // Attempt to add contact

            // Only navigate to TrainerHomeScreen if contact addition was successful and the user is logged in
            if (isContactAdded && authService.IsLoggedIn)
            {
                await Navigation.PushAsync(new MainTabbedPage());
            } else {
                await Navigation.PushModalAsync(new WelcomeScreen());
            }
        }

    }
}
