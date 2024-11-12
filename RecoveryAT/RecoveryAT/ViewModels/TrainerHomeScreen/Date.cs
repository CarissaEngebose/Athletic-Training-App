namespace CalendarManagment
{
    // wrapper class of DateTime to make working with dates easier
    public class Date
    {
        public static string[] DaysOfWeek = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        public static string[] MonthNames = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private DateTime _date;
        public int Month => _date.Month;
        public int Year => _date.Year;
        public int Day => _date.Day;
        public int DayOfTheWeek => (int)_date.DayOfWeek;
        public List<Day> Week {get; set;}
        public Action<DateTime> OnNewDaySelected;
        public Date AddDays(double NumDays) => new Date(_date.AddDays(NumDays));
        public Date AddMonths(int months) => new Date(_date.AddMonths(months));
        public Date AddYears(int years) => new Date(_date.AddYears(years));

        public Date(int Month, int Year, int Day) : this(new DateTime(Month, Year, Day))
        {
        }

        public Date(DateTime NewDate){
            _date = NewDate;
            Week = CalculateWeek();
        }

        private List<Day> CalculateWeek()
        {
            DateTime sundayDate = _date.AddDays(-DayOfTheWeek); // gets closest previous sunday
            List<Day> daysOfTheWeek = new List<Day>(); // temporary storage of new days
            for (int CurrDay = 0; CurrDay < 7; CurrDay++) // for each day in the week
            {
                Day NewDay = new Day(DaysOfWeek[CurrDay], sundayDate.AddDays(CurrDay).Day); // create a new day
                daysOfTheWeek.Add(NewDay); // add new day to week
                // set what happens when a new day is selected
                NewDay.IsSelectedEvent += (NewSelectedDay) =>
                {
                    foreach (Day day in Week) // for each day in the week
                    {
                        if (!day.Equals(NewSelectedDay) && day.IsSelected) // if day isnt newly selected day
                        {
                            day.IsSelected = false; // deselect
                        }
                    }
                    Console.WriteLine(">>>>>>>>>>>Date delagate");
                    OnNewDaySelected?.Invoke(this._date);
                };
            }
            return daysOfTheWeek;
        }
    }
}