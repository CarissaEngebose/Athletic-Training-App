/*
    Date: 12/08/24
    Description: This page asks a series of security questions to verify the identity of a user trying to log in. 
                 If they answer correctly, they may reset their password.
    Bugs: None Known
    Reflection: This screen wasn’t too bad to implement, but I originally tried to do this with an email and it 
                seemed impossible. I hope this is secure enough.
*/

namespace RecoveryAT;
public partial class ResetPassword : ContentPage
{
    private readonly IBusinessLogic _businessLogic = MauiProgram.BusinessLogic; // Business logic for database operations
    private readonly string email; // Email address of the user
    private int numGuesses = 3; // Maximum number of allowed attempts

    /// <summary>
    /// Constructor initializes the page with the user's email.
    /// </summary>
    /// <param name="email">Email address of the user attempting to reset their password.</param>
    public ResetPassword(string email)
    {
        InitializeComponent(); // Load XAML components
        this.email = email; // Set email for user identification
    }

    /// <summary>
    /// Called when the user submits answers to the security questions.
    /// </summary>
    private async void OnSubmitAnswers(object sender, EventArgs e)
    {
        // Validate that all questions are selected
        if (QuestionOne.SelectedItem == null || QuestionTwo.SelectedItem == null || QuestionThree.SelectedItem == null)
        {
            await DisplayAlert("Unselected Questions", "Please select all three questions", "OK");
            return;
        }

        // Validate that all answers are provided
        if (string.IsNullOrWhiteSpace(QuestionOneEntry.Text) || 
            string.IsNullOrWhiteSpace(QuestionTwoEntry.Text) || 
            string.IsNullOrWhiteSpace(QuestionThreeEntry.Text))
        {
            await DisplayAlert("Empty Answers", "Please answer all questions", "OK");
            return;
        }

        // Concatenate answers and questions for validation
        string securityAnswers = QuestionOneEntry.Text + QuestionTwoEntry.Text + QuestionThreeEntry.Text;
        string securityQuestions = QuestionOne.SelectedIndex.ToString() + 
                                   QuestionTwo.SelectedIndex.ToString() + 
                                   QuestionThree.SelectedIndex.ToString();

        // Fetch the user based on the email
        User user = _businessLogic.GetUserFromEmail(email);

        // Validate the combined security questions and answers hash
        if (BCrypt.Net.BCrypt.Verify(securityQuestions + securityAnswers, user.HashedSecurityQuestions))
        {
            OpenResetPasswordPopup(); // Allow user to reset password
        }
        else
        {
            // Decrement the number of allowed attempts
            numGuesses--;
            if (numGuesses < 1)
            {
                await DisplayAlert("Incorrect Answers", "No more guesses left.", "OK");
                await Navigation.PopAsync(); // Return to the previous page
                return;
            }

            // Notify the user of remaining attempts
            await DisplayAlert("Incorrect Answers", $"You have {numGuesses} guesses left.", "OK");
        }
    }

    /// <summary>
    /// Displays the password reset popup.
    /// </summary>
    private void OpenResetPasswordPopup()
    {
        PasswordPopup.IsVisible = true; // Show the popup for resetting the password
    }

    /// <summary>
    /// Called when the user cancels the password reset operation.
    /// </summary>
    private void OnCancelPasswordChangeClicked(object sender, EventArgs e)
    {
        PasswordPopup.IsVisible = false; // Hide the popup
        Navigation.PopAsync(); // Return to the previous page
    }

    /// <summary>
    /// Called when the user submits a new password.
    /// </summary>
    private async void OnSubmitPasswordChangeClicked(object sender, EventArgs e)
    {
        string newPassword = NewPasswordEntry.Text; // New password entered by the user
        string confirmPassword = ConfirmPasswordEntry.Text; // Confirmation password entered by the user

        // Validate that password fields are not empty
        if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Error", "Please fill in the password fields", "OK");
            return;
        }

        // Validate that the new password and confirmation match
        if (newPassword != confirmPassword)
        {
            await DisplayAlert("Error", "New password and confirmation do not match.", "OK");
            return;
        }

        // Hash the new password and update it in the database
        string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
        bool isUpdated = _businessLogic.UpdateUserPassword(email, hashedNewPassword);

        if (isUpdated)
        {
            await DisplayAlert("Success", "Password updated successfully.", "OK");
            PasswordPopup.IsVisible = false; // Hide the popup

            // Clear the input fields
            NewPasswordEntry.Text = string.Empty;
            ConfirmPasswordEntry.Text = string.Empty;

            await Navigation.PopAsync(); // Return to the previous page
        }
        else
        {
            await DisplayAlert("Error", "Failed to update password. Please try again later.", "OK");
        }
    }
}
