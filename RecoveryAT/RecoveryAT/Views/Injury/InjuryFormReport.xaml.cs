using System.IO;

namespace RecoveryAT
{
    public partial class InjuryFormReport : ContentPage
    {
        private string _schoolCode;
        private bool _isEvalSelected;
        private AuthenticationService _authService;

        // Constructor for athletes with provided SchoolCode
        public InjuryFormReport(string schoolCode)
        {
            InitializeComponent();
            BindingContext = this;
            _schoolCode = schoolCode;
            _isEvalSelected = false;
        }

        // Constructor for trainers
        public InjuryFormReport()
        {
            InitializeComponent();
            BindingContext = this;
            _authService = ((App)Application.Current).AuthService;

            // Retrieve SchoolCode from AuthenticationService
            _schoolCode = _authService?.GetSchoolCode() ?? "DefaultCode";
            _isEvalSelected = false;
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            // Get form inputs
            var firstName = FirstNameEntry.Text;
            var lastName = LastNameEntry.Text;
            var sport = SportPicker.SelectedItem as string;
            var injuredArea = InjuredAreaEntry.Text;
            var injuredSide = InjuredSide.SelectedItem as string;
            var treatmentType = TreatmentType.SelectedItem as string;
            var athleteComments = CommentsEditor.Text;
            var dateOfBirth = DateOfBirthPicker.Date;

            // Validate required fields
            if (string.IsNullOrWhiteSpace(_schoolCode) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(sport) ||
                string.IsNullOrWhiteSpace(injuredArea) ||
                string.IsNullOrWhiteSpace(injuredSide) ||
                string.IsNullOrWhiteSpace(treatmentType))
            {
                await DisplayAlert("Error", "All fields must be filled in, except comments.", "OK");
                return;
            }

            // Call AddForm and store the response message
            string resultMessage = MauiProgram.BusinessLogic.AddForm(
                _schoolCode,
                firstName,
                lastName,
                sport,
                injuredArea,
                injuredSide,
                treatmentType,
                dateOfBirth,
                athleteComments,
                null, // Status is null initially
                DateTime.Now
            );

            if (treatmentType == "Eval")
            {
                _isEvalSelected = true;
            }

            // Display result message to the user
            await DisplayAlert("Form Submission", resultMessage, "OK");

            if (resultMessage.Contains("successfully", StringComparison.OrdinalIgnoreCase))
            {
                await FormSubmissionIsSuccessful();
            }
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            if (picker.SelectedIndex == -1) // No selection made
            {
                picker.TextColor = Color.FromArgb("#D3D3D3"); // Light Gray color
            }
            else // An item is selected
            {
                picker.TextColor = Color.FromArgb("#000000"); // Predefined Black color
            }
        }

        private async void OnDownloadTextClicked(object sender, EventArgs e)
        {
            try
            {
                // Define the file path
                var fileName = "InjuryFormDetails.txt";
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                // Prepare the content
                var formDetails = $@"Here are your injury form responses:
- First Name: {FirstNameEntry?.Text ?? "N/A"}
- Last Name: {LastNameEntry?.Text ?? "N/A"}
- Date of Birth: {DateOfBirthPicker?.Date.ToString("MM/dd/yyyy") ?? "N/A"}
- Sport: {SportPicker?.SelectedItem as string ?? "N/A"}
- Injured Area: {InjuredAreaEntry?.Text ?? "N/A"}
- Injured Side: {InjuredSide?.SelectedItem as string ?? "N/A"}
- Treatment Type: {TreatmentType?.SelectedItem as string ?? "N/A"}
- Comments: {CommentsEditor?.Text ?? "N/A"}";

                // Save the text file
                File.WriteAllText(filePath, formDetails);

                // Automatically open the file
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save or open text file: {ex.Message}", "OK");
            }
        }

        private async Task FormSubmissionIsSuccessful()
        {
            try
            {
                // Fetch the last inserted formKey for the current SchoolCode
                long formKey = MauiProgram.BusinessLogic.GetLastInsertedFormKey(_schoolCode);

                // Clear the form inputs
                FirstNameEntry.Text = string.Empty;
                LastNameEntry.Text = string.Empty;
                SportPicker.SelectedIndex = -1;
                InjuredAreaEntry.Text = string.Empty;
                InjuredSide.SelectedIndex = -1;
                TreatmentType.SelectedIndex = -1;
                CommentsEditor.Text = string.Empty;

                if (_isEvalSelected)
                {
                    await Navigation.PushAsync(new AthleteContacts(formKey)); // Navigate to AthleteContacts
                }
                else
                {
                    if (_authService == null)
                    {
                        await Navigation.PushAsync(new WelcomeScreen()); // Navigate to the Welcome screen
                    }
                    else
                    {
                        Application.Current.MainPage = new MainTabbedPage(); // Navigate to the MainTabbedPage
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while submitting the form: {ex.Message}", "OK");
            }
        }

        private void OnDateOfBirthSelected(object sender, DateChangedEventArgs e)
        {
            if (e.NewDate != DateTime.Today)
            {
                PlaceholderLabel.IsVisible = false;
                DateOfBirthPicker.TextColor = Colors.Black;
            }
            else
            {
                PlaceholderLabel.IsVisible = true;
                DateOfBirthPicker.TextColor = Colors.Transparent;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Reset placeholder visibility and DatePicker appearance
            PlaceholderLabel.IsVisible = true;
            DateOfBirthPicker.TextColor = Colors.Transparent;
        }
    }
}
