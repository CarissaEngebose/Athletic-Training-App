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
using Microcharts;
using SkiaSharp;
using Microsoft.Maui.Controls;

namespace RecoveryAT
{
    // The InjuryStatistics class represents the page for displaying injury statistics
    public partial class InjuryStatistics : ContentPage
    {
        ChartEntry[] entries = new[]
        {
            new ChartEntry(32)
            {
                Label = "Ankle",
                ValueLabel = "32%",
                Color = SKColor.Parse("#4A90E2")
            },
            new ChartEntry(12)
            {
                Label = "Shoulder",
                ValueLabel = "12%",
                Color = SKColor.Parse("#7ED321")
            },
            new ChartEntry(15)
            {
                Label = "Knee",
                ValueLabel = "15%",
                Color = SKColor.Parse("#F5A623")
            },
            new ChartEntry(21)
            {
                Label = "Head",
                ValueLabel = "21%",
                Color = SKColor.Parse("#9013FE")
            },
            new ChartEntry(20)
            {
                Label = "Wrist",
                ValueLabel = "20%",
                Color = SKColor.Parse("#D0021B")
            }
        };

        // Constructor to initialize the page components
        public InjuryStatistics()
        {
            InitializeComponent(); // Load the XAML components

            // Set the default selection to "All Sports" in the picker
            sportPicker.SelectedIndex = 0;

            // create pie chart
            chartView.Chart = new PieChart
            {
                LabelTextSize = 40, Entries = entries
            };
        }

        // Event handler for when a sport is selected from the dropdown picker
        private void OnSportSelected(object sender, EventArgs e)
        {
            // Cast the sender to a Picker control
            var picker = (Picker)sender;

            // Get the index of the selected item
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1) // Ensure a valid selection was made
            {
                // Retrieve the selected sport's name
                string selectedSport = (string)picker.SelectedItem;

                // Logic to update the pie chart based on the selected sport
                // Example: Display an alert (replace with actual chart logic)
            }
        }
    }
}