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
using RecoveryAT.ViewModels;

namespace RecoveryAT
{
    public partial class AthleteStatuses : FlyoutPage
    {
        // Constructor for the page
        public AthleteStatuses()
        {
            InitializeComponent();
            // Set the page's BindingContext to this class, so the XAML can access AthleteList
            this.BindingContext = new AthleteStatusesViewModel();
        }
    }    

}
