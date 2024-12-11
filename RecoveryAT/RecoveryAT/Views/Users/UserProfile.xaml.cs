/*
    Date: 12/06/24
    Description: A screen for the athletic trainer to view and edit profile information.
    Bugs: None that I know of.
    Reflection: This part of the UserProfile screen was easy to implement.
*/

namespace RecoveryAT
{
    public partial class UserProfile : ContentPage
    {
        private User _user; // Reference to the logged-in user.

        public UserProfile()
        {
            InitializeComponent(); // Initialize the XAML components.
            _user = ((App)Application.Current).User; // Access the current user from the App class.
            BindingContext = _user; // Set the binding context for data binding in the XAML.
        }

        // Method triggered when the page appears
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _user = ((App)Application.Current).User; // refresh the user (after UserProfileEdit)

            // Ensure user data is available, otherwise display an error and prompt re-login.
            if (_user == null || string.IsNullOrWhiteSpace(_user.Email))
            {
                _ = DisplayAlert("Error", "Could not retrieve user data. Please log in again.", "OK");
                return;
            }

            // Update the user details displayed on the screen.
            NameLabel.Text = _user.FullName;
            SchoolNameLabel.Text = EncryptionHelper.Decrypt(_user.SchoolName, _user.Key, _user.IV);
            SchoolCodeLabel.Text = _user.SchoolCode;
            EmailLabel.Text = _user.Email;
        }

        // Event handler for when the "Edit" button or icon is tapped.
        private async void OnEditTapped(object sender, EventArgs e)
        {
            // Navigate to the UserProfileEdit screen for editing profile information.
            await Navigation.PushAsync(new UserProfileEdit());
        }

        // Event handler for when the info (i) button is tapped.
        private void OnInfoTapped(object sender, EventArgs e)
        {
            InfoPopup.IsVisible = true; // Show the popup.
            DisplayPopupText(); // Populate the popup with dynamically formatted text.
        }

        // Event handler for when the close button on the popup is clicked.
        private void OnClosePopupClicked(object sender, EventArgs e)
        {
            InfoPopup.IsVisible = false; // Hide the popup.
        }

        // Helper method to display bullet-pointed text in the InfoPopup.
        private void DisplayPopupText()
        {
            // Create a new FormattedString to manage bolded titles and descriptions.
            var formattedString = new FormattedString();

            // Define a collection of items with titles and descriptions.
            var items = new[]
            {
                new { Title = "Add Athlete Contacts", Description = "Add contact info for athletes (phone/relationship). Tap Finish to save or delete entries using the trash icon." },
                new { Title = "View Athlete Form Details", Description = "View athlete details (DOB, sport, injury). Edit with the top-right button." },
                new { Title = "Edit Athlete Form", Description = "Edit athlete info (treatment type, status, contact). Tap Save Changes to update." },
                new { Title = "Athlete Directory", Description = "Search and view athlete profiles. Tap entries to see more details." },
                new { Title = "Past Forms", Description = "View past forms by treatment type. Use search or tap forms for details." },
                new { Title = "Update Athlete Status", Description = "View and update athlete statuses. Use the picker to change statuses." },
                new { Title = "Submit Injury Report", Description = "Submit injury details for review. Fill the form and tap Submit." },
                new { Title = "Injury Statistics", Description = "Select a sport to view injury stats. Stats display in a chart." },
                new { Title = "Enter School Code", Description = "Enter the 5-digit school code. Tap Submit Code to verify." },
                new { Title = "Trainer Dashboard", Description = "Select a date and view athlete forms for that day." },
                new { Title = "Add School Information", Description = "Enter school name and code. Tap Create School to finalize." }
            };

            // Loop through each item and add formatted spans for titles and descriptions.
            foreach (var item in items)
            {
                // Add the bold title.
                formattedString.Spans.Add(new Span
                {
                    Text = $"• {item.Title}: ",
                    FontAttributes = FontAttributes.Bold
                });

                // Add the description.
                formattedString.Spans.Add(new Span
                {
                    Text = $"{item.Description}",
                    FontAttributes = FontAttributes.None
                });

                // Add a smaller span for spacing.
                formattedString.Spans.Add(new Span
                {
                    Text = "\n", // Single newline.
                    FontSize = 5 // Smaller font size for the spacing.
                });
            }

            // Set the formatted text to the Label in the popup.
            var infoLabel = InfoPopup.FindByName<Label>("PopupTextLabel");
            if (infoLabel != null)
            {
                infoLabel.FormattedText = formattedString;
            }
        }

        // Event handler for when the "Logout" button is clicked.
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Prompt user to confirm the logout action.
            bool confirmLogout = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");
            if (confirmLogout)
            {
                _user.Logout(); // Log the user out.

                // Clear the navigation stack and set the login page as the root.
                Application.Current.MainPage = new NavigationPage(new UserLogin());
            }
        }

        // Event handler for when the "Delete Account" button is clicked.
        private async void OnDeleteAccountClicked(object sender, EventArgs e)
        {
            string email = _user.Email; // Get the email of the logged-in user.

            // Validate that the email is available before attempting deletion.
            if (string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Error", "Could not retrieve your email for account deletion.", "OK");
                return;
            }

            // Prompt user to confirm account deletion.
            bool confirmDelete = await DisplayAlert("Delete Account", "Are you sure you want to delete your account? This action cannot be undone.", "Yes", "No");
            if (confirmDelete)
            {
                // Attempt to delete the user account via the business logic layer.
                var result = MauiProgram.BusinessLogic.DeleteUserAccount(email);

                if (result)
                {
                    // Notify user of successful deletion and redirect to login page.
                    await DisplayAlert("Account Deleted", "Your account has been successfully deleted.", "OK");
                    await Navigation.PushAsync(new UserLogin()); // Navigate to the login page.
                }
                else
                {
                    // Notify user of an error during deletion.
                    await DisplayAlert("Error", "An error occurred while deleting your account. Please try again later.", "OK");
                }
            }
        }
    }
}
