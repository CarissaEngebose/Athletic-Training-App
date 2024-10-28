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
        public ObservableCollection<AthleteForm> AthleteList { get; set; }

        // Constructor for the page
        public AthleteStatuses()
        {
            InitializeComponent();

            // Create some fake data to simulate what would come from a database
            AthleteList = new ObservableCollection<AthleteForm>
            {
                new AthleteForm("John", "Smith", "Soccer", "Ankle", "Total Rest"),
                new AthleteForm("Reece", "Thomas", "Tennis", "Shoulder", "Limited Contact"),
                new AthleteForm("Marcus", "Rye", "Football", "Knee", "Activity as Tolerated"),
                new AthleteForm("Sophia", "Lee", "Basketball", "Wrist", "Total Rest"),
                new AthleteForm("Lucas", "Brown", "Soccer", "Back", "Limited Contact")
            };


            // Set the page's BindingContext to this class, so the XAML can access AthleteList
            this.BindingContext = this;
        }
    }    

}
