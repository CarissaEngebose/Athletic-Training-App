/*
    Name: Hannah Hotchkiss
	Date: 10/14/2024
	Description: AthleteStatuses - This page displays a list of athletes along with their status, injury, and sport. 
    Users can select a status from a dropdown menu and search for athletes using a search bar.
	Bugs: None Known
    Reflection: This screen was a little more complicated because it's just a digital prototype so the binding for the 
    showing of example athletes and status took some time to figure out.
*/

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace RecoveryAT
{
    public partial class AthleteStatuses : FlyoutPage
    {
        private readonly BusinessLogic _businessLogic;
        public ObservableCollection<AthleteForm> AthleteList { get; set; }
        public ObservableCollection<string> StatusOptions { get; set; }
        public ObservableCollection<string> SearchOptions { get; set; }
        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged(nameof(SelectedStatus));
                    FilterAndSortAthletes();
                }
            }
        }

        public AthleteStatuses()
        {
            InitializeComponent();

            _businessLogic = new BusinessLogic(new Database());
            AthleteList = new ObservableCollection<AthleteForm>();
            SearchOptions = new ObservableCollection<string>
            {
                "All",
                "No Status",
                "Full Contact",
                "Limited Contact",
                "Activity as Tolerated",
                "Total Rest"
            };

            StatusOptions = new ObservableCollection<string>
            {
                "No Status",
                "Full Contact",
                "Limited Contact",
                "Activity as Tolerated",
                "Total Rest"
            };
            LoadAthletes();
            BindingContext = this;
        }

        private async void OnTileTapped(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var tappedItem = frame.BindingContext; // get the tapped item information

            // Should pass in a valid AthleteForm with info from database, Dummy data now fix later - Dominick
            await Navigation.PushAsync(new AthleteFormInformation(new AthleteForm("First", "Last","Sport","Inj","stat"))); // navigate to athlete form information on tapped
        }

        private void LoadAthletes()
        {
            try
            {
                var athletes = _businessLogic.GetAllForms();

                AthleteList.Clear();
                foreach (var athlete in athletes)
                {
                    // Set "No Status" only if the status from the database is null or empty
                    athlete.Status = string.IsNullOrEmpty(athlete.Status) ? "No Status" : athlete.Status;
                    AthleteList.Add(athlete);
                }
                OnPropertyChanged(nameof(AthleteList));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading athletes: {ex.Message}");
            }
        }

        private void FilterAndSortAthletes()
        {
            try
            {
                var athletes = _businessLogic.GetAllForms().ToList();

                // Check the selected status and filter accordingly
                if (SelectedStatus == "All")
                {
                    // When "All" is selected, include all athletes regardless of their status
                    athletes.ForEach(a => a.Status = string.IsNullOrEmpty(a.Status) ? "No Status" : a.Status);
                }
                else if (SelectedStatus == "No Status")
                {
                    // Include athletes with null or empty statuses in addition to those explicitly set to "No Status"
                    athletes = athletes.Where(a => string.IsNullOrEmpty(a.Status) || a.Status == "No Status").ToList();
                }
                else
                {
                    // Filter for athletes with the specific selected status
                    athletes = athletes.Where(a => a.Status == SelectedStatus).ToList();
                }

                // Clear the current list and add the filtered athletes
                AthleteList.Clear();
                foreach (var athlete in athletes)
                {
                    // Ensure "No Status" is set for display purposes if the status is null or empty
                    athlete.Status = string.IsNullOrEmpty(athlete.Status) ? "No Status" : athlete.Status;
                    AthleteList.Add(athlete);
                }
                OnPropertyChanged(nameof(AthleteList));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering athletes: {ex.Message}");
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                LoadAthletes();
            }
            else
            {
                SearchAthletes(e.NewTextValue);
            }
        }

        private void SearchAthletes(string query)
        {
            try
            {
                var searchResults = _businessLogic.SearchAthletes(query);

                AthleteList.Clear();
                foreach (var athlete in searchResults)
                {
                    // Ensure "No Status" is set for null or empty statuses
                    athlete.Status = string.IsNullOrEmpty(athlete.Status) ? "No Status" : athlete.Status;
                    AthleteList.Add(athlete);
                }
                OnPropertyChanged(nameof(AthleteList));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching athletes: {ex.Message}");
            }
        }

        private async void OnStatusChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var selectedStatus = (string)picker.SelectedItem;

            if (picker.BindingContext is AthleteForm selectedAthlete)
            {
                selectedAthlete.Status = selectedStatus;

                try
                {
                    // Ensure FormKey is available before attempting to update the database
                    if (selectedAthlete.FormKey.HasValue)
                    {
                        string result = _businessLogic.UpdateContactStatus(selectedAthlete.FormKey.Value, selectedStatus);

                        // Check if the update was successful based on the returned result
                        if (result.Contains("successfully"))
                        {
                            await DisplayAlert("Success", "Athlete status updated successfully.", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Error", "Failed to update athlete status in the database.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "FormKey is null and cannot be used to update status.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
        }
    }
}