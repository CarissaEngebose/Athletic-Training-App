/*
    Name: Hannah Hotchkiss
    Date: 10/14/2024
    Description: UserCreateAccount - This page allows users to create an account by providing their email, password, and confirming their password.
    Users can select a status from a dropdown menu and search for athletes using a search bar.
    Bugs: None Known
    Reflection: This screen was a easy as well because there also wasn't too much going on. The hardest part was 
    adding the aesthetic blue overlapping circles to the bottom of the screen.
*/

namespace RecoveryAT
{
    public partial class UserCreateAccount : ContentPage
    {
        private readonly BusinessLogic _businessLogic;

        public UserCreateAccount()
        {
            InitializeComponent();
            _businessLogic = new BusinessLogic(new Database()); // Initialize BusinessLogic with Database
        }

        private async void CreateAccountClicked(object sender, EventArgs e)
        {
            // Retrieve user input
            var firstName = firstNameEntry.Text;
            var lastName = lastNameEntry.Text;
            var email = emailEntry.Text;
            var password = passwordEntry.Text;
            var confirmPassword = confirmPasswordEntry.Text;

            // Validate user input
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || 
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || 
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Error", "Please fill all fields.", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // checks if entered password is strong and secure - Dominick
            CredentialsValidator.PasswordStatus passwordStatus = CredentialsValidator.ValidatePassword(password); // get password status
            if(passwordStatus != CredentialsValidator.PasswordStatus.Good){ // if password is not good
                await DisplayAlert("Error", CredentialsValidator.GetMessage(passwordStatus), "OK"); // display what needs to be fixed
                return; // horrible password, make them redo it
            }

            if(!CredentialsValidator.isValidEmail(email)){ // if email isnt formated correctly
                await DisplayAlert("Error", "Email is invalid", "OK"); // tell the user
                return; // dont create account
            }

            // Optionally, hash the password (replace with actual hashing)
            var hashedPassword = "hashed_password_example"; // Replace with hashing code

            // Navigate to TrainerSchoolInformation and pass the collected data
            await Navigation.PushAsync(new TrainerSchoolInformation(firstName, lastName, email, hashedPassword));
        }
    }
}
