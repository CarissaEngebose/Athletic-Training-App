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
        private readonly Database _database;

        public ObservableCollection<AthleteForm> AthleteList { get; set; }
        public ObservableCollection<string> StatusOptions { get; set; }

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

        public AthleteStatuses()
        {
            InitializeComponent();

            _database = new Database();

            AthleteList = new ObservableCollection<AthleteForm>();
            StatusOptions = new ObservableCollection<string>
            {
                "All", // Add an option to show all athletes
                "Full Contact",
                "Limited Contact",
                "Activity as Tolerated",
                "Total Rest"
            };

            // Load all athletes initially
            LoadAthletes();

            BindingContext = this;
            OnPropertyChanged(nameof(AthleteList));
        }

        private async void OnTileTapped(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var tappedItem = frame.BindingContext; // get the tapped item information

            await Navigation.PushAsync(new AthleteFormInformation(new AthleteForm("First", "Last","Sport","Inj","stat"))); // navigate to athlete form information on tapped

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
