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

        /// <summary>
        /// Initializes the UserProfile screen and sets up data binding.
        /// </summary>
        public UserProfile()
        {
            InitializeComponent(); // Load XAML components
            _user = ((App)Application.Current).User; // Retrieve the logged-in user from the app instance
            BindingContext = _user; // Set the binding context for data binding
        }

        /// <summary>
        /// Method triggered when the page appears. Ensures data is refreshed.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Refresh user data after potential edits
            _user = ((App)Application.Current).User;

            // Handle missing user data and prompt re-login if necessary
            if (_user == null || string.IsNullOrWhiteSpace(_user.Email))
            {
                _ = DisplayAlert("Error", "Could not retrieve user data. Please log in again.", "OK");
                return;
            }

            // Update UI elements with user details
            NameLabel.Text = _user.FullName;
            SchoolNameLabel.Text = EncryptionHelper.Decrypt(_user.SchoolName, _user.Key, _user.IV);
            SchoolCodeLabel.Text = _user.SchoolCode;
            EmailLabel.Text = _user.Email;
        }

        /// <summary>
        /// Event handler for navigating to the UserProfileEdit screen for editing profile information.
        /// </summary>
        private async void OnEditTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserProfileEdit());
        }

        /// <summary>
        /// Event handler to display the information popup.
        /// </summary>
        private void OnInfoTapped(object sender, EventArgs e)
        {
            InfoPopup.IsVisible = true;
            DisplayPopupText();
        }

        /// <summary>
        /// Event handler to close the information popup.
        /// </summary>
        private void OnClosePopupClicked(object sender, EventArgs e)
        {
            InfoPopup.IsVisible = false;
        }

        /// <summary>
        /// Displays a formatted list of instructions or details in the information popup.
        /// </summary>
        private void DisplayPopupText()
        {
            var formattedString = new FormattedString();

            // List of instructions to display
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

            // Format each item with bolded titles and descriptions
            foreach (var item in items)
            {
                formattedString.Spans.Add(new Span
                {
                    Text = $"• {item.Title}: ",
                    FontAttributes = FontAttributes.Bold
                });
                formattedString.Spans.Add(new Span
                {
                    Text = $"{item.Description}\n",
                    FontAttributes = FontAttributes.None
                });
            }

            // Set the formatted text in the popup label
            var infoLabel = InfoPopup.FindByName<Label>("PopupTextLabel");
            if (infoLabel != null)
            {
                infoLabel.FormattedText = formattedString;
            }
        }

        /// <summary>
        /// Event handler to log out the user and navigate to the login screen.
        /// </summary>
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirmLogout = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");
            if (confirmLogout)
            {
                _user.Logout();
                Application.Current.MainPage = new NavigationPage(new WelcomeScreen());
            }
        }

        /// <summary>
        /// Event handler to delete the user's account after confirmation.
        /// </summary>
        private async void OnDeleteAccountClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_user.Email))
            {
                await DisplayAlert("Error", "Could not retrieve your email for account deletion.", "OK");
                return;
            }

            bool confirmDelete = await DisplayAlert("Delete Account", "Are you sure you want to delete your account? This action cannot be undone.", "Yes", "No");
            if (confirmDelete)
            {
                bool result = MauiProgram.BusinessLogic.DeleteUserAccount(_user.Email, _user.SchoolCode);

                if (result)
                {
                    await DisplayAlert("Account Deleted", "Your account has been successfully deleted.", "OK");
                    // set all the user values to null
                    _user.Logout();
                    Application.Current.MainPage = new NavigationPage(new WelcomeScreen());
                }
                else
                {
                    await DisplayAlert("Error", "An error occurred while deleting your account. Please try again later.", "OK");
                }
            }
        }
    }
}
