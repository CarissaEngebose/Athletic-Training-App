using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RecoveryAT
{
    public partial class TrainerHomeScreenViewModel
    {
        public CalendarViewModel Calendar { get; set; } = new CalendarViewModel();
        public ObservableCollection<AthleteForm> AthleteForms { get; set; } = new ObservableCollection<AthleteForm>();
    }

    public class CalendarViewModel : INotifyPropertyChanged
    {
        private string[] monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        List<Day> _week;
        int _selectedDay;
        String _month;
        int _year;
        private DateTime _fullDate;

        
        public List<Day> Week { get => _week; set{_week = value; OnPropertyChanged();}}
        public string Month { get => _month; set{_month = value; OnPropertyChanged();}}
        public int Year { get => _year; set{_year = value; OnPropertyChanged();}}
        public int SelectedDay { get => _selectedDay; set{_selectedDay = value; OnPropertyChanged();}}
        public DateTime FullDate => new DateTime(Year, Array.IndexOf(monthNames, Month), Week[0].DayNumber);

        public CalendarViewModel() 
        {
            Week = CalculateWeek(DateTime.Now);
            Month = monthNames[DateTime.Now.Month];
            Year = DateTime.Now.Year; 
        }

        private List<Day> CalculateWeek(DateTime currDay)
        {
            DateTime sundayDate = currDay.AddDays(-(int)currDay.DayOfWeek); // gets closest previous sunday
            List<Day> daysOfTheWeek = new List<Day>();
            for (int currDayOfTheWeek = 0; currDayOfTheWeek < 7; currDayOfTheWeek++)
            {
                daysOfTheWeek.Add(new Day(Day.dayNames[currDayOfTheWeek], sundayDate.AddDays(currDayOfTheWeek).Day));
            }
            return daysOfTheWeek;
        }

        private void SetNextWeek()
        {
            int MonthNumber = Array.IndexOf(monthNames, Month) + 1;
            // if week is last week in month
            if (DateTime.DaysInMonth(MonthNumber, Year) == Week[6].DayNumber)
            {
                if (!Month.Equals("December"))
                { // if end of year
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

        // code for data binding
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Day
    {
        public Day(String dayName, int dayNumber)
        {
            this.DayName = dayName;
            this.DayNumber = dayNumber;
        }
        public String DayName {get; set;}
        public int DayNumber {get; set;}
        public static String[] dayNames = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
    }
}