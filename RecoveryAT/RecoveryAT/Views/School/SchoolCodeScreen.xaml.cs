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

        // Event handler for text changed in code entry fields
        private void OnCodeEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (entry.Text.Length == 1)
            {
                switch (entry)
                {
                    case var _ when entry == CodeEntry1:
                        CodeEntry2.Focus();
                        break;
                    case var _ when entry == CodeEntry2:
                        CodeEntry3.Focus();
                        break;
                    case var _ when entry == CodeEntry3:
                        CodeEntry4.Focus();
                        break;
                    case var _ when entry == CodeEntry4:
                        CodeEntry5.Focus();
                        break;
                }
            }
            else if (entry.Text.Length == 0)
            {
                switch (entry)
                {
                    case var _ when entry == CodeEntry2:
                        CodeEntry1.Focus();
                        break;
                    case var _ when entry == CodeEntry3:
                        CodeEntry2.Focus();
                        break;
                    case var _ when entry == CodeEntry4:
                        CodeEntry3.Focus();
                        break;
                    case var _ when entry == CodeEntry5:
                        CodeEntry4.Focus();
                        break;
                }
            }
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
