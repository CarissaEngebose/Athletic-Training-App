/*
    Name: Dominick Hagedorn
    Date: 10/14/2024
    Description: UserLogin screen
    Bugs: None known
    Reflection: This was another easy screen. The hardest part was figuring out how to do the "forgot password" link.
*/

namespace RecoveryAT;

public partial class UserLogin : ContentPage {
    private AuthenticationService authService;
    private IBusinessLogic _businessLogic;

    // Constructor for initializing the UserLogin screen
    public UserLogin() {
        InitializeComponent(); // Load XAML components
        authService = ((App)Application.Current).AuthService;
        _businessLogic = MauiProgram.BusinessLogic;
    }

    // Event handler for when the user taps the "Forgot Password" link
    private void OnForgotPasswordClicked(object sender, EventArgs e) {
        // Example: Navigate to the ForgotPassword page
    }

    // Event handler for when the user clicks the Login button
    private async void OnLoginClicked(object sender, EventArgs e) {
        // Example: Perform login logic (e.g., check username and password)
        string email = EmailEntry.Text as string;
        string password = PasswordEntry.Text as string;

        bool isAuthenticated = _businessLogic.ValidateCredentials(email, password);

        if (isAuthenticated)
        {
            OnLoginSuccessful();
        }
        else
        {
            // Show an alert if login fails
            await DisplayAlert("Login Failed", "Invalid username or password.", "OK");
        }
    }

    // Event handler for when the user taps the create account link
    private async void OnCreateAccountTapped(object sender, EventArgs e) {
        await Navigation.PushAsync(new UserCreateAccount()); // navigate to the create account page
    }

    private async void OnLoginSuccessful() {
        authService.Login(); // log the user in
        await Navigation.PushModalAsync(new MainTabbedPage()); // navigate to the main tabbed page
    }
}
