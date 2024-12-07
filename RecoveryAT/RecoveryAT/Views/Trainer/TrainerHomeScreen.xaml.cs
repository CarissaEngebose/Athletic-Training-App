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
        public ObservableCollection<AthleteForm> AthleteForms { get; set; } // Collection for binding athlete forms
        public User User { get; set; } // Current logged-in user
        private string SchoolCode { get; set; } // School code associated with the trainer
        private IBusinessLogic _businessLogic; // Business logic interface

        public TrainerHomeScreen()
        {
            InitializeComponent();

            // Initialize properties
            User = ((App)Application.Current).User;
            SchoolCode = User.SchoolCode;
            _businessLogic = MauiProgram.BusinessLogic;
            AthleteForms = new ObservableCollection<AthleteForm>();

            // Bind data
            BindingContext = this;

            // Initialize DatePicker
            DatePicker.Date = DateTime.Today;

            // Load forms for today's date
            LoadAthleteFormsForDay(DateTime.Today);
        }

        // Event handler for date selection changes
        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            LoadAthleteFormsForDay(e.NewDate);
        }

        // Load athlete forms for the selected date
        private void LoadAthleteFormsForDay(DateTime date)
        {
            AthleteForms.Clear();
            var forms = _businessLogic.GetFormsByDateCreated(SchoolCode, date);

            foreach (var form in forms)
            {
                AthleteForms.Add(form);
            }
        }

        // Event handler for navigating to athlete form details
        public async void OnFrameTapped(object sender, EventArgs e)
        {
            Frame athleteFormFrame = (Frame)sender;
            AthleteForm selectedAthleteForm = (AthleteForm)athleteFormFrame.BindingContext;

            await Navigation.PushAsync(new AthleteFormInformation(selectedAthleteForm));
        }
    }
}
