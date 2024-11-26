/*
    Name: Luke Kastern
    Date: 10/14/2024
    Description: AthletePastForms Screen
    Bugs: None Known
    Reflection: This was fairly easy because I had AthleteStatus as a reference, 
                but it was hard to get the form number to be the only element on the right side.
*/

using System.Collections.ObjectModel;
using System;
using System.Linq;

namespace RecoveryAT
{
    public partial class AthletePastForms : FlyoutPage
    {
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
            "Eval",
            "Tape",
            "Rehab",
            "Wound",
            "Other"
        };

        private readonly string SchoolCode = "THS24"; // REMOVE THIS LATER! Just for testing purposes

        public AthletePastForms()
        {
            _selectedStatus = "All"; // Initialize _selectedStatus with a non-null value
            InitializeComponent();

            LoadAthletes(); // Load all athletes initially

            BindingContext = this;
            OnPropertyChanged(nameof(AthleteList));
        }

        private async void OnTileTapped(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var tappedItem = frame.BindingContext;

            AthleteForm currForm = (AthleteForm)tappedItem;
            await Navigation.PushAsync(new AthleteFormInformation(currForm));
        }

        private void LoadAthletes()
        {
            try
            {
                // Load all athletes with forms created before today’s date
                ObservableCollection<AthleteForm> athletes = MauiProgram.BusinessLogic.GetFormsBeforeToday();

                AthleteList.Clear();
                if (athletes != null)
                {
                    foreach (var athlete in athletes)
                    {
                        AthleteList.Add(athlete);
                    }
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
                ObservableCollection<AthleteForm> athletes;

                // Get filtered athletes based on SelectedStatus and created date
                if (SelectedStatus == "All")
                {
                    athletes = MauiProgram.BusinessLogic.GetFormsBeforeToday();
                }
                else
                {
                    athletes = new ObservableCollection<AthleteForm>(
                        MauiProgram.BusinessLogic.GetFormsBeforeToday().Where(a => a.TreatmentType == SelectedStatus)
                    );
                }

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
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                FilterAndSortAthletes();
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
                var searchResults = MauiProgram.BusinessLogic.SearchAthletesByMultipleCriteria(query);

                // Filter search results to only include forms created before today’s date
                var filteredResults = new ObservableCollection<AthleteForm>(
                    searchResults.Where(a => a.DateCreated < DateTime.Today)
                );

                AthleteList.Clear();
                foreach (var athlete in filteredResults)
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
