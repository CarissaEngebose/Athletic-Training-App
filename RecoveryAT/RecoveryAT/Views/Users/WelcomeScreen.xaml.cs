/* 
    Date: 12/06/24
    Description: A welcome screen that will navigate to the school code screen 
                 (to fill out a form) or login depending on if the user
                 is an athlete or an athletic trainer.
    Bugs: None that I know of.
    Reflection: This was easy to implement because there isn't much on the screen
                and they are all one above the next.
*/

namespace RecoveryAT
{
    public partial class WelcomeScreen : ContentPage
    {
        /// <summary>
        /// Initializes the welcome screen components.
        /// </summary>
        public WelcomeScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the "Fill Out Form" button click event.
        /// Navigates the user to the school code screen to start filling out a form.
        /// </summary>
        private async void OnFillOutFormClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new SchoolCodeScreen());
            }
            catch (Exception ex)
            {
                // Handle navigation error
                await DisplayAlert("Error", $"Failed to navigate to the form: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Handles the "Login" text tap event.
        /// Navigates the user to the login screen.
        /// </summary>
        private async void OnLoginTapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new UserLogin());
            }
            catch (Exception ex)
            {
                // Handle navigation error
                await DisplayAlert("Error", $"Failed to navigate to the login screen: {ex.Message}", "OK");
            }
        }
    }
}
