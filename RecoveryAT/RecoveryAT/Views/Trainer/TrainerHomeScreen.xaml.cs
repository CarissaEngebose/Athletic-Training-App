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
using System.Globalization;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using CalendarManagment;

namespace RecoveryAT
{
    public partial class TrainerHomeScreen : ContentPage
    {
        public TrainerHomeScreenViewModel ViewModel;
        public AuthenticationService authService;
        private string schoolCode;
        private IBusinessLogic _businessLogic;

        public TrainerHomeScreen()
        {
            InitializeComponent();
            authService = ((App)Application.Current).AuthService;
            schoolCode = authService.SchoolCode;
            _businessLogic = MauiProgram.BusinessLogic;
            ViewModel = new TrainerHomeScreenViewModel(_businessLogic, schoolCode);
            BindingContext = ViewModel;
            MonthCarousel.Position = ViewModel.Calendar.SelectedDate.Month - 1;
            YearCarousel.Position = ViewModel.Calendar.SelectedDate.Year-1950;
        }
        
        public void OnDayTapped(object sender, EventArgs e){
            Frame DayElement = (Frame)sender; // get the xaml Frame 
            Day SelectedDay = (Day)DayElement.BindingContext; // convert to day object
            SelectedDay.IsSelected = true;
        }

        public async void OnFrameTapped(object sender, EventArgs e)
        {
            Frame AthleteFormFrame = (Frame)sender; // get XAML frame
            AthleteForm SelectedAthleteForm = (AthleteForm)AthleteFormFrame.BindingContext; // get seleceted athlete form from clicked frame
            await Navigation.PushAsync(new AthleteFormInformation(SelectedAthleteForm)); // send to AthleteFormInformation view to display
        }

        public void MonthChanged(object sender, EventArgs e){
            ViewModel.Calendar.SetMonth(MonthCarousel.Position);
        }

        public void YearChanged(object sender, EventArgs e){
            ViewModel.Calendar.SetYear(YearCarousel.Position+1950);
        }
    }
}