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
    // Constructor for initializing the UserLogin screen
    public UserLogin()
    {
        InitializeComponent(); // Load XAML components
    }

    // Event handler for when the user taps the "Forgot Password" link
    private async void OnForgotPasswordClicked(object sender, EventArgs e)
    {
        // Example: Navigate to the ForgotPassword page
    }

    // Event handler for when the user clicks the Login button
    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        // Example: Perform login logic (e.g., check username and password)
        bool isAuthenticated = ValidateCredentials(); // Replace with actual logic

        if (isAuthenticated)
        {
            // Navigate to the home screen upon successful login
            await Navigation.PushAsync(new TrainerHomeScreen());
        }
        else
        {
            // Show an alert if login fails
            await DisplayAlert("Login Failed", "Invalid username or password.", "OK");
        }
    }

    // Placeholder for validating user credentials (replace with actual validation logic)
    private bool ValidateCredentials()
    {
        // Example logic: always return true for now
        return true;
    }
}
