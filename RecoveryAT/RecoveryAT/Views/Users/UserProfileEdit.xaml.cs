/*
    Date: 12/05/24
    Description: This is a profile editing screen where users can update their personal details, 
                 such as first name, last name, school name, school code, and email. 
                 The data is retrieved and bound to the current logged-in user's profile.
    Bugs: None that we know of.
    Reflection: This screen was easy to implement. The hardest part was ensuring the binding 
                correctly reflected the logged-in user's data.
*/

using System.Text.RegularExpressions;

namespace RecoveryAT
{
    public partial class UserProfileEdit : ContentPage
    {
        private User _user; // Represents the currently logged-in user.

        /// <summary>
        /// Constructor to initialize the UserProfileEdit screen.
        /// </summary>
        public UserProfileEdit()
        {
            InitializeComponent();
            _user = ((App)Application.Current).User; // Retrieve the current user from the application instance.
        }

        /// <summary>
        /// Ensures the user data is loaded when the page appears.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            string email = _user?.Email;

            if (string.IsNullOrWhiteSpace(email))
            {
                // Alert the user if email retrieval fails.
                _ = DisplayAlert("Error", "Could not retrieve user data. Please log in again.", "OK");
                return;
            }

            if (_user != null)
            {
                // Populate the UI with decrypted user data.
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

        /// <summary>
        /// Opens the popup for changing the user's password.
        /// </summary>
        private void OnChangePasswordClicked(object sender, EventArgs e)
        {
            PasswordPopup.IsVisible = true;
        }

        /// <summary>
        /// Cancels the password change operation and hides the popup.
        /// </summary>
        private void OnCancelPasswordChangeClicked(object sender, EventArgs e)
        {
            PasswordPopup.IsVisible = false;
        }

        /// <summary>
        /// Handles the password change submission.
        /// </summary>
        private async void OnSubmitPasswordChangeClicked(object sender, EventArgs e)
        {
            string currentPassword = CurrentPasswordEntry.Text;
            string newPassword = NewPasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // Validate password inputs
            if (string.IsNullOrWhiteSpace(currentPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Error", "All password fields are required.", "OK");
                return;
            }

            if (newPassword.Length < 8 || !Regex.IsMatch(newPassword, @"[^a-zA-Z0-9]") || !Regex.IsMatch(newPassword, @"\d"))
            {
                await DisplayAlert("Password Error", "Password must be at least 8 characters, contain one symbol, and one number.", "OK");
                return;
            }

            if (newPassword != confirmPassword)
            {
                await DisplayAlert("Error", "New password and confirmation do not match.", "OK");
                return;
            }

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, _user.HashedPassword))
            {
                await DisplayAlert("Error", "Current password is incorrect.", "OK");
                return;
            }

            if (BCrypt.Net.BCrypt.Verify(newPassword, _user.HashedPassword))
            {
                await DisplayAlert("Error", "New password cannot be the same as the current password.", "OK");
                return;
            }

            // Update the password
            string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            bool isUpdated = ((App)Application.Current).BusinessLogic.UpdateUserPassword(_user.Email, hashedNewPassword);

            if (isUpdated)
            {
                await DisplayAlert("Success", "Password updated successfully.", "OK");
                PasswordPopup.IsVisible = false;
                CurrentPasswordEntry.Text = string.Empty;
                NewPasswordEntry.Text = string.Empty;
                ConfirmPasswordEntry.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Error", "Failed to update password. Please try again.", "OK");
            }
        }

        /// <summary>
        /// Cancels profile editing and navigates back to the UserProfile screen.
        /// </summary>
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            _ = await Navigation.PopAsync();
        }

        /// <summary>
        /// Saves the updated profile information.
        /// </summary>
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string schoolCode = SchoolCodeEntry.Text;
            string email = EmailEntry.Text;

            // Validate unique school code
            if (_user.SchoolCode != schoolCode && MauiProgram.BusinessLogic.SchoolCodeExists(schoolCode))
            {
                await DisplayAlert("Error", "This school code already exists. Please enter a unique code.", "OK");
                return;
            }

            // Validate unique email
            if (_user.Email != email && MauiProgram.BusinessLogic.IsEmailRegistered(email))
            {
                await DisplayAlert("Error", "Email is already in use.", "OK");
                return;
            }

            // Encrypt the updated school name
            var encryptedSchoolName = EncryptionHelper.Encrypt(SchoolNameEntry.Text, _user.Key, _user.IV);

            // Update the profile
            bool isUpdated = ((App)Application.Current).BusinessLogic.UpdateUserProfile(
                _user.Email, // Use the original email as identifier
                firstName,
                lastName,
                encryptedSchoolName,
                schoolCode,
                email
            );

            if (isUpdated)
            {
                await DisplayAlert("Success", "Profile updated successfully.", "OK");

                // Refresh the user data
                ((App)Application.Current).User = ((App)Application.Current).BusinessLogic.GetUserFromEmail(_user.Email);
                _user = ((App)Application.Current).User;

                // Navigate back to UserProfile screen
                _ = await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to update profile. Please try again later.", "OK");
            }
        }
    }
}
