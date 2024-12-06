/**
    Date: 12/05/24
    Description: This is a profile editing screen where users can update their personal details, 
                 such as first name, last name, school name, school code, and email. 
                 The data is retrieved and bound to the current logged-in user's profile.
    Bugs: None that we know of.
    Reflection: This screen was easy to implement.
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
                DisplayAlert("Error", "Could not retrieve user data. Please log in again.", "OK");
                return;
            }

            // Fetch user data from the database via BusinessLogic.
            var userData = ((App)Application.Current).BusinessLogic.GetUserByEmail(email);

            if (userData != null)
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
                DisplayAlert("Error", "User data could not be loaded.", "OK");
            }
        }

        // Event handler for cancel button click.
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Navigate back to the UserProfile screen.
            await Navigation.PopAsync();
        }

        // Event handler for save button click.
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Collect updated user profile data.
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string schoolName = SchoolNameEntry.Text;
            string schoolCode = SchoolCodeEntry.Text;
            string email = EmailEntry.Text;

            // Update user profile in the database via BusinessLogic.
            bool isUpdated = ((App)Application.Current).BusinessLogic.UpdateUserProfile(
                _user.Email, // Use the original email as identifier.
                firstName,
                lastName,
                schoolName,
                schoolCode,
                email
            );

            if (isUpdated)
            {
                // Show success message and navigate back to the UserProfile page.
                await DisplayAlert("Success", "Profile updated successfully.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                // Show error message if update fails.
                await DisplayAlert("Error", "Failed to update profile. Please try again later.", "OK");
            }
        }
    }
}
