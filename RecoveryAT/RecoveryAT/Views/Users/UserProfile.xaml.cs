/*
    Date: 12/06/24
    Description: A screen for the athletic trainer to view and edit profile information.
    Bugs: None that I know of.
    Reflection: This part of the UserProfile screen was easy to implement.
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
            BindingContext = _user; // Set the binding context for data binding
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_user == null || string.IsNullOrWhiteSpace(_user.Email))
            {
                DisplayAlert("Error", "Could not retrieve user data. Please log in again.", "OK");
                return;
            }

            // Update additional fields manually if needed
            NameLabel.Text = _user.FullName;
            SchoolNameLabel.Text = EncryptionHelper.Decrypt(_user.SchoolName, _user.Key, _user.IV);
            SchoolCodeLabel.Text = _user.SchoolCode;
            EmailLabel.Text = _user.Email;
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
                _user.Logout(); // Log the user out

                // Clear the navigation stack and set the login page as the root
                Application.Current.MainPage = new NavigationPage(new UserLogin());
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
    }
}
