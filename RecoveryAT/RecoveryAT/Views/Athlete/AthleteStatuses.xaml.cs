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
        private readonly string _schoolCode;
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

        public AthleteStatuses(string schoolCode)
        {
            _schoolCode = schoolCode;
            _selectedStatus = "All"; // Initialize with a default value
            InitializeComponent();

            AthleteList = new ObservableCollection<AthleteForm>();
            StatusOptions = new ObservableCollection<string>
            {
                "No Status",
                "Full Contact",
                "Limited Contact",
                "Activity as Tolerated",
                "Total Rest"
            };

            SearchOptions = new ObservableCollection<string>
            {
                "All",
                "No Status",
                "Full Contact",
                "Limited Contact",
                "Activity as Tolerated",
                "Total Rest"
            };

            _businessLogic = new BusinessLogic(new Database());
            LoadAthletes();
            BindingContext = this;
        }

        private async void OnTileTapped(object sender, EventArgs e)
        {
            try
            {
                var frame = (Frame)sender;
                var tappedAthlete = frame.BindingContext as AthleteForm;

                if (tappedAthlete != null && tappedAthlete.FormKey.HasValue)
                {
                    // Fetch the full athlete details from the database using the FormKey
                    var athleteForm = _businessLogic.GetAllForms()
                                                    .FirstOrDefault(a => a.FormKey == tappedAthlete.FormKey.Value);

                    if (athleteForm != null)
                    {
                        // Navigate to the AthleteFormInformation page with the fetched athleteForm
                        await Navigation.PushAsync(new AthleteFormInformation(athleteForm));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Athlete information not found in the database.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Invalid athlete selection or missing FormKey.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private void LoadAthletes()
        {
            try
            {
                // Filter athletes by the specified _schoolCode
                var athletes = _businessLogic.GetAllForms()
                                             .Where(a => a.SchoolCode == _schoolCode);

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
                var athletes = _businessLogic.GetAllForms()
                                             .Where(a => a.SchoolCode == _schoolCode)
                                             .ToList();

                if (SelectedStatus == "All")
                {
                    athletes.ForEach(a => a.Status = string.IsNullOrEmpty(a.Status) ? "No Status" : a.Status);
                }
                else if (SelectedStatus == "No Status")
                {
                    athletes = athletes.Where(a => string.IsNullOrEmpty(a.Status) || a.Status == "No Status").ToList();
                }
                else
                {
                    athletes = athletes.Where(a => a.Status == SelectedStatus).ToList();
                }

                AthleteList.Clear();
                foreach (var athlete in athletes)
                {
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
                var searchResults = _businessLogic.SearchAthletes(query)
                                                  .Where(a => a.SchoolCode == _schoolCode);

                AthleteList.Clear();
                foreach (var athlete in searchResults)
                {
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