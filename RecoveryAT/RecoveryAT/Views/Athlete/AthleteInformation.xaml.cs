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
        private readonly Database _database;
        
        public ObservableCollection<Athlete> AthleteList { get; set; }

        public ICommand NavigateToPastFormsCommand { get; }
        public ICommand NavigateToStatisticsCommand { get; }
        public ICommand NavigateToAthleteStatusesCommand { get; }
        public ICommand NavigateToHomeCommand { get; }

        public ObservableCollection<AthleteContact> ContactList { get; set; } = new ObservableCollection<AthleteContact>();

        public AthleteInformation()
        {
            InitializeComponent();

            _database = new Database();

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
            AthleteForm selectedAthlete = new AthleteForm(currAthlete.Name.Split(" ")[0], currAthlete.Name.Split(" ")[1],"Sport","Injury","stat"); // should get from database, fix later
            await Detail.Navigation.PushAsync(new AthleteFormInformation(selectedAthlete)); // navigate to athlete form information on tapped
        }
        private void LoadContacts()
        {
            ContactList.Clear();
            var contacts = _database.SelectAllContacts();
            foreach (var contact in contacts)
            {
                ContactList.Add(contact);
            }
        }

        private void NavigateToHome(object obj)
        {
            Application.Current.MainPage = new NavigationPage(new MainTabbedPage());
            IsPresented = false;
        }

        private async void NavigateToPastForms()
        {
            Detail = new NavigationPage(new AthletePastForms());
            IsPresented = false; // Hide flyout
        }

        private async void NavigateToStatistics()
        {
            Detail = new NavigationPage(new InjuryStatistics());
            IsPresented = false;
        }

        private async void NavigateToAthleteStatuses()
        {
            Detail = new NavigationPage(new AthleteStatuses());
            IsPresented = false;
        }
    }

    public class Athlete
    {
        public string Date { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string PhoneNumber { get; set; }
        public string Grade { get; set; }
    }
}
