/*
    Name: Hannah Hotchkiss
	Date: 10/14/2024
	Description: AthleteStatuses - This page displays a list of athletes along with their status, injury, and sport. 
    Users can select a status from a dropdown menu and search for athletes using a search bar.
	Bugs: None Known
    Reflection: This screen was a little more complicated because it's just a digital prototype so the binding for the 
    showing of example athletes and status took some time to figure out.
*/

using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace RecoveryAT
{
    public partial class AthleteStatuses : FlyoutPage
    {
        public ObservableCollection<AthleteForm> AthleteList { get; set; }

        public AthleteStatuses()
        {
            InitializeComponent();

            // Initialize the hardcoded data
            AthleteList = new ObservableCollection<AthleteForm>
            {
                new AthleteForm("001", "12345", "Hannah", "Smith", 12, "Basketball", "Knee", "Left", "Physical Therapy", 
                                new DateTime(2024, 10, 1), "Knee pain during practice", "Use knee brace", "Limited Contact"),
                new AthleteForm("002", "12345", "Jake", "Brown", 11, "Soccer", "Ankle", "Right", "Ice Pack",
                                new DateTime(2024, 10, 2), "Rolled ankle in game", "Ice daily", "Non-Contact"),
                new AthleteForm("003", "12345", "Mia", "Jones", 10, "Volleyball", "Shoulder", "Right", "Strength Training",
                                new DateTime(2024, 10, 3), "Shoulder strain from serving", "Rest for a week", "Full Contact")
            };

            // Set the BindingContext and notify the UI of data changes
            BindingContext = this;
            OnPropertyChanged(nameof(AthleteList));
        }
    }
}
