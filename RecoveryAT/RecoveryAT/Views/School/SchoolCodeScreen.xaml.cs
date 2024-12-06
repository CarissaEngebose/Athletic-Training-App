/**
    Date: 12/06/24
    Description: A screen that allows a user to enter a 5-character school code corresponding to the athletic trainer 
                 for a specific school. This ensures that only the correct trainer can access the forms for their school.
    Bugs: None that we know of.
    Reflection: This screen was relatively easy to implement.
**/

namespace RecoveryAT
{
    public partial class SchoolCodeScreen : ContentPage
    {
        // Constructor initializes the components defined in the XAML file.
        public SchoolCodeScreen() 
        {
            InitializeComponent();
        }

        // Handles text input changes in code entry fields and manages focus navigation.
        private void OnCodeEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (entry.Text.Length == 1) // If the current entry is filled, move to the next field.
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
            else if (entry.Text.Length == 0) // If the current entry is cleared, move back to the previous field.
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

        // Handles the submit button click event to validate and process the school code.
        private async void OnSubmitCodeClicked(object sender, EventArgs e)
        {
            // Combine the text from all code entry fields into individual parts.
            string codePart1 = CodeEntry1.Text;
            string codePart2 = CodeEntry2.Text;
            string codePart3 = CodeEntry3.Text;
            string codePart4 = CodeEntry4.Text;
            string codePart5 = CodeEntry5.Text;

            // Validate the entered school code using business logic.
            string validationMessage = IsValidSchoolCode(codePart1, codePart2, codePart3, codePart4, codePart5);

            if (validationMessage == "Code is valid.") 
            {
                // If valid, concatenate the parts to form the complete school code.
                string schoolCode = ConcatSchoolCode(codePart1, codePart2, codePart3, codePart4, codePart5);

                // Navigate to the InjuryFormReport page with the validated school code.
                await Navigation.PushAsync(new InjuryFormReport(schoolCode));
            }
            else 
            {
                // If invalid, display an error message.
                await DisplayAlert("Error", validationMessage, "OK");
            }
        }

        // Calls the business logic to validate the 5-character school code.
        private string IsValidSchoolCode(string part1, string part2, string part3, string part4, string part5)
        {
            return MauiProgram.BusinessLogic.IsValidSchoolCode(part1, part2, part3, part4, part5);
        }

        // Concatenates the five parts of the school code into a single string.
        private string ConcatSchoolCode(string part1, string part2, string part3, string part4, string part5) 
        {
            return string.Concat(part1, part2, part3, part4, part5);
        }
    }
}
