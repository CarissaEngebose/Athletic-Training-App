using Android.Media.Metrics;

namespace RecoveryAT;
/*
    Date: 12/08/24
    Description: This page asks a series of security questions in order to 
    verify the identity of a user trying to login. if they answer correctly,
    they may reset their password.
    Bugs: None Known
    Reflection: This screen wasnt too bad to implement, but I originally tried to
    do this with an email and it seemed impossible. I hope this is secure enough.
*/
public partial class ResetPassword : ContentPage
{
    IBusinessLogic _businessLogic = MauiProgram.BusinessLogic;
    String email;
    int numGuesses = 3;
    public ResetPassword(String email)
    {
        InitializeComponent(); // Load XAML components
        this.email = email;
    }
    /// <summary>
    /// Called when a user trys to submit answers to the questions
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnSubmitAnswers(object sender, EventArgs e)
    {
        String securityQuestions = QuestionOneEntry.Text + QuestionTwoEntry.Text + QuestionThreeEntry.Text; // security questions mushed together
        User user = _businessLogic.GetUserFromEmail(email); // user with a given email
        Console.WriteLine("AHHHHHHHHHHHHHHH" + user.HashedSecurityQuestions);
        if (BCrypt.Net.BCrypt.Verify(securityQuestions, user.HashedSecurityQuestions)) // if security questions are correct
        {
            OpenResetPasswordPopup(); // allow user to reset password
        } else {
            numGuesses--;
            if(numGuesses < 1){
                await DisplayAlert("Error", "No more guesses left", "OK");
                await Navigation.PopAsync();
                return;
            }
            await DisplayAlert("Error", "One or more incorrect answers - Number of guesses left: " + numGuesses, "OK");
        }
    }

    // Cancel button on reset password clicked
    private void OnCancelPasswordChangeClicked(object sender, EventArgs e){
        PasswordPopup.IsVisible = false;
    }

    private void OpenResetPasswordPopup()
    {
        PasswordPopup.IsVisible = true;
    }

    private async void OnSubmitPasswordChangeClicked(object sender, EventArgs e)
    {
        string newPassword = NewPasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        // Check if new password and confirm password match.
        if (newPassword != confirmPassword)
        {
            await DisplayAlert("Error", "New password and confirmation do not match.", "OK");
            return;
        }


        // Update the password in the database.
        string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
        bool isUpdated = ((App)Application.Current).BusinessLogic.UpdateUserPassword(email, hashedNewPassword);

        if (isUpdated)
        {
            await DisplayAlert("Success", "Password updated successfully.", "OK");
            PasswordPopup.IsVisible = false;

            // Clear input fields after successful update.
            NewPasswordEntry.Text = string.Empty;
            ConfirmPasswordEntry.Text = string.Empty;
        }
        else
        {
            await DisplayAlert("Error", "Failed to update password. Please try again later.", "OK");
        }

    }
}