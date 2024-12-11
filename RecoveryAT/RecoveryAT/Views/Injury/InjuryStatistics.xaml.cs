/**
    Date: 12/10/2024
    Description: A screen that allows a user to use a dropdown list for sports and view corresponding statistics in pie chart form.
    Bugs: None that I know of.
    Reflection: This screen was very difficult because it took a long time to get a working pie chart.
**/

using Microcharts;
using SkiaSharp;

namespace RecoveryAT
{
    public partial class InjuryStatistics : ContentPage
    {
        private readonly IBusinessLogic _businessLogic; // Handles business logic for data retrieval
        private readonly User _user; // Represents the logged-in user

        /// <summary>
        /// Constructor to initialize the InjuryStatistics page.
        /// </summary>
        public InjuryStatistics()
        {
            InitializeComponent(); // Load the XAML components

            _businessLogic = MauiProgram.BusinessLogic; // Access the business logic layer
            _user = ((App)Application.Current).User; // Retrieve the current logged-in user
        }

        /// <summary>
        /// Method called when the page is about to appear.
        /// Sets the default sport selection and displays statistics.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Set the default selection in the sport picker to "All Sports"
            sportPicker.SelectedIndex = 0;

            // Fetch statistics for all sports and display them on the chart
            var entries = await GetInjuryStatisticsForSportAsync("All Sports");
            UpdateChartWithEntries(entries);
        }

        /// <summary>
        /// Updates the pie chart with the given list of entries.
        /// </summary>
        /// <param name="entries">List of chart entries to display.</param>
        private void UpdateChartWithEntries(List<ChartEntry> entries)
        {
            // Create a new PieChart and assign it to the chart view
            chartView.Chart = new PieChart
            {
                LabelTextSize = 40, // Font size for labels
                Entries = entries // Data to display on the chart
            };
        }

        /// <summary>
        /// Event handler for when a sport is selected from the dropdown picker.
        /// Fetches and updates statistics for the selected sport.
        /// </summary>
        private async void OnSportSelected(object sender, EventArgs e)
        {
            // Check if the sender is a Picker control
            if (sender is Picker picker)
            {
                // Ensure a valid selection was made
                if (picker.SelectedIndex != -1)
                {
                    // Retrieve the selected sport
                    string selectedSport = (string)picker.SelectedItem;

                    // Fetch statistics for the selected sport and update the chart
                    var entries = await GetInjuryStatisticsForSportAsync(selectedSport);
                    UpdateChartWithEntries(entries);
                }
            }
        }

        /// <summary>
        /// Asynchronous method to retrieve injury statistics for a specific sport from the database.
        /// Converts the data into a format suitable for displaying on the pie chart.
        /// </summary>
        /// <param name="sport">The sport for which to retrieve statistics.</param>
        /// <returns>A list of ChartEntry objects representing the statistics.</returns>
        private async Task<List<ChartEntry>> GetInjuryStatisticsForSportAsync(string sport)
        {
            var entries = new List<ChartEntry>(); // List to store chart data

            try
            {
                // Retrieve the school code from the logged-in user's profile
                string schoolCode = _user.SchoolCode;

                // Fetch statistics based on the selected sport
                var statistics = sport == "All Sports"
                    ? _businessLogic.GetStatisticsForAllSports(schoolCode) // Get stats for all sports
                    : _businessLogic.GetStatisticsForSport(schoolCode, sport); // Get stats for a specific sport

                // Convert the retrieved data into ChartEntry objects
                foreach (var stat in statistics)
                {
                    entries.Add(new ChartEntry(stat.Percentage)
                    {
                        Label = stat.InjuryType, // Label for the type of injury
                        ValueLabel = $"{stat.Percentage}%", // Percentage value as a label
                        Color = SKColor.Parse(stat.ColorHex) // Color for the pie chart segment
                    });
                }
            }
            catch (Exception ex)
            {
                // Display an error message if statistics fail to load
                await DisplayAlert("Error", $"Failed to load statistics: {ex.Message}", "OK");
            }

            return entries; // Return the chart entries
        }
    }
}
