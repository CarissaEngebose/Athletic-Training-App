/*
    Name: Hannah Hotchkiss
    Date: 10/14/2024
    Description: UserCreateAccount - This page allows users to create an account by providing their email, password, and confirming their password.
    Users can select a status from a dropdown menu and search for athletes using a search bar.
    Bugs: None Known
    Reflection: This screen was a easy as well because there also wasn't too much going on. The hardest part was 
    adding the aesthetic blue overlapping circles to the bottom of the screen.
*/
using System.ComponentModel;

namespace RecoveryAT
{
    public partial class UserCreateAccount : ContentPage
    {
        private IBusinessLogic _businessLogic;
        private int WORK_FACTOR = 13; // creates an int for the work factor to enhance the hash

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

            var hashedPassword = BC.EnhancedHashPassword(passwordEntry.Text, WORK_FACTOR); // hash the password

            if (!BC.EnhancedVerify(confirmPasswordEntry.Text, hashedPassword))
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // checks if entered password is strong and secure - Dominick
            CredentialsValidator.PasswordStatus passwordStatus = CredentialsValidator.ValidatePassword(passwordEntry.Text); // get password status
            if(passwordStatus != CredentialsValidator.PasswordStatus.Good){ // if password is not good
                await DisplayAlert("Error", CredentialsValidator.GetMessage(passwordStatus), "OK"); // display what needs to be fixed
                return; // horrible password, make them redo it
            }

            if(!CredentialsValidator.isValidEmail(email)){ // if email isnt formated correctly
                await DisplayAlert("Error", "Email is invalid", "OK"); // tell the user
                return; // dont create account
            }

            // Navigate to TrainerSchoolInformation and pass the collected data
            await Navigation.PushAsync(new TrainerSchoolInformation(firstName, lastName, email, hashedPassword));
        }
    }
}
