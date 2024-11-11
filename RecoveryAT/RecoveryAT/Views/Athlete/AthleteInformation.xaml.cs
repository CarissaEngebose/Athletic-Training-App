/*
    Name: Luke Kastern
    Date: 10/14/2024
    Description: AthleteInformation Screen with Flyout Menu
    Bugs: Search bar and bottom nav bar currently don’t do anything.
    Reflection: I was able to use my AthletePastForms screen as the base and then add the flyout screen. 
                It was hard for me to implement, but my group was able to help.
*/

using System.Collections.ObjectModel;
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

        public ObservableCollection<Athlete> ContactList { get; set; } = [];

        private readonly string SchoolCode = "THS24"; // REMOVE THIS LATER! Just for testing purposes
        /// </summary>

        public AthleteInformation()
        {
            InitializeComponent();

            LoadContacts();

            // Initialize Commands
            NavigateToHomeCommand = new Command(NavigateToHome);
            NavigateToPastFormsCommand = new Command(NavigateToPastForms);
            NavigateToStatisticsCommand = new Command(NavigateToStatistics);
            NavigateToAthleteStatusesCommand = new Command(NavigateToAthleteStatuses);

            BindingContext = this;
        }

        private async void OnTileTapped(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var tappedItem = frame.BindingContext; // get the tapped item information

            Athlete currAthlete = (Athlete)tappedItem; // for testing purposes
            if (currAthlete.Name != null)
            {
                var nameParts = currAthlete.Name.Split(" ");
                AthleteForm selectedAthlete = new(nameParts[0], nameParts.Length > 1 ? nameParts[1] : string.Empty, "Sport", "Injury", "stat"); // should get from database, fix later
                await Detail.Navigation.PushAsync(new AthleteFormInformation(selectedAthlete)); // navigate to athlete form information on tapped
            }
        }
        private void LoadContacts()
        {
            ContactList.Clear(); // Clear the list (should probably be done in a better way w/ caching later)

            var athleteForms = MauiProgram.BusinessLogic.GetForms(schoolCode: SchoolCode);

            if (athleteForms == null) return; // Skip if no forms found

            foreach (var form in athleteForms)
            {
            var contacts = MauiProgram.BusinessLogic.GetContactsByFormKey(form.FormKey ?? 0);
            if (contacts == null) continue; // Skip athlete if athlete has no contacts

            foreach (var contact in contacts)
            {
                ContactList.Add(new Athlete
                {
                Name = form.FullName,
                Relationship = contact.ContactType,
                PhoneNumber = contact.PhoneNumber,
                TreatmentType = form.TreatmentType,
                Grade = form.Grade.ToString()
                });
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
            IsPresented = false; // Hide flyout
        }

        private void NavigateToStatistics()
        {
            Detail = new NavigationPage(new InjuryStatistics());
            IsPresented = false;
        }

        private void NavigateToAthleteStatuses()
        {
            string schoolCode = "12345";
            Detail = new NavigationPage(new AthleteStatuses(schoolCode));
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
    }
}
