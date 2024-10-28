/*
    Name: Hannah Hotchkiss
    Date: 10/14/2024
    Description: AthleteContacts - This page allows users to add contact information for athletes, 
                 including their phone number and relationship to the athlete.
    Bugs: None Known
    Reflection: This screen was fairly easy as there isn't much going on. 
                The hardest part was getting the image of the garbage can for the button 
                to be exactly where I wanted it inside the frame.
*/

namespace RecoveryAT  // Defines the namespace for the class, grouping related code logically.
{
    
    // AthleteContacts class inherits from ContentPage, which provides page-related functionality in MAUI.
    public partial class AthleteContacts : ContentPage {

        bool _isTrainer = false; // see if user is logged in (AKA if they're a trainer)

        // Constructor for AthleteContacts, which is called when an instance of this class is created.
        public AthleteContacts()
        {
            InitializeComponent(); // Initializes the UI components defined in the XAML file.
        }

        // Event handler when the Delete button is clicked
        private void OnDeleteClicked(object sender, EventArgs e)
        {
            // delete contact from screen
        }

        // Event handler when the finish button is clicked
        private async void OnFinishClicked(object sender, EventArgs e)
        {
            // Navigate to the welcome screen if athlete or home screen if trainer
            if(_isTrainer) {
                await Navigation.PushModalAsync(new TrainerHomeScreen());
            }
            else {
                await Navigation.PushModalAsync(new WelcomeScreen());
            }
        }
    }
}
