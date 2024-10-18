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
    public partial class AthleteContacts : ContentPage
    {
        // Constructor for AthleteContacts, which is called when an instance of this class is created.
        public AthleteContacts()
        {
            InitializeComponent(); // Initializes the UI components defined in the XAML file.
            // Any additional logic or event handlers can be added here if needed.
        }
    }
}
