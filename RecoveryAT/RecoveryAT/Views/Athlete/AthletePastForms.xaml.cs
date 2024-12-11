/*
    Date: 12/10/2024
    Description: AthletePastForms screen, useful for searching and viewing forms by type or other metrics.
    Bugs: None known
    Reflection: Pulling up forms was only initially challenging.
*/

using System.Collections.ObjectModel;

namespace RecoveryAT
{
    public partial class AthletePastForms : ContentPage
    {
        // The selected status for filtering athlete forms
        private string _selectedStatus;

        // Public property for binding the selected status
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged(nameof(SelectedStatus));
                    FilterAndSortAthletes(); // Update the list whenever the status changes
                }
            }
        }

        // Collection of athlete forms displayed in the UI
        public ObservableCollection<AthleteForm> AthleteList { get; set; } = new ObservableCollection<AthleteForm>();

        // Options for filtering athlete forms by status
        public ObservableCollection<string> StatusOptions { get; set; } = new ObservableCollection<string>
        {
            "All",
            "Eval",
            "Tape",
            "Rehab",
            "Wound",
            "Other"
        };

        /// <summary>
        /// Constructor for initializing the AthletePastForms screen.
        /// </summary>
        public AthletePastForms()
        {
            _selectedStatus = "All"; // Initialize with "All" to show all forms by default
            InitializeComponent();

            LoadAthletes(); // Load all athlete forms initially

            BindingContext = this; // Set the BindingContext for data binding
            OnPropertyChanged(nameof(AthleteList));
        }

        /// <summary>
        /// Refreshes the athlete list when the page is displayed.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadAthletes(); // Ensure the data is up-to-date
        }

        /// <summary>
        /// Event handler for tapping on a tile to view detailed information about an athlete's form.
        /// </summary>
        private async void OnTileTapped(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var tappedItem = frame.BindingContext;

            // Navigate to the AthleteFormInformation page for the tapped item
            AthleteForm currForm = (AthleteForm)tappedItem;
            await Navigation.PushAsync(new AthleteFormInformation(currForm));
        }

        /// <summary>
        /// Loads all athlete forms created before today and filters them by the logged-in user's SchoolCode.
        /// </summary>
        private void LoadAthletes()
        {
            try
            {
                // Retrieve the SchoolCode for the logged-in user
                var schoolCode = ((App)Application.Current).User.SchoolCode;

                // Fetch all forms created before today and filter by SchoolCode
                ObservableCollection<AthleteForm> athletes = new ObservableCollection<AthleteForm>(
                    MauiProgram.BusinessLogic.GetFormsBeforeToday().Where(a => a.SchoolCode == schoolCode)
                );

                // Clear the existing list and populate with fetched data
                AthleteList.Clear();
                if (athletes != null)
                {
                    foreach (var athlete in athletes)
                    {
                        AthleteList.Add(athlete);
                    }
                }
                OnPropertyChanged(nameof(AthleteList)); // Notify the UI about changes
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading athletes: {ex.Message}");
            }
        }

        /// <summary>
        /// Filters and sorts athlete forms based on the selected status.
        /// </summary>
        private void FilterAndSortAthletes()
        {
            try
            {
                // Retrieve the SchoolCode for the logged-in user
                var schoolCode = ((App)Application.Current).User.SchoolCode;

                ObservableCollection<AthleteForm> athletes;

                // Apply filtering based on the selected status
                if (SelectedStatus == "All")
                {
                    athletes = new ObservableCollection<AthleteForm>(
                        MauiProgram.BusinessLogic.GetFormsBeforeToday().Where(a => a.SchoolCode == schoolCode)
                    );
                }
                else
                {
                    athletes = new ObservableCollection<AthleteForm>(
                        MauiProgram.BusinessLogic.GetFormsBeforeToday()
                            .Where(a => a.TreatmentType == SelectedStatus && a.SchoolCode == schoolCode)
                    );
                }

                // Clear the existing list and populate with filtered data
                AthleteList.Clear();
                foreach (var athlete in athletes)
                {
                    AthleteList.Add(athlete);
                }
                OnPropertyChanged(nameof(AthleteList)); // Notify the UI about changes
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering athletes: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles text changes in the search bar and updates the athlete list based on the search query.
        /// </summary>
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                // If the search query is empty, apply the current filter and sorting
                FilterAndSortAthletes();
            }
            else
            {
                // Perform a search with the entered query
                SearchAthletes(e.NewTextValue);
            }
        }

        /// <summary>
        /// Searches for athletes based on the query and updates the athlete list.
        /// </summary>
        private void SearchAthletes(string query)
        {
            try
            {
                // Retrieve the SchoolCode for the logged-in user
                var schoolCode = ((App)Application.Current).User.SchoolCode;

                // Perform a search across multiple criteria
                var searchResults = MauiProgram.BusinessLogic.SearchAthletesByMultipleCriteria(query);

                // Filter search results by forms created before today and matching SchoolCode
                var filteredResults = new ObservableCollection<AthleteForm>(
                    searchResults.Where(a => a.DateCreated < DateTime.Today && a.SchoolCode == schoolCode)
                );

                // Clear the existing list and populate with search results
                AthleteList.Clear();
                foreach (var athlete in filteredResults)
                {
                    AthleteList.Add(athlete);
                }
                OnPropertyChanged(nameof(AthleteList)); // Notify the UI about changes
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching athletes: {ex.Message}");
            }
        }
    }
}
