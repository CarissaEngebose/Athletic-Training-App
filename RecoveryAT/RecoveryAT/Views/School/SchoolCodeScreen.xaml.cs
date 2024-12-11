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
        /// <summary>
        /// Constructor initializes the components defined in the XAML file.
        /// </summary>
        public SchoolCodeScreen() 
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles text input changes in the code entry fields. Automatically moves focus 
        /// to the next or previous field based on input length.
        /// </summary>
        private void OnCodeEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            
            if (entry?.Text?.Length == 1) // If the current entry is filled, move to the next field.
            {
                switch (entry)
                {
                    case var _ when entry == CodeEntry1:
                        _ = CodeEntry2.Focus();
                        break;
                    case var _ when entry == CodeEntry2:
                        _ = CodeEntry3.Focus();
                        break;
                    case var _ when entry == CodeEntry3:
                        _ = CodeEntry4.Focus();
                        break;
                    case var _ when entry == CodeEntry4:
                        _ = CodeEntry5.Focus();
                        break;
                }
            }
            else if (entry?.Text?.Length == 0) // If the current entry is cleared, move back to the previous field.
            {
                switch (entry)
                {
                    case var _ when entry == CodeEntry2:
                        _ = CodeEntry1.Focus();
                        break;
                    case var _ when entry == CodeEntry3:
                        _ = CodeEntry2.Focus();
                        break;
                    case var _ when entry == CodeEntry4:
                        _ = CodeEntry3.Focus();
                        break;
                    case var _ when entry == CodeEntry5:
                        _ = CodeEntry4.Focus();
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the submit button click event. Validates and processes the entered school code.
        /// </summary>
        private async void OnSubmitCodeClicked(object sender, EventArgs e)
        {
            // Combine the text from all code entry fields.
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

        /// <summary>
        /// Validates the entered 5-character school code by calling business logic.
        /// </summary>
        /// <param name="part1">First character of the school code.</param>
        /// <param name="part2">Second character of the school code.</param>
        /// <param name="part3">Third character of the school code.</param>
        /// <param name="part4">Fourth character of the school code.</param>
        /// <param name="part5">Fifth character of the school code.</param>
        /// <returns>A validation message indicating whether the code is valid or not.</returns>
        private static string IsValidSchoolCode(string part1, string part2, string part3, string part4, string part5)
        {
            return MauiProgram.BusinessLogic.IsValidSchoolCode(part1, part2, part3, part4, part5);
        }

        /// <summary>
        /// Concatenates the five parts of the school code into a single string.
        /// </summary>
        /// <param name="part1">First character of the school code.</param>
        /// <param name="part2">Second character of the school code.</param>
        /// <param name="part3">Third character of the school code.</param>
        /// <param name="part4">Fourth character of the school code.</param>
        /// <param name="part5">Fifth character of the school code.</param>
        /// <returns>The full 5-character school code as a single string.</returns>
        private static string ConcatSchoolCode(string part1, string part2, string part3, string part4, string part5)
        {
            return string.Concat(part1, part2, part3, part4, part5);
        }
    }
}
