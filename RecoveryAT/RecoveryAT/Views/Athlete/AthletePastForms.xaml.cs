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
                new Form { Date = "10/01/2024", Name = "John Smith", Sport = "Soccer", Injury = "Ankle", TreatmentType = "Tape" },
                new Form { Date = "09/22/2024", Name = "Reece Thomas", Sport = "Tennis", Injury = "Shoulder", TreatmentType = "Eval" },
                new Form { Date = "09/25/2024", Name = "Marcus Rye", Sport = "Football", Injury = "Knee", TreatmentType = "Eval" },
                new Form { Date = "08/30/2024", Name = "Sophia Lee", Sport = "Basketball", Injury = "Wrist", TreatmentType = "Tape" },
                new Form { Date = "08/10/2024", Name = "Lucas Brown", Sport = "Soccer", Injury = "Back", TreatmentType = "Eval" }
            };

            // Set the BindingContext to the current instance to allow XAML data binding with FormList
            this.BindingContext = this;
        }

        private async void OnTileTapped(object sender, EventArgs e)
        {
            var frame = (Frame)sender;
            var tappedItem = frame.BindingContext; // get the tapped item information

            Form currForm = (Form)tappedItem; // for testing, remove later
            AthleteForm selectedAthlete = new AthleteForm(currForm.Name.Split(" ")[0], currForm.Name.Split(" ")[1], currForm.Sport, currForm.Injury, "stat"); // this should retrieve from the database instead, fix later
            await Navigation.PushAsync(new AthleteFormInformation(selectedAthlete)); // navigate to athlete form information on tapped
        }

    }

    // The Form class defines the structure for each form displayed in the CollectionView
    public class Form
    {
        public string Date { get; set; } // Date when the form was created
        public string Name { get; set; } // Athlete's name
        public string Sport { get; set; } // Sport the athlete plays
        public string Injury { get; set; } // Type of injury the athlete has
        public string TreatmentType { get; set; } // Treatment type 
    }
}
