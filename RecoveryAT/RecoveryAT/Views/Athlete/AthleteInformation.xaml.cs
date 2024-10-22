/*
    Name: Luke Kastern
    Date: 10/14/2024
    Description: AthleteInformation Screen with Flyout Menu
    Bugs: Search bar and bottom nav bar currently don’t do anything.
    Reflection: I was able to use my AthletePastForms screen as the base and then add the flyout screen. 
                It was hard for me to implement, but my group was able to help.
*/

using Microsoft.Maui.Controls; // Import necessary namespaces for MAUI controls
using RecoveryAT.ViewModels;
using System.Collections.ObjectModel; // Import for ObservableCollection

namespace RecoveryAT
{
    // AthleteInformation class inherits from FlyoutPage to create a page with a flyout menu
    public partial class AthleteInformation : FlyoutPage
    {

        // Constructor for AthleteInformation
        public AthleteInformation()
        {
            InitializeComponent(); // Initializes the XAML components
            BindingContext = new AthleteInformationViewModel();
        }
    }
}
