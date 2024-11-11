/*
    Name: Dominick Hagedorn
    Date: 10/14/2024
    Description: AthleteFormInformationEdit screen
    Bugs: None known
    Reflection: This was one of the easier screens. The information box was the hardest to get 
                to look right, but it wasn't too bad. 
                I also just had to change the comment boxes to be editable and add a save changes button.
*/

namespace RecoveryAT;  // Defines the namespace for the application, organizing related code logically.

public partial class AthleteFormInformationEdit : ContentPage
{
    // Constructor for AthleteFormInformationEdit, invoked when this class is instantiated.
    public AthleteFormInformationEdit()
    {
        InitializeComponent(); // Loads and initializes the UI components defined in the XAML file.
    }

    public async void OnSaveChangesClicked(object sender, EventArgs e) { // when done editing
        await Navigation.PopAsync(); // go back to main page 
    }
}
