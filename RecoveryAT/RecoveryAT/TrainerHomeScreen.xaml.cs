/*
    Name: Dominick Hagedorn
    Date: 10/14/2024
    Description: TrainerHomeScreen
    Bugs: None Known
    Reflection: I struggled with this one quite a bit. I really couldn't figure out how to do the date picker 
                in a way that looked good. After asking for help, my group was able to get a working scrollable calendar.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace RecoveryAT
{
    // The main page for the trainer's home screen
    public partial class TrainerHomeScreen : ContentPage
    {
        public CalendarViewModel ViewModel { get; }

        public TrainerHomeScreen()
        {
            InitializeComponent();

            // Initialize the ViewModel and bind it to the page's BindingContext
            ViewModel = new CalendarViewModel();
            BindingContext = ViewModel;

            // Load the calendar data for the current year
            ViewModel.LoadCalendarData(DateTime.Now.Year);
        }
    }

    // ViewModel to manage the calendar's data and state
    public class CalendarViewModel : INotifyPropertyChanged
    {
        private int _currentYear;
        private CalendarDay _selectedDay;

        // Observable collection to group days by month
        public ObservableCollection<MonthGroup> CalendarData { get; } = new ObservableCollection<MonthGroup>();

        // Property to get and set the current year, with logic to reload data on change
        public int CurrentYear
        {
            get => _currentYear;
            set
            {
                if (_currentYear != value)
                {
                    _currentYear = value;
                    OnPropertyChanged(nameof(CurrentYear));
                    LoadCalendarData(_currentYear); // Reload calendar for the new year
                }
            }
        }

        // Property to manage the selected day
        public CalendarDay SelectedDay
        {
            get => _selectedDay;
            set
            {
                _selectedDay = value;
                OnPropertyChanged(nameof(SelectedDay));
            }
        }

        // Command to decrement the year
        public ICommand PreviousYearCommand => new Command(() => CurrentYear--);

        // Command to increment the year
        public ICommand NextYearCommand => new Command(() => CurrentYear++);

        // Constructor initializes the year to the current year
        public CalendarViewModel()
        {
            CurrentYear = DateTime.Now.Year;
        }

        // Method to load calendar data for a given year
        public void LoadCalendarData(int year)
        {
            CalendarData.Clear(); // Clear existing data

            // Loop through each month of the year
            for (int month = 1; month <= 12; month++)
            {
                var monthGroup = new MonthGroup
                {
                    Name = $"{new DateTime(year, month, 1):MMMM yyyy}" // e.g., "January 2024"
                };

                // Add days for the current month
                int daysInMonth = DateTime.DaysInMonth(year, month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    var date = new DateTime(year, month, day);
                    monthGroup.Add(new CalendarDay
                    {
                        DayOfWeek = date.ToString("ddd"), // e.g., "Mon"
                        Day = day,
                        FullDate = date
                    });
                }

                CalendarData.Add(monthGroup); // Add month group to the calendar data
            }

            // Notify that the CalendarData has changed to update the view
            OnPropertyChanged(nameof(CalendarData));
        }

        // INotifyPropertyChanged event to notify the view of property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Helper method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Class representing a single day in the calendar
    public class CalendarDay
    {
        public string DayOfWeek { get; set; } // e.g., "Mon"
        public int Day { get; set; } // Day of the month (e.g., 1)
        public DateTime FullDate { get; set; } // Full date for the day

        // Display the full date in a readable format
        public string DateDisplay => FullDate.ToString("dddd, MMMM d, yyyy");
    }

    // Collection of CalendarDay objects grouped by month
    public class MonthGroup : ObservableCollection<CalendarDay>
    {
        public string Name { get; set; } // Name of the month (e.g., "January 2024")
    }
}
