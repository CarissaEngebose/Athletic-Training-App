/*
    Name: Dominick Hagedorn
    Date: 10/14/2024
    Description: TrainerHomeScreen
    Bugs: None Known
    Reflection: I struggled with this one quite a bit. I really couldn't figure out how to do the date picker 
                in a way that looked good. After asking for help, my group was able to get a working scrollable calendar.
*/

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace RecoveryAT
{
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
        
        public async void OnFrameTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AthleteFormInformation(new AthleteForm("First", "Last","Sport","Inj","stat")));
        }
    }

    // ViewModel to manage the calendar's data and state
    public class CalendarViewModel : INotifyPropertyChanged
    {
        private int _currentYear;
        private CalendarDay _selectedDay;

        public ObservableCollection<MonthGroup> CalendarData { get; } = new ObservableCollection<MonthGroup>();

        // Property to get and set the current year
        public int CurrentYear
        {
            get => _currentYear;
            set
            {
                if (_currentYear != value)
                {
                    _currentYear = value;
                    OnPropertyChanged();
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
                if (_selectedDay != value)
                {
                    _selectedDay = value;
                    OnPropertyChanged();
                }
            }
        }

        // Command to handle day selection
        public ICommand DaySelectedCommand => new Command<CalendarDay>(OnDaySelected);

        // Command to decrement the year
        public ICommand PreviousYearCommand => new Command(() => CurrentYear--);

        // Command to increment the year
        public ICommand NextYearCommand => new Command(() => CurrentYear++);

        public CalendarViewModel()
        {
            CurrentYear = DateTime.Now.Year;
        }

        // Method to handle the day selection logic
        private void OnDaySelected(CalendarDay selectedDay)
        {
            if (selectedDay != null)
            {
                // Deselect all other days in the calendar
                foreach (var month in CalendarData)
                {
                    foreach (var day in month)
                    {
                        day.IsSelected = false; // Reset selection
                    }
                }

                // Select the new day
                selectedDay.IsSelected = true;
                SelectedDay = selectedDay;
            }
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
                    Name = $"{new DateTime(year, month, 1):MMMM yyyy}"
                };

                // Add days for the current month
                int daysInMonth = DateTime.DaysInMonth(year, month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    var date = new DateTime(year, month, day);
                    monthGroup.Add(new CalendarDay
                    {
                        DayOfWeek = date.ToString("ddd"),
                        Day = day,
                        FullDate = date
                    });
                }

                CalendarData.Add(monthGroup);
            }

            OnPropertyChanged(nameof(CalendarData));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Class representing a single day in the calendar
    public class CalendarDay : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string DayOfWeek { get; set; }
        public int Day { get; set; }
        public DateTime FullDate { get; set; }

        // Property to track whether the day is selected
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        // Display the full date in a readable format
        public string DateDisplay => FullDate.ToString("dddd, MMMM d yyyy");

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Collection of CalendarDay objects grouped by month
    public class MonthGroup : ObservableCollection<CalendarDay>
    {
        public string Name { get; set; }
    }
}
