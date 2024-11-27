/*
    Name: Hannah Hotchkiss
    Date: 10/14/2024
    Description: UserCreateAccount - This page allows users to create an account by providing their email, password, and confirming their password.
    Users can select a status from a dropdown menu and search for athletes using a search bar.
    Bugs: None Known
    Reflection: This screen was a easy as well because there also wasn't too much going on. The hardest part was 
    adding the aesthetic blue overlapping circles to the bottom of the screen.
*/
using System.Text.RegularExpressions;

namespace RecoveryAT
{
    public partial class UserCreateAccount : ContentPage
    {
        private IBusinessLogic _businessLogic;

        public UserCreateAccount()
        {
            InitializeComponent();
            _businessLogic = MauiProgram.BusinessLogic;
        }

        private async void CreateAccountClicked(object sender, EventArgs e)
        {
            // Retrieve user input
            var firstName = firstNameEntry.Text;
            var lastName = lastNameEntry.Text;
            var email = emailEntry.Text;

            // Validate user input
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || 
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passwordEntry.Text) || 
                string.IsNullOrWhiteSpace(confirmPasswordEntry.Text))
            {
                await DisplayAlert("Error", "Please fill all fields.", "OK");
                return;
            }

            // checks email format
            if(!CredentialsValidator.isValidEmail(email)) {
                await DisplayAlert("Email Error", "Email format is invalid", "OK"); // tell the user
                return; // dont create account
            }

            if (passwordEntry.Text.Length < 8) {
                await DisplayAlert("Password Error", "Password must be at least 8 characters.", "OK");
                return;
            }
            else if (!Regex.IsMatch(passwordEntry.Text, @"[^a-zA-Z0-9]"))
            {
                await DisplayAlert("Password Error", "Password must contain at least one symbol.", "OK");
                return;
            }
            else if (!Regex.IsMatch(passwordEntry.Text, @"\d"))
            {
                await DisplayAlert("Password Error", "Password must contain at least one number.", "OK");
                return;
            }

            if (passwordEntry.Text != confirmPasswordEntry.Text) // checks if the passwords match
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordEntry.Text); // hashes the password if it is strong and secure

            // Navigate to TrainerSchoolInformation and pass the collected data
            await Navigation.PushAsync(new TrainerSchoolInformation(firstName, lastName, email, hashedPassword));
        }
    }
}
