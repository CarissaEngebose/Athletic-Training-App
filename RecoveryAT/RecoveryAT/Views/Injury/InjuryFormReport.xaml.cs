/*
    Name: Carissa Engebose
    Date: 10/28/2024
    Description: A screen that allows a user to enter information related to injuries that will 
                be sent back to their athletic trainer.
    Bugs: None Known
    Reflection: This screen took a little while to figure out because I kept running into issues with using the grade.Value
                when it was null (which honestly shouldn't have taken me as long as it did to figure out). Other than that,
                I think connecting it with the database went very well.
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
        private AuthenticationService authService;

        // Constructor to initialize the InjuryFormReport page (for athletes)
        public InjuryFormReport(string schoolCode)
        {
            InitializeComponent(); // Load the XAML components
            BindingContext = this; // Set the BindingContext for data binding with the UI
            _schoolCode = schoolCode; // school code to insert into the database
            _isEvalSelected = false;
        }

        // Constructor to initialize the InjuryFormReport page (for trainers)
        public InjuryFormReport()
        {
            InitializeComponent(); // Load the XAML components
            BindingContext = this; // Set the BindingContext for data binding with the UI
            authService = ((App)Application.Current).AuthService;
            _schoolCode = authService.SchoolCode; // school code to insert into the database
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
        // Inside the InjuryFormReport class

        private async void FormSubmissionIsSuccessful()
        {
            // Assuming AddForm returns a formKey after inserting the form into the database
            long formKey = MauiProgram.BusinessLogic.GetLastInsertedFormKey(_schoolCode);  // Fetch the last inserted formKey

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
                await Navigation.PushAsync(new AthleteContacts(formKey)); // Pass formKey to AthleteContacts
            }
            else
            {
                if (authService == null){
                    await Navigation.PushAsync(new WelcomeScreen()); // navigate to the welcome screen
                } else {
                    Application.Current.MainPage = new MainTabbedPage(); // navigate to the home screen
                }
            }
        }

    }
}
