/*
    Name: Luke Kastern
    Date: 10/14/2024
    Description: AthletePastForms Screen
    Bugs: None Known
    Reflection: This was fairly easy because I had AthleteStatus as a reference, 
                but it was hard to get the form number to be the only element on the right side.
*/

using System.Collections.ObjectModel;
using RecoveryAT.ViewModels; // Import required namespace for ObservableCollection

namespace RecoveryAT
{
    // AthletePastForms class inherits from FlyoutPage to implement a page with a flyout menu
    public partial class AthletePastForms : FlyoutPage
    {
        // Constructor to initialize the AthletePastForms page
        public AthletePastForms()
        {
            InitializeComponent(); // Initialize XAML components
            // Set the BindingContext to the current instance to allow XAML data binding with FormList
            this.BindingContext = new AthletePastFormsViewModel();
        }
    }
}
