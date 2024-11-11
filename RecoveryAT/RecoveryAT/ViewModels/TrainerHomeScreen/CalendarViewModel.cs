using System.ComponentModel;
using CalendarManagment;

namespace RecoveryAT{
    
    public class CalendarViewModel : INotifyPropertyChanged
    {
        public List<String> Months {get; set;}
        public List<int> Years {get; set;} // property to make data binding easier
        private Date _selectedDate;
        public Date SelectedDate {get => _selectedDate; set{_selectedDate = value; OnPropertyChanged();}}
        public Action<DateTime> OnDaySelected;
        public CalendarViewModel()
        {
            SelectedDate = new Date(DateTime.Now, OnDaySelected);
            Months = new List<String>(Date.MonthNames);
            Years = Enumerable.Range(1950, 101).ToList(); // generate list of years 1950 to 2050
        }

        public void SetNextWeek()
        {
            SelectedDate = SelectedDate.AddDays(7); // go to next week
        }

        public void SetPreviousWeek()
        {
            SelectedDate = SelectedDate.AddDays(-7); // go to previous week
        }

        public void SetMonth(int Month){
            SelectedDate = SelectedDate.AddMonths(SelectedDate.Month - Month); // calculate new month
        }

        public void SetYear(int Year){
            SelectedDate = SelectedDate.AddYears(SelectedDate.Year - Year); // calculate new year
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}