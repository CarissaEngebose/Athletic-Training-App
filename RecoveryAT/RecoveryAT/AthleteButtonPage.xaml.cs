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

namespace RecoveryAT
{
    public partial class AthleteButtonPage : ContentPage
    {
        /// <summary>
        /// The school code associated with the logged-in user.
        /// </summary>
        private string SchoolCode { get; set; } = string.Empty;

        /// <summary>
        /// Constructor to initialize the page and load the user-specific school code.
        /// </summary>
        public AthleteButtonPage()
        {
            InitializeComponent();
            _ = InitializePageAsync();
        }

        /// <summary>
        /// Initializes the page by retrieving the user's profile and school code.
        /// </summary>
        private async Task InitializePageAsync()
        {
            try
            {
                var user = ((App)Application.Current).User; // Access the current logged-in user

                // Ensure a user is logged in
                if (user == null || string.IsNullOrWhiteSpace(user.Email))
                {
                    await DisplayAlert("Error", "No user is logged in. Please log in to continue.", "OK");
                    return;
                }

                SchoolCode = user.SchoolCode ?? "DefaultCode"; // Retrieve SchoolCode or use a default value
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to initialize the page: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Handles the "Athlete Information" button click event.
        /// Navigates to the AthleteInformation page.
        /// </summary>
        private async void OnAthleteInfoClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new AthleteInformation());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to navigate: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Handles the "Athlete Forms" button click event.
        /// Navigates to the AthletePastForms page.
        /// </summary>
        private async void OnAthleteFormsClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new AthletePastForms());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to navigate: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Handles the "Injury Statistics" button click event.
        /// Navigates to the InjuryStatistics page.
        /// </summary>
        private async void OnInjuryStatisticsClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new InjuryStatistics());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to navigate: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Handles the "Athlete Statuses" button click event.
        /// Navigates to the AthleteStatuses page if SchoolCode is available.
        /// </summary>
        private async void OnAthleteStatusesClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SchoolCode))
                {
                    await DisplayAlert("Error", "School code not available. Please log in again.", "OK");
                    return;
                }

                await Navigation.PushAsync(new AthleteStatuses(SchoolCode));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to navigate: {ex.Message}", "OK");
            }
        }
    }
}
