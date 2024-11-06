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
            StatusOptions = new ObservableCollection<string>
            {
                "All",
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
            var tappedItem = frame.BindingContext;

            await Navigation.PushAsync(new AthleteFormInformation());
        }

        private void LoadAthletes()
        {
            try
            {
                var athletes = _businessLogic.GetAllForms();

                AthleteList.Clear();
                foreach (var athlete in athletes)
                {
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

                if (SelectedStatus != "All" && !string.IsNullOrEmpty(SelectedStatus))
                {
                    athletes = athletes.Where(a => a.Status == SelectedStatus).ToList();
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
                var searchResults = _businessLogic.SearchAthletes(query);

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