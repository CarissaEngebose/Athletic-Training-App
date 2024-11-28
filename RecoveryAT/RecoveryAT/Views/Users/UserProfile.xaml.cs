/*
    Name: Carissa Engebose
    Date: 10/9/2024
    Description: A screen for the athletic trainer to view and edit profile information.
    Bugs: None that I know of.
    Reflection: This screen was a little difficult because of all the spacing and aligning the user icon properly. 
                I also had to adjust the information layout, as I couldn't place the labels on opposite sides, 
                but the new layout turned out better.
*/

using System;
using Microsoft.Maui.Controls;

namespace RecoveryAT
{
    public partial class UserProfile : ContentPage
    {
        private User _user; // reference to the user

        public UserProfile()
        {
            InitializeComponent(); // initialize the XAML components
            _user = ((App)Application.Current).User; // access the user from App
        }

        // Event handler for when the "Edit" button or icon is tapped
        private async void OnEditTapped(object sender, EventArgs e)
        {
            // Navigate to the UserProfileEdit screen
            await Navigation.PushAsync(new UserProfileEdit());
        }

        // Event handler for when the "Logout" button is clicked
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirmLogout = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");
            if (confirmLogout)
            {
                _user.Logout(); // log the user out
                await Navigation.PushAsync(new UserLogin()); // navigate back to the login screen
            }
        }

        private async void OnDeleteAccountClicked(object sender, EventArgs e)
        {
            string email = _user.Email; // Get the email of the logged-in user

            if (string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Error", "Could not retrieve your email for account deletion.", "OK");
                return;
            }

            bool confirmDelete = await DisplayAlert("Delete Account", "Are you sure you want to delete your account? This action cannot be undone.", "Yes", "No");
            if (confirmDelete)
            {
                var result = MauiProgram.BusinessLogic.DeleteUserAccount(email);

                if (result)
                {
                    await DisplayAlert("Account Deleted", "Your account has been successfully deleted.", "OK");
                    await Navigation.PushAsync(new UserLogin()); // Navigate to the login page
                }
                else
                {
                    await DisplayAlert("Error", "An error occurred while deleting your account. Please try again later.", "OK");
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string email = _user.Email; // Get the logged-in user's email

            if (string.IsNullOrWhiteSpace(email))
            {
                DisplayAlert("Error", "Could not retrieve user data. Please log in again.", "OK");
                return;
            }

            // Fetch user data from the database via BusinessLogic
            User user = MauiProgram.BusinessLogic.GetUserFromEmail(email);

            if (_user != null)
            {
                // Update UI elements directly
                NameLabel.Text = _user.FullName;
                SchoolNameLabel.Text = EncryptionHelper.Decrypt(_user.SchoolName, _user.Key, _user.IV);
                SchoolCodeLabel.Text = _user.SchoolCode;
                EmailLabel.Text = _user.Email;
            }
            else
            {
                DisplayAlert("Error", "User information could not be loaded.", "OK");
            }
        }
    }
}
