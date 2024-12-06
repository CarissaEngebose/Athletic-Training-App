/* 
    Date: 12/06/24
    Description: A welcome screen allowing navigation to the school code screen for athletes 
                 or the login screen for athletic trainers.
    Bugs: None reported.
    Reflection: Simple implementation due to minimal functionality.
*/

using System;
using Microsoft.Maui.Controls;

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
