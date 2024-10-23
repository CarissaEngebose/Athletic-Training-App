/*
    Name: Carissa Engebose
    Date: 10/9/2024
    Description: A screen that allows a user to enter information related to injuries that will 
                be sent back to their athletic trainer.
    Bugs: None Known
    Reflection: This screen took a little while to get the layout exactly how I wanted it, 
                but overall, I think it's good.
*/

using Microsoft.Maui.Controls; // Import necessary MAUI controls
using System; // Import system-level utilities

namespace RecoveryAT
{
    // The InjuryFormReport class inherits from ContentPage to represent a page in the MAUI app
    public partial class InjuryFormReport : ContentPage
    {
        // Constructor to initialize the InjuryFormReport page
        public InjuryFormReport()
        {
            InitializeComponent(); // Load the XAML components
            BindingContext = this; // Set the BindingContext for data binding with the UI
        }

        // Event handler for the Submit button click event
        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            // Logic to handle form submission, such as validation or sending data to the trainer
            // TO DO - NEED TO UPDATE TO CHECK IF EVAL IS CLICKED THEN ATHLETE CONTACTS OTHERWISE WELCOME SCREEN IF LOGGED OUT OR HOME SCREEN IF LOGGED IN
            await Navigation.PushAsync(new AthleteContacts()); // navigate to the athlete contacts 
        }
    }
}
