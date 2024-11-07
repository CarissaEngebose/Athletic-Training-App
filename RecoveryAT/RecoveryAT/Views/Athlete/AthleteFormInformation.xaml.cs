/*
    Name: Dominick Hagedorn
    Date: 10/14/2024
    Description: AthleteFormInformation screen
    Bugs: None known
    Reflection: This was one of the easier screens. The information box was the hardest 
                to get to look right, but it wasn't too bad.
*/

namespace RecoveryAT;  // Defines the namespace, grouping related code for the RecoveryAT app.

public partial class AthleteFormInformation : ContentPage
{    
    AthleteForm athleteForm;

    // Constructor for AthleteFormInformation, called when an instance of this class is created.
    public AthleteFormInformation(AthleteForm athleteForm)
    {
        InitializeComponent(); // This method initializes the XAML components for this screen.
        this.athleteForm = athleteForm;
        BindingContext = athleteForm;
    }

    public async void OnEditClicked(object sender, EventArgs e) {
        await Navigation.PushAsync(new AthleteFormInformationEdit());
    }
}
