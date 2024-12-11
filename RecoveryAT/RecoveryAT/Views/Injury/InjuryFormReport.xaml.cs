/*
    Date: 12/06/24
    Description: A screen that allows a user to enter in information relating to injuries that will go back to their athletic trainer.
    Bugs: None Known
    Reflection: This screen took a little while to get the layout exactly how we wanted it but overall we think it's good.
*/

namespace RecoveryAT
{
    public partial class InjuryFormReport : ContentPage
    {
        private string _schoolCode; // The school code used to associate the form with a specific school.
        private bool _isEvalSelected; // Tracks if the treatment type "Eval" is selected.
        private User _user; // Represents the current logged-in user.

        /// <summary>
        /// Constructor for athletes submitting a form, with the SchoolCode provided.
        /// </summary>
        /// <param name="schoolCode">The school code associated with the athlete.</param>
        public InjuryFormReport(string schoolCode)
        {
            InitializeComponent();
            BindingContext = this;
            _schoolCode = schoolCode; // Set the provided school code.
            _isEvalSelected = false; // Initialize Eval selection flag.
        }

        /// <summary>
        /// Constructor for trainers submitting a form.
        /// Retrieves the SchoolCode from the logged-in user's profile.
        /// </summary>
        public InjuryFormReport()
        {
            InitializeComponent();
            BindingContext = this;
            _user = ((App)Application.Current).User;

            // Retrieve SchoolCode from the user's profile or set a default value.
            _schoolCode = _user?.SchoolCode ?? "DefaultCode";
            _isEvalSelected = false; // Initialize Eval selection flag.
        }

        /// <summary>
        /// Handles the form submission, validates inputs, and stores the data in the database.
        /// </summary>
        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            // Retrieve inputs from the form fields.
            var firstName = FirstNameEntry.Text;
            var lastName = LastNameEntry.Text;
            var sport = SportPicker.SelectedItem as string;
            var injuredArea = InjuredAreaEntry.Text;
            var injuredSide = InjuredSide.SelectedItem as string;
            var treatmentType = TreatmentType.SelectedItem as string;
            var athleteComments = CommentsEditor.Text;
            var dateOfBirth = DateOfBirthPicker.Date;

            // Validate required fields.
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

            // Add the form to the database using the BusinessLogic layer.
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
                null, // Status is null initially.
                DateTime.Now // Current date and time.
            );

            // Check if "Eval" treatment type is selected.
            if (treatmentType == "Eval")
            {
                _isEvalSelected = true;
            }

            // Display a confirmation or error message.
            await DisplayAlert("Form Submission", resultMessage, "OK");

            if (resultMessage.Contains("successfully", StringComparison.OrdinalIgnoreCase))
            {
                await FormSubmissionIsSuccessful(); // Handle successful submission.
            }
        }

        /// <summary>
        /// Handles the color change for Picker control based on selection.
        /// </summary>
        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            if (picker.SelectedIndex == -1) // No selection made.
            {
                picker.TextColor = Color.FromArgb("#D3D3D3"); // Light gray.
            }
            else // Selection made.
            {
                picker.TextColor = Color.FromArgb("#000000"); // Black.
            }
        }

        /// <summary>
        /// Downloads form details as a text file and opens it for the user.
        /// </summary>
        private async void OnDownloadTextClicked(object sender, EventArgs e)
        {
            try
            {
                // Define the file name and path.
                var fileName = "InjuryFormDetails.txt";
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                // Prepare the form details.
                var formDetails = $@"Here are your injury form responses:
- First Name: {FirstNameEntry?.Text ?? "N/A"}
- Last Name: {LastNameEntry?.Text ?? "N/A"}
- Date of Birth: {DateOfBirthPicker?.Date.ToString("MM/dd/yyyy") ?? "N/A"}
- Sport: {SportPicker?.SelectedItem as string ?? "N/A"}
- Injured Area: {InjuredAreaEntry?.Text ?? "N/A"}
- Injured Side: {InjuredSide?.SelectedItem as string ?? "N/A"}
- Treatment Type: {TreatmentType?.SelectedItem as string ?? "N/A"}
- Comments: {CommentsEditor?.Text ?? "N/A"}";

                // Save the text file.
                File.WriteAllText(filePath, formDetails);

                // Open the saved file.
                _ = await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save or open text file: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Handles successful form submission by clearing inputs and navigating appropriately.
        /// </summary>
        private async Task FormSubmissionIsSuccessful()
        {
            try
            {
                // Retrieve the last inserted formKey.
                long formKey = MauiProgram.BusinessLogic.GetLastInsertedFormKey(_schoolCode);

                // Clear all form inputs.
                FirstNameEntry.Text = string.Empty;
                LastNameEntry.Text = string.Empty;
                SportPicker.SelectedIndex = -1;
                InjuredAreaEntry.Text = string.Empty;
                InjuredSide.SelectedIndex = -1;
                TreatmentType.SelectedIndex = -1;
                CommentsEditor.Text = string.Empty;

                // Navigate based on whether "Eval" was selected.
                if (_isEvalSelected)
                {
                    await Navigation.PushAsync(new AthleteContacts(formKey));
                }
                else
                {
                    if (_user == null)
                    {
                        await Navigation.PushAsync(new WelcomeScreen()); // Navigate to Welcome screen.
                    }
                    else
                    {
                        Application.Current.MainPage = new MainTabbedPage(); // Navigate to MainTabbedPage.
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while submitting the form: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Handles the selection of a date of birth in the DatePicker control.
        /// </summary>
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

        /// <summary>
        /// Resets placeholder visibility and appearance when the page is displayed.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            PlaceholderLabel.IsVisible = true;
            DateOfBirthPicker.TextColor = Colors.Transparent;
        }
    }
}
