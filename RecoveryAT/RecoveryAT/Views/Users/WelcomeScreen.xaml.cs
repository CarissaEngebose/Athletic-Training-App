/* 
    Name: Carissa Engebose
    Date: 10/9/2024
    Description: A welcome screen that will navigate to the school code screen (to fill out a form) or login depending on if the user
    is an athlete or an athletic trainer.
    Bugs: None that I know of.
    Reflection: This screen was the first one I created so it did take a little bit of work to figure out how I wanted everything
    laid out. It also took me a while to figure out how to space things correcly with the circles on the top of the screen, but overall,
    I think it went pretty well.
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

        private async void OnFillOutFormClicked(object sender, EventArgs e)
        {
            // navigate to the school code screen 
            await Navigation.PushAsync(new SchoolCodeScreen());
        }

        private async void OnLoginTapped(object sender, EventArgs e)
        {
            // navigate to the login page
            await Navigation.PushAsync(new UserLogin());
        }
    }
}