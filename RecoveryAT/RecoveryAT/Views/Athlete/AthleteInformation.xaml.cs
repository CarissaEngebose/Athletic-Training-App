using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace RecoveryAT
{
    public partial class AthleteInformation : FlyoutPage
    {
        public ICommand NavigateToPastFormsCommand { get; }
        public ICommand NavigateToStatisticsCommand { get; }
        public ICommand NavigateToAthleteStatusesCommand { get; }
        public ICommand NavigateToHomeCommand { get; }

        public ObservableCollection<Athlete> ContactList { get; set; } = new();
        private ObservableCollection<Athlete> _allContacts = new(); // To store the full list for resetting
        public string SearchQuery { get; set; }

        private readonly string SchoolCode = "THS24";

        public AthleteInformation()
        {
            InitializeComponent();

            LoadContacts();

            NavigateToHomeCommand = new Command(NavigateToHome);
            NavigateToPastFormsCommand = new Command(NavigateToPastForms);
            NavigateToStatisticsCommand = new Command(NavigateToStatistics);
            NavigateToAthleteStatusesCommand = new Command(NavigateToAthleteStatuses);

            BindingContext = this;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                ContactList.Clear();
                foreach (var contact in _allContacts)
                    ContactList.Add(contact);
            }
            else
            {
                var filteredContacts = _allContacts.Where(c => 
                    (c.Name != null && c.Name.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase)) ||
                    (c.Relationship != null && c.Relationship.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase)) ||
                    (c.PhoneNumber != null && c.PhoneNumber.Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase))
                );

                ContactList.Clear();
                foreach (var contact in filteredContacts)
                    ContactList.Add(contact);
            }
        }

        private async void OnTileTapped(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var tappedItem = frame.BindingContext;

            if (tappedItem is Athlete currAthlete && currAthlete.Name != null)
            {
                var nameParts = currAthlete.Name.Split(" ");
                var selectedAthlete = new AthleteForm(nameParts[0], nameParts.Length > 1 ? nameParts[1] : string.Empty, "Sport", "Injury", "stat");
                await Detail.Navigation.PushAsync(new AthleteFormInformation(selectedAthlete));
            }
        }

        private void LoadContacts()
        {
            ContactList.Clear();
            _allContacts.Clear();

            var athleteForms = MauiProgram.BusinessLogic.GetForms(schoolCode: SchoolCode);
            if (athleteForms == null) return;

            foreach (var form in athleteForms)
            {
                var contacts = MauiProgram.BusinessLogic.GetContactsByFormKey(form.FormKey ?? 0);
                if (contacts == null) continue;

                foreach (var contact in contacts)
                {
                    var athlete = new Athlete
                    {
                        Name = form.FullName,
                        Relationship = contact.ContactType,
                        PhoneNumber = contact.PhoneNumber,
                        TreatmentType = form.TreatmentType,
                        DateOfBirth = form.DateOfBirth,
                    };
                    ContactList.Add(athlete);
                    _allContacts.Add(athlete);
                }
            }
        }

        private void NavigateToHome(object obj)
        {
            if (Application.Current != null)
            {
                Application.Current.MainPage = new NavigationPage(new MainTabbedPage());
            }
            IsPresented = false;
        }

        private void NavigateToPastForms()
        {
            Detail = new NavigationPage(new AthletePastForms());
            IsPresented = false;
        }

        private void NavigateToStatistics()
        {
            Detail = new NavigationPage(new InjuryStatistics());
            IsPresented = false;
        }

        private void NavigateToAthleteStatuses()
        {
            Detail = new NavigationPage(new AthleteStatuses(SchoolCode));
            IsPresented = false;
        }
    }

    public class Athlete
    {
        public string? Name { get; set; }
        public string? Relationship { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Grade { get; set; }
        public string? TreatmentType { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
