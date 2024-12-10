/*
    Date: 12/06/2024
    Description: This page displays a list of athletes along with their status, injury, and sport. 
                 Users can select a status from a dropdown menu and search for athletes using a search bar.
    Bugs: None Known
    Reflection: This screen was pretty easy to implement. The hardest part was the search feature so it searches
                anything on the screen.
*/

using System.Collections.ObjectModel;

namespace RecoveryAT
{
    public partial class AthleteStatuses : ContentPage
    {
        private readonly BusinessLogic _businessLogic;
        private readonly string _schoolCode; // Unique code to filter athletes by school.
        public ObservableCollection<AthleteForm> AthleteList { get; set; } // Dynamic list of athletes for data binding.
        public ObservableCollection<string> StatusOptions { get; set; } // Dropdown options for athlete statuses.
        public ObservableCollection<string> SearchOptions { get; set; } // Options for the search functionality.
        private bool _isUserChange = false;// Flag to track user-initiated changes

        private string _selectedStatus; // Tracks the currently selected status from the dropdown.
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged(nameof(SelectedStatus)); // Notify UI of changes.
                    FilterAndSortAthletes(); // Filter and update the athlete list based on the new status.
                }
            }
        }

        // Constructor initializes the page and loads athlete data
        public AthleteStatuses(string schoolCode)
        {
            _schoolCode = schoolCode;
            _selectedStatus = "All"; // Default status filter.
            InitializeComponent();

            // Initialize collections for data binding
            AthleteList = new ObservableCollection<AthleteForm>();
            StatusOptions = new ObservableCollection<string>
            {
                "No Status",
                "Full Contact",
                "Limited Contact",
                "Activity as Tolerated",
                "Total Rest"
            };

            // Search options include all statuses and no filter
            SearchOptions = new ObservableCollection<string>
            {
                "All",
                "No Status",
                "Full Contact",
                "Limited Contact",
                "Activity as Tolerated",
                "Total Rest"
            };

            _businessLogic = new BusinessLogic(
                             new ContactsDatabase(),
                             new FormsDatabase(),
                             new UsersDatabase(),
                             new SearchDatabase(),
                             new Database());
            LoadAthletes(); // Load athlete data from the database.
            BindingContext = this; // Set data binding context to this page.
        }

        // Handles taps on athlete tiles and navigates to the detailed athlete form page.
        private async void OnTileTapped(object sender, EventArgs e)
        {
            try
            {
                var frame = (Frame)sender;
                var tappedAthlete = frame.BindingContext as AthleteForm;

                if (tappedAthlete != null && tappedAthlete.FormKey.HasValue)
                {
                    var athleteForm = _businessLogic.GetAllForms()
                                                    .FirstOrDefault(a => a.FormKey == tappedAthlete.FormKey.Value);

                    if (athleteForm != null)
                    {
                        await Navigation.PushAsync(new AthleteFormInformation(athleteForm)); // Navigate to details page.
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

        // Loads athletes from the database and applies the school filter.
        private void LoadAthletes()
        {
            try
            {
                var athletes = _businessLogic.GetAllForms()
                                             .Where(a => a.SchoolCode == _schoolCode);

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
                Console.WriteLine($"Error loading athletes: {ex.Message}");
            }
        }

        // Filters and sorts athletes based on the selected status.
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

        // Handles text input in the search bar and updates the athlete list accordingly.
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                LoadAthletes(); // Reset the list when search is cleared.
            }
            else
            {
                SearchAthletes(e.NewTextValue);
            }
        }

        // Searches athletes based on the query and updates the athlete list.
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

            if (picker.BindingContext is AthleteForm selectedAthlete && _isUserChange)
            {
                _isUserChange = false; // Reset flag after handling user change
                selectedAthlete.Status = selectedStatus;

                try
                {
                    if (selectedAthlete.FormKey.HasValue)
                    {
                        string result = _businessLogic.UpdateContactStatus(selectedAthlete.FormKey.Value, selectedStatus);

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

        // Handle the Picker's Focus event to track user interaction
        private void OnPickerFocused(object sender, FocusEventArgs e)
        {
            _isUserChange = true; // Set the flag to indicate user-initiated change
        }
    }
}
