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
        // Instance of the business logic layer
        private readonly BusinessLogic _businessLogic;

        // Unique school code used to filter athletes by organization
        private readonly string _schoolCode;

        // Dynamic list of athletes bound to the UI
        public ObservableCollection<AthleteForm> AthleteList { get; set; }

        // Dropdown options for filtering athlete statuses
        public ObservableCollection<string> StatusOptions { get; set; }

        // Options for the search functionality
        public ObservableCollection<string> SearchOptions { get; set; }

        // Tracks user-initiated changes in the Picker control
        private bool _isUserChange = false;

        // Tracks the currently selected status for filtering
        private string _selectedStatus;

        // Property for the selected status, updates the UI when changed
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged(nameof(SelectedStatus)); // Notify UI of changes
                    FilterAndSortAthletes(); // Filter the athlete list based on the new status
                }
            }
        }

        /// <summary>
        /// Constructor for initializing the AthleteStatuses page.
        /// </summary>
        /// <param name="schoolCode">The school code for filtering athletes.</param>
        public AthleteStatuses(string schoolCode)
        {
            _schoolCode = schoolCode; // Store the provided school code
            _selectedStatus = "All"; // Default status filter
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

            // Include "All" and other statuses for search options
            SearchOptions = new ObservableCollection<string>
            {
                "All",
                "No Status",
                "Full Contact",
                "Limited Contact",
                "Activity as Tolerated",
                "Total Rest"
            };

            // Initialize the business logic instance
            _businessLogic = new BusinessLogic(
                             new ContactsDatabase(),
                             new FormsDatabase(),
                             new UsersDatabase(),
                             new SearchDatabase(),
                             new Database());

            LoadAthletes(); // Load the initial list of athletes
            BindingContext = this; // Set the BindingContext for data binding
        }

        /// <summary>
        /// Refreshes the athlete list when the page appears.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadAthletes(); // Ensure the data is up-to-date
        }

        /// <summary>
        /// Event handler for tapping on athlete tiles, navigates to the detailed athlete form page.
        /// </summary>
        private async void OnTileTapped(object sender, EventArgs e)
        {
            try
            {
                var frame = (Frame)sender;
                var tappedAthlete = frame.BindingContext as AthleteForm;

                if (tappedAthlete != null && tappedAthlete.FormKey.HasValue)
                {
                    // Fetch the full form data for the tapped athlete
                    var athleteForm = _businessLogic.GetAllForms()
                                                    .FirstOrDefault(a => a.FormKey == tappedAthlete.FormKey.Value);

                    if (athleteForm != null)
                    {
                        // Navigate to the AthleteFormInformation page
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

        /// <summary>
        /// Loads athletes from the database and applies the school filter.
        /// </summary>
        private void LoadAthletes()
        {
            try
            {
                var athletes = _businessLogic.GetAllForms()
                                             .Where(a => a.SchoolCode == _schoolCode);

                AthleteList.Clear();
                foreach (var athlete in athletes)
                {
                    // Default to "No Status" if the status is null or empty
                    athlete.Status = string.IsNullOrEmpty(athlete.Status) ? "No Status" : athlete.Status;
                    AthleteList.Add(athlete);
                }
                OnPropertyChanged(nameof(AthleteList)); // Notify UI of updates
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading athletes: {ex.Message}");
            }
        }

        /// <summary>
        /// Filters and sorts athletes based on the selected status.
        /// </summary>
        private void FilterAndSortAthletes()
        {
            try
            {
                var athletes = _businessLogic.GetAllForms()
                                             .Where(a => a.SchoolCode == _schoolCode)
                                             .ToList();

                // Apply status-based filtering
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
                    // Default to "No Status" if the status is null or empty
                    athlete.Status = string.IsNullOrEmpty(athlete.Status) ? "No Status" : athlete.Status;
                    AthleteList.Add(athlete);
                }
                OnPropertyChanged(nameof(AthleteList)); // Notify UI of updates
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering athletes: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles text input in the search bar and updates the athlete list.
        /// </summary>
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                LoadAthletes(); // Reset the list when search is cleared
            }
            else
            {
                SearchAthletes(e.NewTextValue); // Perform a search
            }
        }

        /// <summary>
        /// Searches athletes based on the query and updates the athlete list.
        /// </summary>
        private void SearchAthletes(string query)
        {
            try
            {
                var searchResults = _businessLogic.SearchAthletes(query)
                                                  .Where(a => a.SchoolCode == _schoolCode);

                AthleteList.Clear();
                foreach (var athlete in searchResults)
                {
                    // Default to "No Status" if the status is null or empty
                    athlete.Status = string.IsNullOrEmpty(athlete.Status) ? "No Status" : athlete.Status;
                    AthleteList.Add(athlete);
                }
                OnPropertyChanged(nameof(AthleteList)); // Notify UI of updates
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching athletes: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the athlete's status in the database when changed in the UI.
        /// </summary>
        private async void OnStatusChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var selectedStatus = (string)picker.SelectedItem;

            if (picker.BindingContext is AthleteForm selectedAthlete && _isUserChange)
            {
                _isUserChange = false; // Reset flag after handling the change
                selectedAthlete.Status = selectedStatus;

                try
                {
                    if (selectedAthlete.FormKey.HasValue)
                    {
                        // Update the status in the database
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

        /// <summary>
        /// Tracks user interaction with the Picker control.
        /// </summary>
        private void OnPickerFocused(object sender, FocusEventArgs e)
        {
            _isUserChange = true; // Indicate a user-initiated change
        }
    }
}
