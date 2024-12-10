/**
    Name: Carissa Engebose
    Date: 10/13/2024
    Description: A screen that allows a user to use a dropdown list for sports and view corresponding statistics in pie chart form.
    Bugs: None that I know of.
    Reflection: This screen was very difficult because I kept trying to implement an actual pie chart, 
                but nothing worked. Even using resources from GitHub didn’t help. While it still needs work, 
                the layout is a good starting point.
**/



using Microcharts;
using SkiaSharp;

namespace RecoveryAT
{
    public partial class InjuryStatistics : ContentPage
    {
        private readonly IBusinessLogic _businessLogic;
        private readonly User _user;

        // Constructor to initialize the page components
        public InjuryStatistics()
        {
            InitializeComponent(); // Load the XAML components

            _businessLogic = MauiProgram.BusinessLogic; // Access business logic
            _user = ((App)Application.Current).User; // Access user
        }

        // Method called when the page is about to appear
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            sportPicker.SelectedIndex = 0; // Default to "All Sports" in the picker

            // Fetch and display entries for all sports
            var entries = await GetInjuryStatisticsForSportAsync("All Sports");
            UpdateChartWithEntries(entries);
        }

        // Method to create and display a pie chart with specified entries
        private void UpdateChartWithEntries(List<ChartEntry> entries)
        {
            // Update the chart view with a new pie chart
            chartView.Chart = new PieChart
            {
                LabelTextSize = 40,
                Entries = entries
            };
        }

        // Event handler for when a sport is selected from the dropdown picker
        private async void OnSportSelected(object sender, EventArgs e)
        {
            // Cast the sender to a Picker control
            if (sender is Picker picker)
            {
                // Ensure a valid selection was made
                if (picker.SelectedIndex != -1)
                {
                    // Retrieve the selected sport's name
                    string selectedSport = (string)picker.SelectedItem;

                    // Fetch and display statistics for the selected sport
                    var entries = await GetInjuryStatisticsForSportAsync(selectedSport);
                    UpdateChartWithEntries(entries);
                }
            }
        }

        // Async method to retrieve injury statistics for a specific sport from the database
        private async Task<List<ChartEntry>> GetInjuryStatisticsForSportAsync(string sport)
        {
            var entries = new List<ChartEntry>();

            try
            {
                // Retrieve the school code from user
                string schoolCode = _user.SchoolCode;

                // Fetch statistics based on the sport
                var statistics = sport == "All Sports"
                    ? _businessLogic.GetStatisticsForAllSports(schoolCode)
                    : _businessLogic.GetStatisticsForSport(schoolCode, sport);

                // Convert data to ChartEntry objects for the pie chart
                foreach (var stat in statistics)
                {
                    entries.Add(new ChartEntry(stat.Percentage)
                    {
                        Label = stat.InjuryType,
                        ValueLabel = $"{stat.Percentage}%",
                        Color = SKColor.Parse(stat.ColorHex)
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load statistics: {ex.Message}", "OK");
            }

            return entries;
        }
    }
}
