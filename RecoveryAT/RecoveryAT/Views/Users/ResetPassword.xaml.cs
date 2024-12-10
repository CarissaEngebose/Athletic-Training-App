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
        if(QuestionOne.SelectedItem == null || QuestionOne.SelectedItem == null || QuestionOne.SelectedItem == null){
            await DisplayAlert("Unselected Questions", "Please select all three questions", "OK");
            return;
        }
        if(string.IsNullOrWhiteSpace(QuestionOneEntry.Text) || string.IsNullOrWhiteSpace(QuestionOneEntry.Text) || string.IsNullOrWhiteSpace(QuestionOneEntry.Text)){
            await DisplayAlert("Empty Answers", "Please answer all questions", "OK");
            return;
        }
        String securityQuestions = QuestionOneEntry.Text + QuestionTwoEntry.Text + QuestionThreeEntry.Text; // security questions mushed together
        User user = _businessLogic.GetUserFromEmail(email); // user with a given email
        if (BCrypt.Net.BCrypt.Verify(securityQuestions, user.HashedSecurityQuestions)) // if security questions are correct
        {
            OpenResetPasswordPopup(); // allow user to reset password
        } else {
            numGuesses--;
            if(numGuesses < 1){
                await DisplayAlert("Incorrect Answers", "No more guesses left", "OK");
                await Navigation.PopAsync();
                return;
            }
            await DisplayAlert("Incorrect Answers", "You have " + numGuesses + " guesses left.", "OK");
        }
    }

    // Cancel button on reset password clicked
    private void OnCancelPasswordChangeClicked(object sender, EventArgs e){
        PasswordPopup.IsVisible = false;
        Navigation.PopAsync();
    }

    private void OpenResetPasswordPopup()
    {
        PasswordPopup.IsVisible = true;
    }

    private async void OnSubmitPasswordChangeClicked(object sender, EventArgs e)
    {
        string newPassword = NewPasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        // make sure password isnt empty
        if(string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword)){
            await DisplayAlert("Error", "Please fill in password field", "OK");
            return;
        }

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
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Failed to update password. Please try again later.", "OK");
        }

    }
}