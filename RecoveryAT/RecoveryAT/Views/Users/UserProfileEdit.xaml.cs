using System;
using Microsoft.Maui.Controls;

namespace RecoveryAT
{
    public partial class UserProfileEdit : ContentPage
    {
        private AuthenticationService authService;

        public UserProfileEdit()
        {
            InitializeComponent();
            authService = ((App)Application.Current).AuthService;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            string email = authService.GetLoggedInUserEmail(); // Get the logged-in user's email

            if (string.IsNullOrWhiteSpace(email))
            {
                DisplayAlert("Error", "Could not retrieve user data. Please log in again.", "OK");
                return;
            }

            // Fetch user data from the database via BusinessLogic
            var userData = ((App)Application.Current).BusinessLogic.GetUserByEmail(email);

            if (userData != null)
            {
                FirstNameEntry.Text = userData["FirstName"];
                LastNameEntry.Text = userData["LastName"];
                SchoolNameEntry.Text = userData["SchoolName"];
                SchoolCodeEntry.Text = userData["SchoolCode"];
                EmailEntry.Text = userData["Email"];
            }
            else
            {
                DisplayAlert("Error", "User data could not be loaded.", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Navigate back to the UserProfile screen
            await Navigation.PopModalAsync();
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
                authService.GetLoggedInUserEmail(),
                firstName,
                lastName,
                schoolName,
                schoolCode,
                email
            );

            if (isUpdated)
            {
                await DisplayAlert("Success", "Profile updated successfully.", "OK");
                await Navigation.PopModalAsync(); // Navigate back to the UserProfile page
            }
            else
            {
                await DisplayAlert("Error", "Failed to update profile. Please try again later.", "OK");
            }
        }
    }
}
