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
using System.Threading.Tasks;

namespace RecoveryAT
{
    public partial class AthleteContacts : ContentPage
    {
        private IBusinessLogic _businessLogic;
        private ObservableCollection<AthleteContact> _contacts;
        private long _formKey;
        private User user;

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
            user = ((App)Application.Current).User;
            _businessLogic = MauiProgram.BusinessLogic;
            _formKey = formKey;

            LoadContacts();
        }

        private void LoadContacts()
        {
            Contacts = _businessLogic.GetContactsByFormKey(_formKey);
        }

        private async Task<bool> OnAddContactClicked()
        {
            var contactType = RelationshipEntry.Text;
            var phoneNumber = PhoneNumberEntry.Text;

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

            var resultMessage = _businessLogic.InsertContact(_formKey, contactType, phoneNumber);
            await DisplayAlert("Add Contact", resultMessage, "OK");

            if (resultMessage.Contains("successfully"))
            {
                LoadContacts();
                ClearEntries();
                return true;
            }

            return false;
        }

        private void OnDeleteContactClicked(object sender, EventArgs e)
        {
            ClearEntries();
        }

        private void ClearEntries()
        {
            PhoneNumberEntry.Text = string.Empty;
            RelationshipEntry.Text = string.Empty;
        }

        private async void OnFinishClicked(object sender, EventArgs e)
        {
            bool isContactAdded = await OnAddContactClicked();

            if (isContactAdded && user.IsLoggedIn)
            {
                // Set MainTabbedPage as the root page if logged in
                Application.Current.MainPage = new MainTabbedPage();
            }
            else
            {
                // Set WelcomeScreen as the root page if logged in
                Application.Current.MainPage = new WelcomeScreen();
            }
        }
    }
}