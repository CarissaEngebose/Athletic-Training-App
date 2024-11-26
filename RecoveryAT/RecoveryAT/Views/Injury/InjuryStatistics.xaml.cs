/*
    Name: Carissa Engebose
    Date: 10/13/2024
    Description: A screen that allows a user to use a dropdown list for sports and see corresponding statistics in pie chart form.
    Bugs: None that I know of.
    Reflection: This screen was very difficult because I kept trying to find a way to put an actual pie chart in, 
                but no matter what I did or the other files I created, I wasn't able to create one. 
                I even found a GitHub solution, but it didn’t work. Overall, this screen needs more work, 
                but the layout is a good starting point.
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microcharts;
using SkiaSharp;
using Microsoft.Maui.Controls;

namespace RecoveryAT
{    
    // The InjuryStatistics class represents the page for displaying injury statistics
    public partial class InjuryStatistics : ContentPage
    {
        private IBusinessLogic _businessLogic;
        private AuthenticationService authService;

        // Constructor to initialize the page components
        public InjuryStatistics()
        {
            InitializeComponent(); // Load the XAML components

            _businessLogic = MauiProgram.BusinessLogic;

            authService = ((App)Application.Current).AuthService; // gets the current user information
        }

        // this method is called when the page is about to appear
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            sportPicker.SelectedIndex = 0; // set the default selection to "All Sports" in the picker

            var entries = await GetInjuryStatisticsForSportAsync("All Sports"); // fetch entries for all sports

            // update the chart with the fetched statistics
            UpdateChartWithEntries(entries);
        }

        // Method to create and display a pie chart with specified entries
        private void UpdateChartWithEntries(List<ChartEntry> entries)
        {
            chartView.Chart = new PieChart
            {
                LabelTextSize = 40,
                Entries = entries
            };
        }

        // event handler for when a sport is selected from the dropdown picker
        private async void OnSportSelected(object sender, EventArgs e)
        {
            // Cast the sender to a Picker control
            var picker = (Picker)sender;

            // Get the index of the selected item
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1) // Ensure a valid selection was made
            {
                // Retrieve the selected sport's name
                string selectedSport = (string)picker.SelectedItem;

                // Fetch injury statistics from the database based on the selected sport
                var entries = await GetInjuryStatisticsForSportAsync(selectedSport);

                // Update the chart with the fetched statistics
                UpdateChartWithEntries(entries);
            }
        }

        // async method to retrieve injury statistics for a specific sport from the database
        private async Task<List<ChartEntry>> GetInjuryStatisticsForSportAsync(string sport)
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            try
            {
                string schoolCode = authService.SchoolCode; // gets school code from user information 

                // fetch injury statistics based on the sport
                var statistics = sport == "All Sports"
                    ? _businessLogic.GetStatisticsForAllSports(schoolCode)
                    : _businessLogic.GetStatisticsForSport(schoolCode, sport);

                // covert data to ChartEntry objects for the pie chart
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