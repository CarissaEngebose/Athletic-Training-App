/*
    Date: 12/10/2024
    Description: Allows the trainer to log in with their username and password.
    Bugs: None known.
    Reflection: This was another easy screen. The hardest part was figuring out how to do the "forgot password" link.
*/

namespace RecoveryAT;

public partial class UserLogin : ContentPage
{
    private User _user; // Represents the current user
    private readonly IBusinessLogic _businessLogic; // Interface for business logic operations

    /// <summary>
    /// Constructor for initializing the UserLogin screen.
    /// </summary>
    public UserLogin()
    {
        InitializeComponent(); // Load XAML components
        _user = ((App)Application.Current).User; // Retrieve the current user from the application instance
        _businessLogic = MauiProgram.BusinessLogic; // Initialize the business logic instance
    }

    /// <summary>
    /// Event handler for when the user taps the "Forgot Password" link.
    /// </summary>
    private async void OnForgotPasswordClicked(object sender, EventArgs e)
    {
        // Prompt the user to enter their email address
        var email = await DisplayPromptAsync(
            "Password Reset",
            "Enter your email address:",
            keyboard: Keyboard.Email); // Suggest proper keyboard for email input

        if (!string.IsNullOrWhiteSpace(email)) // Check if the email is not empty
        {
            email = email.Trim(); // Remove leading and trailing spaces

            // Validate the email format and check if it is registered
            if (CredentialsValidator.isValidEmail(email) && _businessLogic.IsEmailRegistered(email))
            {
                await Navigation.PushAsync(new ResetPassword(email)); // Navigate to the ResetPassword screen
            }
            else
            {
                // Show an alert if the email is invalid or not registered
                await DisplayAlert("Email Not Found", "The email address is not found.", "OK");
            }
        }
    }

    /// <summary>
    /// Event handler for when the user clicks the Login button.
    /// </summary>
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;

        // Check if email or password is empty
        if (string.IsNullOrWhiteSpace(PasswordEntry.Text) || string.IsNullOrWhiteSpace(email))
        {
            await DisplayAlert("Login Failed", "Email and password must be filled out.", "OK");
            return;
        }

        // Check if the email is registered
        if (!_businessLogic.IsEmailRegistered(email))
        {
            await DisplayAlert("Login Failed", "The email address is not registered.", "OK");
            return;
        }

        // Retrieve the user from the database using the email
        _user = _businessLogic.GetUserFromEmail(email);

        // Check if the user exists
        if (_user == null)
        {
            await DisplayAlert("Login Failed", "User information not found.", "OK");
            return;
        }

        // Verify the entered password against the stored hashed password
        if (_user != null && !string.IsNullOrEmpty(_user.HashedPassword) &&
            BCrypt.Net.BCrypt.Verify(PasswordEntry.Text, _user.HashedPassword))
        {
            OnLoginSuccessful(); // Proceed to login success logic
        }
        else
        {
            // Alert the user if the password is incorrect
            await DisplayAlert("Login Failed", "Incorrect Password.", "OK");
        }
    }

    /// <summary>
    /// Event handler for when the user taps the "Create Account" link.
    /// </summary>
    private async void OnCreateAccountTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserCreateAccount()); // Navigate to the UserCreateAccount screen
    }

    /// <summary>
    /// Method to handle successful login actions.
    /// </summary>
    private async void OnLoginSuccessful()
    {
        ((App)Application.Current).User = _user; // Set the current user in the application instance
        await Navigation.PushModalAsync(new MainTabbedPage()); // Navigate to the main tabbed page
    }
}
