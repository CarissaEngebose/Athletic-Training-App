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

namespace RecoveryAT
{
    public partial class TrainerHomeScreen : ContentPage
    {
        public TrainerHomeScreenViewModel ViewModel;
        public TrainerHomeScreen(BusinessLogic businessLogic, String SchoolCode)
        {
            InitializeComponent();
            ViewModel = new TrainerHomeScreenViewModel(businessLogic, SchoolCode);
            BindingContext = ViewModel;
        }
        
        public void OnDayTapped(object sender, EventArgs e){
            Frame DayElement = (Frame)sender; // get the xaml Frame 
            CalendarViewModel.Day SelectedDay = (CalendarViewModel.Day)DayElement.BindingContext; // convert to day object
            SelectedDay.IsSelected = true;
        }

        public async void OnFrameTapped(object sender, EventArgs e)
        {
            Frame AthleteFormFrame = (Frame)sender; // get XAML frame
            AthleteForm SelectedAthleteForm = (AthleteForm)AthleteFormFrame.BindingContext; // get seleceted athlete form from clicked frame
            await Navigation.PushAsync(new AthleteFormInformation(SelectedAthleteForm)); // send to AthleteFormInformation view to display
        }
    }
}