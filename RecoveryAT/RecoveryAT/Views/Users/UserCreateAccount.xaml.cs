/*
    Date: 12/06/24
    Description: This page allows users to create an account by providing their email, password, 
                 and confirming their password. Users can select a status from a dropdown menu 
                 and search for athletes using a search bar.
    Bugs: None Known
    Reflection: This screen was easy to implement because there isn't much going on.
*/

using System.Text.RegularExpressions;

namespace RecoveryAT
{
    public partial class UserCreateAccount : ContentPage
    {
        // Dependency for business logic operations
        private IBusinessLogic _businessLogic;

        // Constructor initializes the page and business logic dependency
        public UserCreateAccount()
        {
            InitializeComponent();
            _businessLogic = MauiProgram.BusinessLogic; // Retrieve business logic instance
        }

        // Handles the "Create Account" button click event
        private async void CreateAccountClicked(object sender, EventArgs e)
        {
            // Retrieve user inputs from UI fields
            var firstName = firstNameEntry.Text;
            var lastName = lastNameEntry.Text;
            var email = emailEntry.Text;

            // Validate that all required fields are filled
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || 
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passwordEntry.Text) || 
                string.IsNullOrWhiteSpace(confirmPasswordEntry.Text))
            {
                await DisplayAlert("Error", "Please fill all fields.", "OK");
                return;
            }

            // Validate email format using a helper method
            if (!CredentialsValidator.isValidEmail(email))
            {
                await DisplayAlert("Email Error", "Email format is invalid.", "OK");
                return;
            }

            // Validate password strength and requirements
            if (passwordEntry.Text.Length < 8) 
            {
                await DisplayAlert("Password Error", "Password must be at least 8 characters.", "OK");
                return;
            }
            else if (!Regex.IsMatch(passwordEntry.Text, @"[^a-zA-Z0-9]")) // Check for at least one symbol
            {
                await DisplayAlert("Password Error", "Password must contain at least one symbol.", "OK");
                return;
            }
            else if (!Regex.IsMatch(passwordEntry.Text, @"\d")) // Check for at least one digit
            {
                await DisplayAlert("Password Error", "Password must contain at least one number.", "OK");
                return;
            }

            // Ensure the password and confirmation match
            if (passwordEntry.Text != confirmPasswordEntry.Text)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // Hash the password before saving or passing it to another page
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordEntry.Text);

            // Navigate to the next page, passing collected user data
            await Navigation.PushAsync(new TrainerSchoolInformation(firstName, lastName, email, hashedPassword));
        }
    }
}
