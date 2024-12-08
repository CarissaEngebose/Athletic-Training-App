namespace RecoveryAT;

public partial class ResetPassword : ContentPage
{
    IBusinessLogic _businessLogic = MauiProgram.BusinessLogic;
    String email;
    public ResetPassword(String email)
    {
        InitializeComponent(); // Load XAML components
        this.email = email;
    }
    private void OnSubmitAnswers(object sender, EventArgs e)
    {
        //String securityQuestions = BCrypt.Net.BCrypt.HashPassword(QuestionOneAnswerEntry.Text + QuestionTwoEntry.Text + QuestionThreeEntry.Text);
        String securityQuestions = QuestionOneAnswerEntry.Text + QuestionTwoEntry.Text + QuestionThreeEntry.Text;
        User user = _businessLogic.GetUserFromEmail(email);
        if (BCrypt.Net.BCrypt.Verify(securityQuestions, user.HashedSecurityQuestions));
        {
            OpenResetPasswordPopup();
        }
    }

    private void OpenResetPasswordPopup()
    {
        PasswordPopup.IsVisible = true;
    }

    private async void OnResetPasswordSubmitClicked(object sender, EventArgs e)
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