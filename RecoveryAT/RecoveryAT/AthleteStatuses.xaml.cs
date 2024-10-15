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

namespace RecoveryAT
{
    public partial class AthleteStatuses : FlyoutPage
    {
        // The collection of athletes that will be displayed in the CollectionView
        public ObservableCollection<Athlete> AthleteList { get; set; }

        // Constructor for the page
        public AthleteStatuses()
        {
            InitializeComponent();

            // Create some fake data to simulate what would come from a database
            AthleteList = new ObservableCollection<Athlete>
            {
                new Athlete { Name = "John Smith", Injury = "Ankle", Sport = "Soccer", Status = "Total Rest" },
                new Athlete { Name = "Reece Thomas", Injury = "Shoulder", Sport = "Tennis", Status = "Limited Contact" },
                new Athlete { Name = "Marcus Rye", Injury = "Knee", Sport = "Football", Status = "Activity as Tolerated" },
                new Athlete { Name = "Sophia Lee", Injury = "Wrist", Sport = "Basketball", Status = "Total Rest" },
                new Athlete { Name = "Lucas Brown", Injury = "Back", Sport = "Soccer", Status = "Limited Contact" }
            };

            // Set the page's BindingContext to this class, so the XAML can access AthleteList
            this.BindingContext = this;
        }
    }    

}
