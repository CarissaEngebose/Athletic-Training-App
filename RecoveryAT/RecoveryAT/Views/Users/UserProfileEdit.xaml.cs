using System;
using Microsoft.Maui.Controls;

namespace RecoveryAT
{
    public partial class UserProfileEdit : ContentPage
    {
        private User _user;

        public UserProfileEdit()
        {
            InitializeComponent();
            _user = ((App)Application.Current).User;
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
            var userData = ((App)Application.Current).BusinessLogic.GetUserByEmail(email);

            if (_user != null)
            {
                FirstNameEntry.Text = _user.FirstName;
                LastNameEntry.Text = _user.LastName;
                SchoolNameEntry.Text = _user.SchoolName;
                SchoolCodeEntry.Text = _user.SchoolCode;
                EmailEntry.Text = _user.Email;
            }
            else
            {
                DisplayAlert("Error", "User data could not be loaded.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Navigate back to the UserProfile screen
            await Navigation.PopAsync();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Collect updated data
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string schoolName = SchoolNameEntry.Text;
            string schoolCode = SchoolCodeEntry.Text;
            string email = EmailEntry.Text;

            // Update user data in the database via BusinessLogic
            bool isUpdated = ((App)Application.Current).BusinessLogic.UpdateUserProfile(
                _user.Email,
                firstName,
                lastName,
                schoolName,
                schoolCode,
                email
            );

            if (isUpdated)
            {
                await DisplayAlert("Success", "Profile updated successfully.", "OK");
                await Navigation.PopAsync(); // Navigate back to the UserProfile page
            }
            else
            {
                await DisplayAlert("Error", "Failed to update profile. Please try again later.", "OK");
            }
        }
    }
}
