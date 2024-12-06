/**
    Date: 12/06/24
    Description: Displays a trainer's home screen, allowing date selection and viewing athlete forms.
    Bugs: None reported.
    Reflection: Straightforward implementation with effective use of bindings and event handlers.
**/

using CalendarManagment;

namespace RecoveryAT
{
    public partial class TrainerHomeScreen : ContentPage
    {
        public TrainerHomeScreenViewModel ViewModel; // ViewModel for binding data and handling logic for the trainer home screen.
        public User user; // Represents the current user logged into the application.
        private string schoolCode; // School code associated with the trainer for filtering data.
        private IBusinessLogic _businessLogic; 
        public TrainerHomeScreen()
        {
            InitializeComponent(); 
            user = ((App)Application.Current).User; // Retrieve the current user from the application instance.
            schoolCode = user.SchoolCode; // Get the school code for the logged-in user.
            _businessLogic = MauiProgram.BusinessLogic; 

            ViewModel = new TrainerHomeScreenViewModel(_businessLogic, schoolCode);
            BindingContext = ViewModel;

            // Initialize the DatePicker with today's date.
            DatePicker.Date = DateTime.Today;
            ViewModel.Calendar.SelectedDate = new Date(DatePicker.Date); // Update the calendar's selected date.
        }

        // Event handler for date selection changes in the DatePicker.
        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            // Update the ViewModel's selected date.
            ViewModel.Calendar.SelectedDate = new Date(e.NewDate);

            // Load athlete forms for the newly selected date.
            ViewModel.LoadAthleteFormsForDay(e.NewDate);
        }

        // Event handler for tapping on a frame containing athlete form information.
        public async void OnFrameTapped(object sender, EventArgs e)
        {
            // Retrieve the tapped frame and its associated athlete form data.
            Frame athleteFormFrame = (Frame)sender;
            AthleteForm selectedAthleteForm = (AthleteForm)athleteFormFrame.BindingContext;

            // Navigate to the AthleteFormInformation page to display details of the selected form.
            await Navigation.PushAsync(new AthleteFormInformation(selectedAthleteForm));
        }
    }
}
