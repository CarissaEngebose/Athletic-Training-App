/*
    Date: 12/06/24
    Description: This page allows users to create an account by providing their email, password, 
                 and confirming their password. Users must also answer security questions for 
                 account recovery.
    Bugs: None Known
    Reflection: This screen was easy to implement because there isn't much going on.
*/

using System.Text.RegularExpressions;

namespace RecoveryAT
{
    public partial class UserCreateAccount : ContentPage
    {
        // Dependency for business logic operations
        private readonly IBusinessLogic _businessLogic;

        /// <summary>
        /// Constructor initializes the page and sets up the business logic dependency.
        /// </summary>
        public UserCreateAccount()
        {
            InitializeComponent();
            _businessLogic = MauiProgram.BusinessLogic; // Retrieve business logic instance
        }

        /// <summary>
        /// Handles the "Create Account" button click event. Validates user input and navigates to the next page.
        /// </summary>
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

            // Validate email format
            if (!CredentialsValidator.isValidEmail(email))
            {
                await DisplayAlert("Email Error", "Email format is invalid.", "OK");
                return;
            }

            // Validate password strength
            if (passwordEntry.Text.Length < 8)
            {
                await DisplayAlert("Password Error", "Password must be at least 8 characters long.", "OK");
                return;
            }
            else if (!Regex.IsMatch(passwordEntry.Text, @"[^a-zA-Z0-9]")) // Check for at least one symbol
            {
                await DisplayAlert("Password Error", "Password must contain at least one symbol.", "OK");
                return;
            }
            else if (!Regex.IsMatch(passwordEntry.Text, @"\d")) // Check for at least one number
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

            // Ensure all security questions are selected
            if (QuestionOne.SelectedItem == null || QuestionTwo.SelectedItem == null || QuestionThree.SelectedItem == null)
            {
                await DisplayAlert("Unselected Questions", "Please select all three questions.", "OK");
                return;
            }

            // Ensure all security questions are answered
            if (string.IsNullOrWhiteSpace(QuestionOneEntry.Text) || 
                string.IsNullOrWhiteSpace(QuestionTwoEntry.Text) || 
                string.IsNullOrWhiteSpace(QuestionThreeEntry.Text))
            {
                await DisplayAlert("Error", "Please answer all security questions.", "OK");
                return;
            }

            // Check if the email is already registered
            if (_businessLogic.IsEmailRegistered(email))
            {
                await DisplayAlert("Error", "Email is already in use.", "OK");
                return;
            }

            // Hash the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordEntry.Text);

            // Hash security questions and answers
            string securityAnswers = QuestionOneEntry.Text + QuestionTwoEntry.Text + QuestionThreeEntry.Text;
            string securityQuestions = QuestionOne.SelectedIndex.ToString() + 
                                       QuestionTwo.SelectedIndex.ToString() + 
                                       QuestionThree.SelectedIndex.ToString();
            string hashedSecurityQuestions = BCrypt.Net.BCrypt.HashPassword(securityQuestions + securityAnswers);

            // Navigate to the next page, passing collected user data
            await Navigation.PushAsync(new TrainerSchoolInformation(firstName, lastName, email, hashedPassword, hashedSecurityQuestions));
        }
    }
}
