/*
    Name: Carissa Engebose
    Date: 10/9/2024
    Description: A screen for the athletic trainer to view and edit profile information.
    Bugs: None that I know of.
    Reflection: This screen was a little difficult because of all the spacing and aligning the user icon properly. 
                I also had to adjust the information layout, as I couldn't place the labels on opposite sides, 
                but the new layout turned out better.
*/

using System;
using Microsoft.Maui.Controls;

namespace RecoveryAT
{
    public partial class UserProfile : ContentPage
    {
        public UserProfile()
        {
            InitializeComponent(); // Initialize the XAML components
        }

        // Event handler for when the "Edit" button or icon is tapped
        private void OnEditTapped(object sender, EventArgs e)
        {
            // TODO: Add navigation to an edit profile screen or enable inline editing
        }

        // Event handler for when the "Logout" button is clicked
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // TODO: Implement actual logout logic (e.g., clear user session or navigate to login page)
            bool confirmLogout = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");
            if (confirmLogout)
            {
                // Navigate to the login page after logout
                await Navigation.PushAsync(new UserLogin());
            }
        }
    }
}
