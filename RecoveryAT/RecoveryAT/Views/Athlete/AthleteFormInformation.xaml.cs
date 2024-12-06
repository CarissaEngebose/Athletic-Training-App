using System.ComponentModel;

namespace RecoveryAT;

public partial class AthleteFormInformation : ContentPage
{
    private AthleteForm athleteForm;
    private readonly IBusinessLogic _businessLogic;

    public AthleteFormInformation(AthleteForm athleteForm)
    {
        InitializeComponent();
        this.athleteForm = athleteForm;

        // Initialize the BusinessLogic instance internally
        _businessLogic = new BusinessLogic(
                         new ContactsDatabase(),
                         new FormsDatabase(),
                         new UsersDatabase(),
                         new SearchDatabase(),
                         new Database());


        BindingContext = athleteForm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Refresh contact information and set default values if needed
        LoadContactInformation();
        
        // Refresh the fields that may have been updated in the edit view
        if (BindingContext is AthleteForm currentForm)
        {
            // Update the Status label
            StatusLabel.Text = string.IsNullOrWhiteSpace(currentForm.Status) ? "No Status" : currentForm.Status;

            // Update the TreatmentType label
            TreatmentTypeLabel.Text = string.IsNullOrWhiteSpace(currentForm.TreatmentType) ? "No Treatment Type" : currentForm.TreatmentType;

            // Update the AthleteComments label
            AthleteCommentsLabel.Text = string.IsNullOrWhiteSpace(currentForm.AthleteComments) ? "No Comments" : currentForm.AthleteComments;
        }
    }

    private void LoadContactInformation()
    {
        if (athleteForm.FormKey.HasValue)
        {
            // Fetch contacts using BusinessLogic
            var contacts = _businessLogic.SelectContactsByFormKey(athleteForm.FormKey.Value);

            // Assuming we want to display the first contact, if it exists
            if (contacts.Any())
            {
                var primaryContact = contacts.First();

                // Set contact data or default messages if fields are empty
                ContactTypeLabel.Text = string.IsNullOrWhiteSpace(primaryContact.ContactType)
                    ? "No Contact Type"
                    : primaryContact.ContactType;

                ContactPhoneNumberLabel.Text = string.IsNullOrWhiteSpace(primaryContact.PhoneNumber)
                    ? "No Phone Number"
                    : primaryContact.PhoneNumber;
            }
            else
            {
                // If no contact data is found, set default messages
                ContactTypeLabel.Text = "No Contact Type";
                ContactPhoneNumberLabel.Text = "No Phone Number";
            }
        }
        else
        {
            // If FormKey is null, set default messages
            ContactTypeLabel.Text = "No Contact Type";
            ContactPhoneNumberLabel.Text = "No Phone Number";
        }
    }

    public async void OnEditClicked(object sender, EventArgs e)
    {
        if (athleteForm.FormKey.HasValue)
        {
            // Navigate to the edit page
            await Navigation.PushAsync(new AthleteFormInformationEdit(athleteForm));
        }
        else
        {
            // Handle the case where FormKey is null (e.g., show an alert or log the issue)
            await DisplayAlert("Error", "FormKey is not available.", "OK");
        }
    }
}
