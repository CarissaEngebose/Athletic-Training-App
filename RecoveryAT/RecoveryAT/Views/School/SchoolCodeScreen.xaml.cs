/*
    Name: Carissa Engebose
    Date: 10/27/2024
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
        public SchoolCodeScreen() 
        {
            InitializeComponent(); // Initialize the XAML components
        }

        // Event handler for the Submit button click
        private async void OnSubmitCodeClicked(object sender, EventArgs e)
        {
            // Combine the text for the 5 entries
            string codePart1 = CodeEntry1.Text;
            string codePart2 = CodeEntry2.Text;
            string codePart3 = CodeEntry3.Text;
            string codePart4 = CodeEntry4.Text;
            string codePart5 = CodeEntry5.Text;

            // Call the business logic to validate the school code
            string validationMessage = IsValidSchoolCode(codePart1, codePart2, codePart3, codePart4, codePart5);

            if (validationMessage == "Code is valid.") 
            {
                string schoolCode = ConcatSchoolCode(codePart1, codePart2, codePart3, codePart4, codePart5);
                await Navigation.PushAsync(new InjuryFormReport(schoolCode)); // Navigate to the injury form page
            }
            else 
            {
                await DisplayAlert("Error", validationMessage, "OK");
            }
        }

        // Method to call business logic for validating the school code
        private string IsValidSchoolCode(string part1, string part2, string part3, string part4, string part5)
        {
            return MauiProgram.BusinessLogic.IsValidSchoolCode(part1, part2, part3, part4, part5);
        }

        private string ConcatSchoolCode(string part1, string part2, string part3, string part4, string part5) {
            return string.Concat(part1, part2, part3, part4, part5);
        }
    }
}
