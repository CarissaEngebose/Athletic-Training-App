/*
    Name: Luke Kastern
    Date: 10/14/2024
    Description: AthleteInformation Screen with Flyout Menu
    Bugs: Search bar and bottom nav bar currently don’t do anything.
    Reflection: I was able to use my AthletePastForms screen as the base and then add the flyout screen. 
                It was hard for me to implement, but my group was able to help.
*/

using Microsoft.Maui.Controls; // Import necessary namespaces for MAUI controls
using System.Collections.ObjectModel; // Import for ObservableCollection

namespace RecoveryAT
{
    // AthleteInformation class inherits from FlyoutPage to create a page with a flyout menu
    public partial class AthleteInformation : FlyoutPage
    {
        // Public property to store the list of athletes, used for data binding with the CollectionView
        public ObservableCollection<Athlete> AthleteList { get; set; }

        // Constructor for AthleteInformation
        public AthleteInformation()
        {
            InitializeComponent(); // Initializes the XAML components

            // Initialize the AthleteList with sample data
            AthleteList = new ObservableCollection<Athlete>
            {
                new Athlete { Date = "2024-10-01", Name = "John Smith", Relationship = "Mother", PhoneNumber = "(123) 456-7890", FormNumber = "12" },
                new Athlete { Date = "2024-09-22", Name = "Reece Thomas", Relationship = "Mother", PhoneNumber = "(555) 111-2222", FormNumber = "11" },
                new Athlete { Date = "2024-09-15", Name = "Marcus Rye", Relationship = "Guardian", PhoneNumber = "(111) 222-3333", FormNumber = "9" }
            };

            BindingContext = this; // Set the BindingContext to the current instance for data binding
        }

        // Event handler to display the flyout menu when a menu button is clicked
        private void OnMenuButtonClicked(object sender, EventArgs e)
        {
            IsPresented = true; // Opens the flyout menu
        }
    }
}
