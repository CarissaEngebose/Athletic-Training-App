using System.Collections.ObjectModel;
using System.ComponentModel;
using CalendarManagment;

/*
    Name: Dominick Hagedorn
    Date: 11/10/2024
    Description: TrainerHomeScreenViewModel
    Bugs: You have to click twice to select a day
    Reflection: I find this view model to be a lot more readable, but it
    still has a long way to go. There is definetly an easier way to do this
    with the built in .net DateTime object that I am going to have to figure out later
    
*/
namespace RecoveryAT
{
    public partial class TrainerHomeScreenViewModel : INotifyPropertyChanged
    {
        private IBusinessLogic _businessLogic;
        private String _schoolCode;
        private ObservableCollection<AthleteForm> _athleteForms;
        public CalendarViewModel Calendar { get; set; } = new CalendarViewModel();
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<AthleteForm> AthleteForms
        {
            get => _athleteForms;
            set
            {
                _athleteForms = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        public TrainerHomeScreenViewModel(IBusinessLogic BusinessLogic, String SchoolCode)
        {
            this._businessLogic = BusinessLogic;
            this._schoolCode = SchoolCode;
            this.AthleteForms = new ObservableCollection<AthleteForm>();

            NewDateCreated(); // Create a new date on instantiation
            Calendar.OnNewDateCreated += NewDateCreated; // Called each time the date changes

            // Load forms for today's date on initialization
            LoadAthleteFormsForDay(DateTime.Today);
        }

        // code that needs to be run everytime a new date is created
        private void NewDateCreated()
        {
            Calendar.SelectedDate.OnNewDaySelected += LoadAthleteFormsForDay;
        }

        public void LoadAthleteFormsForDay(DateTime date)
        {
            AthleteForms = _businessLogic.GetFormsByDateCreated(_schoolCode, date);
        }
    }
}