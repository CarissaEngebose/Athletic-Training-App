

using System.Text.RegularExpressions;


/**
       Date: 12/05/24
       Description: This is a profile editing screen where users can update their personal details, 
                    such as first name, last name, school name, school code, and email. 
                    The data is retrieved and bound to the current logged-in user's profile.
       Bugs: None that we know of.
       Reflection: This screen was easy to implement. The hardest part was ensuring the binding 
                   correctly reflected the logged-in user's data.
**/
namespace RecoveryAT
{
    public partial class UserProfileEdit : ContentPage
    {
        private User _user; // Represents the currently logged-in user.

        public UserProfileEdit()
        {
            InitializeComponent();
            _user = ((App)Application.Current).User; // Retrieve the current user from the application instance.
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string email = _user.Email; // Get the logged-in user's email.

            if (string.IsNullOrWhiteSpace(email))
            {
                // Alert the user if email retrieval fails.
                _ = DisplayAlert("Error", "Could not retrieve user data. Please log in again.", "OK");
                return;
            }

            if (_user != null)
            {
                // Populate UI fields with decrypted user data.
                FirstNameEntry.Text = _user.FirstName;
                LastNameEntry.Text = _user.LastName;
                SchoolNameEntry.Text = EncryptionHelper.Decrypt(_user.SchoolName, _user.Key, _user.IV);
                SchoolCodeEntry.Text = _user.SchoolCode;
                EmailEntry.Text = _user.Email;
            }
            else
            {
                // Alert the user if data loading fails.
                _ = DisplayAlert("Error", "User data could not be loaded.", "OK");
            }
        }

        // Event handler for Change Password click.
        private void OnChangePasswordClicked(object sender, EventArgs e)
        {
            // Show the password change popup.
            PasswordPopup.IsVisible = true;
        }

        // Event handler for canceling the password change.
        private void OnCancelPasswordChangeClicked(object sender, EventArgs e)
        {
            // Hide the password change popup.
            PasswordPopup.IsVisible = false;
        }

        // Event handler for submitting the password change.
        private async void OnSubmitPasswordChangeClicked(object sender, EventArgs e)
        {
            // Retrieve user inputs
            string currentPassword = CurrentPasswordEntry.Text;
            string newPassword = NewPasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // Check if any fields are empty
            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                await DisplayAlert("Error", "Please enter your current password.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                await DisplayAlert("Error", "Please enter a new password.", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Error", "Please confirm your new password.", "OK");
                return;
            }

            // Validate password strength and requirements
            if (newPassword.Length < 8)
            {
                await DisplayAlert("Password Error", "Password must be at least 8 characters.", "OK");
                return;
            }
            else if (!Regex.IsMatch(newPassword, @"[^a-zA-Z0-9]")) // Check for at least one symbol
            {
                await DisplayAlert("Password Error", "Password must contain at least one symbol.", "OK");
                return;
            }
            else if (!Regex.IsMatch(newPassword, @"\d")) // Check for at least one digit
            {
                await DisplayAlert("Password Error", "Password must contain at least one number.", "OK");
                return;
            }

            // Check if the new password and confirm password match
            if (newPassword != confirmPassword)
            {
                await DisplayAlert("Error", "New password and confirmation do not match.", "OK");
                return;
            }

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, _user.HashedPassword))
            {
                await DisplayAlert("Error", "Current password is incorrect.", "OK");
                return;
            }

            // Check if the new password is the same as the current password
            if (BCrypt.Net.BCrypt.Verify(newPassword, _user.HashedPassword))
            {
                await DisplayAlert("Error", "New password cannot be the same as the current password.", "OK");
                return;
            }

            // Update the password in the database
            string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            bool isUpdated = ((App)Application.Current).BusinessLogic.UpdateUserPassword(_user.Email, hashedNewPassword);

            if (isUpdated)
            {
                await DisplayAlert("Success", "Password updated successfully.", "OK");
                PasswordPopup.IsVisible = false;

                // Clear input fields after successful update
                CurrentPasswordEntry.Text = string.Empty;
                NewPasswordEntry.Text = string.Empty;
                ConfirmPasswordEntry.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Error", "Failed to update password. Please try again later.", "OK");
            }
        }

        // Event handler for cancel button click.
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Navigate back to the UserProfile screen.
            _ = await Navigation.PopAsync();
        }

        // Event handler for save button click.
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Collect updated user profile data.
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string schoolCode = SchoolCodeEntry.Text;
            string email = EmailEntry.Text;

            // Check if the school code already exists in the database
            if (_user.SchoolCode != schoolCode && MauiProgram.BusinessLogic.SchoolCodeExists(schoolCode))
            {
                await DisplayAlert("Error", "This school code already exists. Please enter a unique code.", "OK");
                return;
            }

            if (_user.Email != email && MauiProgram.BusinessLogic.IsEmailRegistered(email))
            { // check to see if email is already in use
                await DisplayAlert("Error", "Email is already in use", "OK");
                return;
            }

            var encryptedSchoolName = EncryptionHelper.Encrypt(SchoolNameEntry.Text, _user.Key, _user.IV); // encrypt the new school name

            // Update user profile in the database via BusinessLogic.
            bool isUpdated = ((App)Application.Current).BusinessLogic.UpdateUserProfile(
                _user.Email, // Use the original email as identifier.
                firstName,
                lastName,
                encryptedSchoolName,
                schoolCode,
                email
            );

            if (isUpdated)
            {
                // Show success message and navigate back to the UserProfile page.
                await DisplayAlert("Success", "Profile updated successfully.", "OK");

                // update the user information for the app
                ((App)Application.Current).User = ((App)Application.Current).BusinessLogic.GetUserFromEmail(_user.Email);
                _user = ((App)Application.Current).User;

                FirstNameEntry.Text = _user.FirstName;
                LastNameEntry.Text = _user.LastName;
                SchoolNameEntry.Text = EncryptionHelper.Decrypt(_user.SchoolName, _user.Key, _user.IV);
                SchoolCodeEntry.Text = _user.SchoolCode;
                EmailEntry.Text = _user.Email;

                _ = await Navigation.PopAsync();
            }
            else
            {
                // Show error message if update fails.
                await DisplayAlert("Error", "Failed to update profile. Please try again later.", "OK");
            }
        }
    }
}
