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
        IBusinessLogic BusinessLogic;
        String SchoolCode;
        public TrainerHomeScreenViewModel(IBusinessLogic BusinessLogic, String SchoolCode)
        {
            Calendar.LoadAthleteForms += LoadAthleteFormsForDay; // set up event to load athlete forms when day is selected
            this.BusinessLogic = BusinessLogic;
            this.SchoolCode = SchoolCode;
            this.AthleteForms = new ObservableCollection<AthleteForm>();
        }

        ObservableCollection<AthleteForm> _athleteForms;
        public CalendarViewModel Calendar { get; set; } = new CalendarViewModel();
        public ObservableCollection<AthleteForm> AthleteForms
        {
            get => _athleteForms;
            set
            {
                _athleteForms = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void LoadAthleteFormsForDay(DateTime date)
        {
            AthleteForms = BusinessLogic.GetFormsByDate(SchoolCode, date);
            Console.WriteLine(date);
        }
    }

    public class CalendarViewModel : INotifyPropertyChanged
    {
        private string[] monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        List<Day> _week;
        int _selectedDay;
        String _month;
        int _year;
        private DateTime _fullDate;


        public List<Day> Week { get => _week; set { _week = value; OnPropertyChanged(); } }
        public string Month { get => _month; set { _month = value; OnPropertyChanged(); } }
        public int Year { get => _year; set { _year = value; OnPropertyChanged(); } }
        public int SelectedDay { get => _selectedDay; set { _selectedDay = value; OnPropertyChanged(); } }
        public DateTime FullDate {get{return new DateTime(Year, Array.IndexOf(monthNames, Month), Week[0].DayNumber);}}

        public CalendarViewModel()
        {
            // sets calendar date to today
            Week = CalculateWeek(DateTime.Now);
            Month = monthNames[DateTime.Now.Month];
            Year = DateTime.Now.Year;
        }

        private List<Day> CalculateWeek(DateTime currDay)
        {
            DateTime sundayDate = currDay.AddDays(-(int)currDay.DayOfWeek); // gets closest previous sunday
            List<Day> daysOfTheWeek = new List<Day>(); // temporary storage of new days
            for (int currDayOfTheWeek = 0; currDayOfTheWeek < 7; currDayOfTheWeek++) // for each day in the week
            {
                Day NewDay = new Day(Day.dayNames[currDayOfTheWeek], sundayDate.AddDays(currDayOfTheWeek).Day); // create a new day
                daysOfTheWeek.Add(NewDay); // add new day to week
                // set what happens when a new day is selected
                NewDay.IsSelectedEvent += (SelectedDay) =>
                {
                    foreach (Day day in Week) // for each day in the week
                    {
                        if (!day.Equals(SelectedDay) && day.IsSelected) // if day isnt newly selected day
                        {
                            day.IsSelected = false; // deselect
                        }
                    }
                    LoadAthleteForms?.Invoke(FullDate);
                };
            }
            return daysOfTheWeek;
        }

        private void SetNextWeek()
        {
            int MonthNumber = Array.IndexOf(monthNames, Month) + 1;
            // if week is last week in month
            if (DateTime.DaysInMonth(MonthNumber, Year) == Week[6].DayNumber)
            {
                if (!Month.Equals("December")) // if end of year
                {
                    Year++; // go to next year
                }
                Month = monthNames[++MonthNumber % 12]; //go to next month
            }
            Week = CalculateWeek(FullDate.AddDays(7)); // go to next week
        }

        private void SetPreviousWeek()
        {
            int MonthNumber = Array.IndexOf(monthNames, Month);
            // if week is first week in month
            if (1 == Week[0].DayNumber)
            {
                if (!Month.Equals("December"))
                { // if end of year
                    Year++; // go to previous year
                }
                Month = monthNames[--MonthNumber % 12]; //go to previous month
            }
            Week = CalculateWeek(FullDate.AddDays(-7)); // go to previous week
        }

        // this shouldn't be in a calendar, figure out some way to put into TrainerHomeScreenViewModel
        public event Action<DateTime> LoadAthleteForms;

        public class Day : INotifyPropertyChanged
        {
            public Day(String dayName, int dayNumber)
            {
                this.DayName = dayName;
                this.DayNumber = dayNumber;
                this.IsSelected = false;
            }
            public event Action<Day> IsSelectedEvent;  // Event for when a day is selected
            private bool _isSelected;
            public string DayName { get; set; } // mon, tue etc
            public int DayNumber { get; set; }
            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    _isSelected = value;
                    IsSelectedEvent?.Invoke(this);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                }
            }
            public static String[] dayNames = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

            public event PropertyChangedEventHandler? PropertyChanged;

            // overide == method to make comparisions easier
            public override bool Equals(object o)
            {
                if (!(o is Day))
                    return false;
                Day SecondDay = (Day)o;
                return this.DayName == SecondDay.DayName && this.DayNumber == SecondDay.DayNumber;
            }
        }

        // code for data binding
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}