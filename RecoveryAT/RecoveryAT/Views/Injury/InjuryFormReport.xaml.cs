/*
    Name: Carissa Engebose
    Date: 10/9/2024
    Description: A screen that allows a user to enter information related to injuries that will 
                be sent back to their athletic trainer.
    Bugs: None Known
    Reflection: This screen took a little while to get the layout exactly how I wanted it, 
                but overall, I think it's good.
*/

using Microsoft.Maui.Controls; // Import necessary MAUI controls
using System; // Import system-level utilities

namespace RecoveryAT
{
    // The InjuryFormReport class inherits from ContentPage to represent a page in the MAUI app
    public partial class InjuryFormReport : ContentPage
    {
        private string _schoolCode;

        private bool _isEvalSelected;

        // Constructor to initialize the InjuryFormReport page
        public InjuryFormReport(string schoolCode)
        {
            InitializeComponent(); // Load the XAML components
            BindingContext = this; // Set the BindingContext for data binding with the UI
            _schoolCode = schoolCode; // school code to insert into the database
            _isEvalSelected = false;
        }

        // Event handler for the Submit button click event
        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            // Gets the entries to be inserted into the database
            var firstName = FirstNameEntry.Text;
            var lastName = LastNameEntry.Text;
            var grade = GradePicker.SelectedIndex >= 0 ? int.Parse((string)GradePicker.SelectedItem) : (int?)null;
            var sport = SportPicker.SelectedItem as string;
            var injuredArea = InjuredAreaEntry.Text;
            var injuredSide = InjuredSide.SelectedItem as string;
            var treatmentType = TreatmentType.SelectedItem as string;
            var athleteComments = CommentsEditor.Text;

            // Call AddForm and store the response message
            string resultMessage = MauiProgram.BusinessLogic.AddForm(
                _schoolCode,
                firstName,
                lastName,
                grade,
                sport,
                injuredArea,
                injuredSide,
                treatmentType,
                athleteComments,
                null,  // trainerComments is null initially
                null,  // status is null initially
                DateTime.Now
            );

            if (treatmentType == "Eval")
            {
                _isEvalSelected = true;
            }

            // Display result message to the user
            await DisplayAlert("Form Submission", resultMessage, "OK");

            if (resultMessage.Contains("successfully"))
            {
                FormSubmissionIsSuccessful();
            }
        }

        // if the result message is successful, clear all of the form entries
        private async void FormSubmissionIsSuccessful()
        {
            // Clear the form if submission was successful
            FirstNameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            GradePicker.SelectedIndex = -1;
            SportPicker.SelectedIndex = -1;
            InjuredAreaEntry.Text = string.Empty;
            InjuredSide.SelectedIndex = -1;
            TreatmentType.SelectedIndex = -1;
            CommentsEditor.Text = string.Empty;

            if (_isEvalSelected)
            {
                await Navigation.PushAsync(new AthleteContacts()); // navigate to the athlete contacts
            } else {
                await Navigation.PushAsync(new WelcomeScreen()); // navigate to the welcome screen
            }
        }
    }
}
