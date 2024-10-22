/*
    Name: Luke Kastern
    Date: 10/14/2024
    Description: TrainerSchoolInformation Screen
    Bugs: None known
    Reflection: This was a very easy screen to implement. The only challenge was aligning the 5 boxes 
                for the 5-digit code to make it look nice and closer to the prototype.
*/

namespace RecoveryAT
{
    // This class represents the TrainerSchoolInformation page in the application
    public partial class TrainerSchoolInformation : ContentPage
    {
        // Constructor that initializes the page components
        public TrainerSchoolInformation()
        {
            InitializeComponent(); // Load the XAML-defined components for this page
        }

        // Event handler when the Create School button is clicked
        private async void OnCreateSchoolClicked(object sender, EventArgs e)
        {
            // Navigate to the trainer home screen page
            await Navigation.PushAsync(new TrainerHomeScreen());
        }
    }
}
