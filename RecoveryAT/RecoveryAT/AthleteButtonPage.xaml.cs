/*
    Date: 12/06/2024
    Description: This page creates a tabbed interface for navigating various athlete-related 
                 functionalities in the RecoveryAT application. The tabs include athlete information, 
                 past forms, injury statistics, and statuses, with SchoolCode dynamically loaded 
                 based on the user's profile.
    Bugs: None Known
    Reflection: These tabs were a little hard to implement because the AthleteStatuses page takes
                in the SchoolCode, so the tabs needed to be added to as children in the cs file.
*/

using Microsoft.Maui.Controls;

namespace RecoveryAT
{
    public partial class AthleteButtonPage : ContentPage
    {
        private string SchoolCode { get; set; }

        public AthleteButtonPage()
        {
            InitializeComponent();
            InitializePageAsync();
        }

        private async Task InitializePageAsync()
        {
            var user = ((App)Application.Current).User; // Access the current user
            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                await DisplayAlert("Error", "No user is logged in. Please log in to continue.", "OK");
                return;
            }

            SchoolCode = user.SchoolCode ?? "DefaultCode"; // Retrieve SchoolCode or use default
        }

        private async void OnAthleteInfoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AthleteInformation());
        }

        private async void OnAthleteFormsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AthletePastForms());
        }

        private async void OnInjuryStatisticsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InjuryStatistics());
        }

        private async void OnAthleteStatusesClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SchoolCode))
            {
                await DisplayAlert("Error", "School code not available. Please log in again.", "OK");
                return;
            }

            await Navigation.PushAsync(new AthleteStatuses(SchoolCode));
        }
    }
}
