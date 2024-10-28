/*
    Name: Luke Kastern
    Date: 10/14/2024
    Description: AthletePastForms Screen
    Bugs: None Known
    Reflection: This was fairly easy because I had AthleteStatus as a reference, 
                but it was hard to get the form number to be the only element on the right side.
*/

using System.Collections.ObjectModel; // Import required namespace for ObservableCollection

namespace RecoveryAT
{
    // AthletePastForms class inherits from FlyoutPage to implement a page with a flyout menu
    public partial class AthletePastForms : FlyoutPage
    {

        // ObservableCollection to store a list of forms for data binding with the CollectionView
        public ObservableCollection<Form> FormList { get; set; }

        // Constructor to initialize the AthletePastForms page
        public AthletePastForms()
        {
            InitializeComponent(); // Initialize XAML components

            // Populate the FormList with sample data to simulate database entries
            FormList = new ObservableCollection<Form>
            {
                new Form { Date = "2024-10-01", Name = "John Smith", Sport = "Soccer", Injury = "Ankle", FormNumber = "F001" },
                new Form { Date = "2024-09-22", Name = "Reece Thomas", Sport = "Tennis", Injury = "Shoulder", FormNumber = "F002" },
                new Form { Date = "2024-09-15", Name = "Marcus Rye", Sport = "Football", Injury = "Knee", FormNumber = "F003" },
                new Form { Date = "2024-08-30", Name = "Sophia Lee", Sport = "Basketball", Injury = "Wrist", FormNumber = "F004" },
                new Form { Date = "2024-08-10", Name = "Lucas Brown", Sport = "Soccer", Injury = "Back", FormNumber = "F005" }
            };

            // Set the BindingContext to the current instance to allow XAML data binding with FormList
            this.BindingContext = this;
        }
    }

    // The Form class defines the structure for each form displayed in the CollectionView
    public class Form
    {
        public string Date { get; set; } // Date when the form was created
        public string Name { get; set; } // Athlete's name
        public string Sport { get; set; } // Sport the athlete plays
        public string Injury { get; set; } // Type of injury the athlete has
        public string FormNumber { get; set; } // Unique identifier for the form
    }
}
