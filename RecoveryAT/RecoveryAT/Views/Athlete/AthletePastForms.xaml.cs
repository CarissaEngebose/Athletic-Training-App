/*
    Name: Luke Kastern
    Date: 10/14/2024
    Description: AthletePastForms Screen
    Bugs: None Known
    Reflection: This was fairly easy because I had AthleteStatus as a reference, 
                but it was hard to get the form number to be the only element on the right side.
*/

using System.Collections.ObjectModel;

namespace RecoveryAT
{
    public partial class AthletePastForms : FlyoutPage
    {
        private readonly Database _database;

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
                    FilterAndSortAthletes(); // Update list when status changes
                }
            }
        }

        public ObservableCollection<AthleteForm> AthleteList { get; set; } = new ObservableCollection<AthleteForm>();
        public ObservableCollection<string> StatusOptions { get; set; } = new ObservableCollection<string>
        {
            "All",
            "Full Contact",
            "Limited Contact",
            "Activity as Tolerated",
            "Total Rest"
        };

        // Constructor to initialize the AthletePastForms page
        public AthletePastForms()
        {
            _selectedStatus = "All"; // Initialize _selectedStatus with a non-null value
            InitializeComponent();

            _database = new Database();

            LoadAthletes(); // Load all athletes initially

            BindingContext = this;
            OnPropertyChanged(nameof(AthleteList));
        }

        private async void OnTileTapped(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            // var tappedItem = frame.BindingContext; // get the tapped item information

            await Navigation.PushAsync(new AthleteFormInformation()); // navigate to athlete form information on tapped

        }

        private void LoadAthletes()
        {
            try
            {
                // Load all athletes from the database
                var athletes = _database.SelectAllForms(); // This returns a List<AthleteForm>

                AthleteList.Clear(); // Clear the existing ObservableCollection
                foreach (var athlete in athletes)
                {
                    AthleteList.Add(athlete); // Add each item from the List to the ObservableCollection
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
                // Convert athletes to a List to perform LINQ filtering, if needed
                var athletes = _database.SelectAllForms().ToList();

                // Filter by selected status if it's not "All"
                if (SelectedStatus != "All" && !string.IsNullOrEmpty(SelectedStatus))
                {
                    athletes = athletes.Where(a => a.Status == SelectedStatus).ToList();
                }

                // Convert the filtered list back to ObservableCollection for data binding
                AthleteList.Clear();
                foreach (var athlete in athletes)
                {
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
            // If the search bar is empty, reload or filter athletes based on status
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                FilterAndSortAthletes();
            }
            else
            {
                // Perform search with the new text
                SearchAthletes(e.NewTextValue);
            }
        }

        private void SearchAthletes(string query)
        {
            try
            {
                // Call the search method in the Database class
                var searchResults = _database.SearchAthletes(query);

                AthleteList.Clear();
                foreach (var athlete in searchResults)
                {
                    AthleteList.Add(athlete);
                }
                OnPropertyChanged(nameof(AthleteList));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching athletes: {ex.Message}");
            }
        }

    }
}
