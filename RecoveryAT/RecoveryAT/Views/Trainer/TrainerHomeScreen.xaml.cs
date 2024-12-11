/**
    Date: 12/06/24
    Description: Displays a trainer's home screen, allowing date selection and viewing athlete forms.
    Bugs: None reported.
    Reflection: Straightforward implementation with effective use of bindings and event handlers.
**/

using System.Collections.ObjectModel;

namespace RecoveryAT
{
    public partial class TrainerHomeScreen : ContentPage
    {
        // Collection of athlete forms for binding to the UI
        public ObservableCollection<AthleteForm> AthleteForms { get; set; } 

        // Represents the currently logged-in user
        public User User { get; set; } 

        // School code associated with the trainer
        private string SchoolCode { get; set; } 

        // Business logic interface for data operations
        private IBusinessLogic _businessLogic; 

        /// <summary>
        /// Constructor initializes the TrainerHomeScreen and loads forms for today's date.
        /// </summary>
        public TrainerHomeScreen()
        {
            InitializeComponent();

            // Retrieve user details and school code
            User = ((App)Application.Current).User;
            SchoolCode = User.SchoolCode;

            // Initialize the business logic interface
            _businessLogic = MauiProgram.BusinessLogic;

            // Initialize the collection for athlete forms
            AthleteForms = new ObservableCollection<AthleteForm>();

            // Set the BindingContext for data binding
            BindingContext = this;

            // Set the DatePicker to the current date
            DatePicker.Date = DateTime.Today;

            // Load athlete forms for today's date
            LoadAthleteFormsForDay(DateTime.Today);
        }

        /// <summary>
        /// Event handler triggered when a new date is selected in the DatePicker.
        /// Updates the athlete forms displayed for the selected date.
        /// </summary>
        /// <param name="sender">The DatePicker control.</param>
        /// <param name="e">Contains the new and old date values.</param>
        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            // Load athlete forms for the newly selected date
            LoadAthleteFormsForDay(e.NewDate);
        }

        /// <summary>
        /// Loads athlete forms for a specific date and updates the ObservableCollection.
        /// </summary>
        /// <param name="date">The date for which to load athlete forms.</param>
        private void LoadAthleteFormsForDay(DateTime date)
        {
            // Clear the current list of forms
            AthleteForms.Clear();

            // Retrieve forms from the business logic layer
            var forms = _businessLogic.GetFormsByDateCreated(SchoolCode, date);

            // Add retrieved forms to the ObservableCollection
            foreach (var form in forms)
            {
                AthleteForms.Add(form);
            }
        }

        /// <summary>
        /// Event handler triggered when a form is tapped. Navigates to the details page for the selected form.
        /// </summary>
        /// <param name="sender">The Frame containing the athlete form.</param>
        /// <param name="e">Event arguments.</param>
        public async void OnFrameTapped(object sender, EventArgs e)
        {
            // Retrieve the Frame and its BindingContext (the selected athlete form)
            Frame athleteFormFrame = (Frame)sender;
            AthleteForm selectedAthleteForm = (AthleteForm)athleteFormFrame.BindingContext;

            // Navigate to the AthleteFormInformation page for the selected form
            await Navigation.PushAsync(new AthleteFormInformation(selectedAthleteForm));
        }
    }
}
