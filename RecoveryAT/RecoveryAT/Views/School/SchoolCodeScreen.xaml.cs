/*
    Name: Carissa Engebose
    Date: 10/9/2024
    Description: A screen that allows a user to enter a 5-character school code that corresponds to the athletic trainer
                for a specific school. Only that trainer will be able to access the forms for their school.
    Bugs: None that I know of.
    Reflection: This screen was relatively easy to implement. It took a bit of time to fine-tune the layout, but overall, 
                I think it turned out well.
*/

using System;
using Microsoft.Maui.Controls;

namespace RecoveryAT
{
    public partial class SchoolCodeScreen : ContentPage
    {
        public SchoolCodeScreen() {
            InitializeComponent(); // initialize the XAML components
        }

        // Event handler for the Submit button click
        private async void OnSubmitCodeClicked(object sender, EventArgs e)
        {
            // combine the text for the 5 entries
            string schoolCode = $"{CodeEntry1.Text}{CodeEntry2.Text}{CodeEntry3.Text}{CodeEntry4.Text}{CodeEntry5.Text}";

            if (IsValidCode(schoolCode)) { 
                await Navigation.PushAsync(new InjuryFormReport()); // navigate to the injury form page
            }
            else {
                await DisplayAlert("Error", "Please enter a valid 5-character code.", "OK");
            }
        }

        // Method to validate the school code
        private bool IsValidCode(string code)
        {
            // Check if the code is exactly 5 characters long and not null or empty
            return !string.IsNullOrEmpty(code) && code.Length == 5;
        }
    }
}
