using System.ComponentModel;
using System.Windows.Input;
using CalendarManagment;

namespace RecoveryAT
{
    public class CalendarViewModel : INotifyPropertyChanged
    {
        public List<String> Months { get; set; }
        public List<int> Years { get; set; } // property to make data binding easier
        private Date _selectedDate;
        public Date SelectedDate { get => _selectedDate; set { _selectedDate = value; OnPropertyChanged(); } }
        public Action OnNewDateCreated; // called each time a new date is created
        public ICommand SetNextWeekCommand { get; set; }
        public ICommand SetPreviousWeekCommand { get; set; }

        public CalendarViewModel()
        {
            SelectedDate = new Date(DateTime.Now);
            Months = new List<String>(Date.MonthNames); // load in month names for data binding
            Years = Enumerable.Range(1950, 101).ToList(); // generate list of years 1950 to 2050

            SetNextWeekCommand = new Command(execute: () => SetNextWeek());
            SetPreviousWeekCommand = new Command(execute: () => SetPreviousWeek());
        }

        public void SetNextWeek()
        {
            Date NextWeek = SelectedDate.AddDays(7);
            if (NextWeek.Month == SelectedDate.Month) // if not end of month
            { 
                SelectedDate = NextWeek; // go to next week
                OnNewDateCreated?.Invoke(); // notify that new date created
            }
        }

        public void SetPreviousWeek()
        {
            Date PreviousWeek = SelectedDate.AddDays(-7);
            if (PreviousWeek.Month == SelectedDate.Month) { // if not beginning of month
                SelectedDate = PreviousWeek; // go to previous week
                OnNewDateCreated?.Invoke(); // notify that new date created
            }
        }

        public void SetMonth(int Month)
        {
            SelectedDate = SelectedDate.AddDays(1 - SelectedDate.Day); // set day to first of month
            SelectedDate = SelectedDate.AddMonths(Month - SelectedDate.Month + 1); // calculate new month
            OnNewDateCreated?.Invoke();
        }

        public void SetMonth(String Month)
        {
            SelectedDate = SelectedDate.AddDays(1 - SelectedDate.Day); // set day to first of month
            SelectedDate = SelectedDate.AddMonths(Months.IndexOf(Month) - SelectedDate.Month + 1); // calculate new month
            OnNewDateCreated?.Invoke();
        }

        public void SetYear(int Year)
        {
            SelectedDate = SelectedDate.AddDays(1 - SelectedDate.Day); // set day to first of month
            SelectedDate = SelectedDate.AddYears(Year - SelectedDate.Year); // calculate new year
            OnNewDateCreated?.Invoke();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}