/*
    Name: Dominick Hagedorn
    Date: 10/14/2024
    Description: UserLogin screen
    Bugs: None known
    Reflection: This was another easy screen. The hardest part was figuring out how to do the "forgot password" link.
*/

namespace RecoveryAT;

public partial class UserLogin : ContentPage
{
    private User _user;
    private IBusinessLogic _businessLogic;

    // Constructor for initializing the UserLogin screen
    public UserLogin()
    {
        InitializeComponent(); // Load XAML components
        _user = ((App)Application.Current).User;
        _businessLogic = MauiProgram.BusinessLogic;
    }

    // Event handler for when the user taps the "Forgot Password" link
    private async void  OnForgotPasswordClicked(object sender, EventArgs e)
    {
        var email = await DisplayPromptAsync("Password Reset", "Enter your email address:");
        if (CredentialsValidator.isValidEmail(email) && _businessLogic.IsEmailRegistered(email)){
            await Navigation.PushAsync(new ResetPassword(email));
        } else {
            await DisplayAlert("Email Not found", "The email address is not found.", "OK");
        }

    }

    // Event handler for when the user clicks the Login button
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;

        // check if values are null - Carissa
        if (string.IsNullOrWhiteSpace(PasswordEntry.Text) || string.IsNullOrWhiteSpace(email)) {
            await DisplayAlert("Login Failed", "Email and password must be filled out.", "OK");
            return;
        }
        // Check if the email is registered
        if (!_businessLogic.IsEmailRegistered(email))
        {
            await DisplayAlert("Login Failed", "The email address is not registered.", "OK");
            return;
        }

        // get the user from the email - Carissa
        _user = _businessLogic.GetUserFromEmail(email);

        if(_user == null) {
            await DisplayAlert("Login Failed", "User information not found.", "OK");
            return;
        }

        if (_user != null && !string.IsNullOrEmpty(_user.HashedPassword) && BCrypt.Net.BCrypt.Verify(PasswordEntry.Text, _user.HashedPassword))
        {
            OnLoginSuccessful(); // Pass the email to OnLoginSuccessful
        }
        else
        {
            Console.WriteLine($"Password: {PasswordEntry.Text}");
            // Show an alert if login fails
            await DisplayAlert("Login Failed", "Incorrect Password.", "OK");
        }
    }

    // Event handler for when the user taps the create account link
    private async void OnCreateAccountTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserCreateAccount()); // navigate to the create account page
    }

    private async void OnLoginSuccessful()
    {
        ((App)Application.Current).User = _user;
        await Navigation.PushModalAsync(new MainTabbedPage()); // Navigate to the main tabbed page
    }
}
