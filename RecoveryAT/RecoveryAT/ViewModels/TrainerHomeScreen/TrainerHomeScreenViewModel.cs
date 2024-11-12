using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ObservableCollection<AthleteForm> AthleteForms {
            get => _athleteForms;
            set
            {
                _athleteForms = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        public TrainerHomeScreenViewModel(IBusinessLogic BusinessLogic, String SchoolCode)
        {
            NewDateCreated(); // a new date is created upon instantiation
            Calendar.OnNewDateCreated += NewDateCreated; // called each time the date is changed

            this._businessLogic = BusinessLogic;
            this._schoolCode = SchoolCode;
            this.AthleteForms = new ObservableCollection<AthleteForm>();
        }

        // code that needs to be run everytime a new date is created
        private void NewDateCreated(){
            Calendar.SelectedDate.OnNewDaySelected += LoadAthleteFormsForDay; // set up event to load athlete forms when day is selected
        }

        public void LoadAthleteFormsForDay(DateTime date)
        {
            //AthleteForms = _businessLogic.GetFormsByDate(_schoolCode, date);
            AthleteForms.Add(new AthleteForm(""+date.Day,"" + date.Month,"" + date.Year,"inj","stat"));
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>"+AthleteForms[0]);
        }
    }
}