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
        public WelcomeScreen()
        {
            InitializeComponent();
        }

        // Navigate to the school code screen when "Fill Out Form" is clicked
        private async void OnFillOutFormClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SchoolCodeScreen());
        }

        // Navigate to the login screen when "Login" is tapped
        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserLogin());
        }
    }
}
