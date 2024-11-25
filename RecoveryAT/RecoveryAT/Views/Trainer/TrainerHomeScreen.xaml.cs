using System;
using CalendarManagment;
using Microsoft.Maui.Controls;

namespace RecoveryAT
{
    public partial class TrainerHomeScreen : ContentPage
    {
        public TrainerHomeScreenViewModel ViewModel;
        public AuthenticationService authService;
        private string schoolCode;
        private IBusinessLogic _businessLogic;

        public TrainerHomeScreen()
        {
            InitializeComponent();
            authService = ((App)Application.Current).AuthService;
            schoolCode = authService.GetSchoolCode();
            _businessLogic = MauiProgram.BusinessLogic;
            ViewModel = new TrainerHomeScreenViewModel(_businessLogic, schoolCode);
            BindingContext = ViewModel;

            // Initialize the DatePicker with today's date
            DatePicker.Date = DateTime.Today;
            ViewModel.Calendar.SelectedDate = new Date(DatePicker.Date);
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            // Update ViewModel's selected date and load forms
            ViewModel.Calendar.SelectedDate = new Date(e.NewDate);
            ViewModel.LoadAthleteFormsForDay(e.NewDate);
        }

        public async void OnFrameTapped(object sender, EventArgs e)
        {
            Frame athleteFormFrame = (Frame)sender; // get XAML frame
            AthleteForm selectedAthleteForm = (AthleteForm)athleteFormFrame.BindingContext; // get selected athlete form from clicked frame
            await Navigation.PushAsync(new AthleteFormInformation(selectedAthleteForm)); // send to AthleteFormInformation view to display
        }
    }
}
