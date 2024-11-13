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
        public List<Day> Week { get; set; }
        public Action<DateTime> OnNewDaySelected;
        public Date AddDays(double NumDays) => new Date(_date.AddDays(NumDays));
        public Date AddMonths(int months) => new Date(_date.AddMonths(months));
        public Date AddYears(int years) => new Date(_date.AddYears(years));

        public Date(int Month, int Year, int Day) : this(new DateTime(Month, Year, Day))
        {
        }

        public Date(DateTime NewDate)
        {
            _date = NewDate;
            Week = CalculateWeek(NewDate); // Calculate week for this date
        }

        public List<Day> CalculateWeek(DateTime StartDate)
        {
            // Set StartDate to the most recent Sunday
            int daysToSunday = (int)StartDate.DayOfWeek; // Sunday is DayOfWeek 0
            StartDate = StartDate.AddDays(-daysToSunday); // Go back to the previous Sunday

            List<Day> daysOfTheWeek = new List<Day>(); // Temporary storage for the days of the week
            DateTime today = DateTime.Today; // Get the current date

            for (int CurrDay = 0; CurrDay < 7; CurrDay++) // Iterate through each day in the week
            {
                DateTime NewDate = StartDate.AddDays(CurrDay);
                Day NewDay = new Day(Date.DaysOfWeek[(int)NewDate.DayOfWeek], NewDate.Day); // Create a new Day object
                daysOfTheWeek.Add(NewDay); // Add the day to the list

                // Automatically select today’s date
                if (NewDate.Date == today)
                {
                    NewDay.IsSelected = true; // Select the current date
                    OnNewDaySelected?.Invoke(NewDate); // Trigger the selection event
                }

                // Set up the event for selecting a day
                NewDay.IsSelectedEvent += (NewSelectedDay) =>
                {
                    foreach (Day day in Week) // Loop through each day in the week
                    {
                        if (!day.Equals(NewSelectedDay) && day.IsSelected) // If it's not the selected day, deselect it
                        {
                            day.IsSelected = false; // Deselect
                        }
                    }
                    OnNewDaySelected?.Invoke(new DateTime(Year, Month, NewDay.DayNumber));
                };
            }
            return daysOfTheWeek;
        }
    }
}