/*
    Name: Hannah Hotchkiss
    Date: 10/14/2024
    Description: UserCreateAccount - This page allows users to create an account by providing their email, password, and confirming their password.
    Users can select a status from a dropdown menu and search for athletes using a search bar.
    Bugs: None Known
    Reflection: This screen was a easy as well because there also wasn't too much going on. The hardest part was 
    adding the aesthetic blue overlapping circles to the bottom of the screen.
*/

namespace RecoveryAT
{
    public partial class UserCreateAccount : ContentPage
    {
        public UserCreateAccount()
        {
            InitializeComponent(); // This method is called to initialize the XAML UI components.
        }

        // Event handler when the "Go to Create Account" button is clicked
        private async void OnCreateAccountClicked(object sender, EventArgs e)
        {
            // Navigate to the UserCreateAccount page
            await Navigation.PushAsync(new UserCreateAccount());
        }

        // Event handler when the Create Account button is clicked
        private async void CreateAccountClicked(object sender, EventArgs e)
        {
            // Navigate to the trainer school information page
            await Navigation.PushAsync(new TrainerSchoolInformation());
        }
    }
}
