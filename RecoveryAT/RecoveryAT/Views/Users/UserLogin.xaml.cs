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
    private AuthenticationService authService;
    private IBusinessLogic _businessLogic;

    // Constructor for initializing the UserLogin screen
    public UserLogin()
    {
        InitializeComponent(); // Load XAML components
        authService = ((App)Application.Current).AuthService;
        _businessLogic = MauiProgram.BusinessLogic;
    }

    // Event handler for when the user taps the "Forgot Password" link
    private void OnForgotPasswordClicked(object sender, EventArgs e)
    {
        // Example: Navigate to the ForgotPassword page
    }

    // Event handler for when the user clicks the Login button
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text as string;
        string password = PasswordEntry.Text as string;

        // Check if the email is registered
        if (!_businessLogic.IsEmailRegistered(email))
        {
            await DisplayAlert("Login Failed", "The email address is not registered.", "OK");
            return;
        }

        // Validate the user's credentials
        bool isAuthenticated = _businessLogic.ValidateCredentials(email, password);

        if (isAuthenticated)
        {
            OnLoginSuccessful(email); // Pass the email to OnLoginSuccessful
        }
        else
        {
            // Show an alert if login fails
            await DisplayAlert("Login Failed", "Invalid username or password.", "OK");
        }
    }

    // Event handler for when the user taps the create account link
    private async void OnCreateAccountTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserCreateAccount()); // navigate to the create account page
    }

    private async void OnLoginSuccessful(string email)
    {
        authService.Login(email); // Pass the logged-in user's email to the authentication service
        await Navigation.PushModalAsync(new MainTabbedPage()); // Navigate to the main tabbed page
    }
}
