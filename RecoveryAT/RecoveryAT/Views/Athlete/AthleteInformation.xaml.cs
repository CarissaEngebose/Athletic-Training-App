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
        public ObservableCollection<Athlete> AthleteList { get; set; }
        
        public ICommand NavigateToPastFormsCommand { get; }
        public ICommand NavigateToStatisticsCommand { get; }
        public ICommand NavigateToAthleteStatusesCommand { get; }
        public ICommand NavigateToHomeCommand { get; }

        public AthleteInformation()
        {
            InitializeComponent();

            AthleteList = new ObservableCollection<Athlete>
            {
                new Athlete { Date = "2024-10-01", Name = "John Smith", Relationship = "Mother", PhoneNumber = "(123) 456-7890", FormNumber = "12" },
                new Athlete { Date = "2024-09-22", Name = "Reece Thomas", Relationship = "Mother", PhoneNumber = "(555) 111-2222", FormNumber = "11" },
                new Athlete { Date = "2024-09-15", Name = "Marcus Rye", Relationship = "Guardian", PhoneNumber = "(111) 222-3333", FormNumber = "9" }
            };

            // Initialize Commands
            NavigateToHomeCommand = new Command(NavigateToHome);
            NavigateToPastFormsCommand = new Command(NavigateToPastForms);
            NavigateToStatisticsCommand = new Command(NavigateToStatistics);
            NavigateToAthleteStatusesCommand = new Command(NavigateToAthleteStatuses);

            BindingContext = this;
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
        public string FormNumber { get; set; }
    }
}
