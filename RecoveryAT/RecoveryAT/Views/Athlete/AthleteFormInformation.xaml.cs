/*
    Date: 10/14/2024
    Description: AthleteFormInformation screen
    Bugs: None known
    Reflection: This was a very easy screen since it is just displaying information
*/

namespace RecoveryAT;

public partial class AthleteFormInformation : ContentPage
{
    // The AthleteForm object representing the current form being displayed
    private AthleteForm athleteForm;

    // The business logic layer instance for accessing application functionality
    private readonly IBusinessLogic _businessLogic;

    /// <summary>
    /// Constructor for AthleteFormInformation screen.
    /// Initializes the screen with the athlete's form data and sets up the BusinessLogic instance.
    /// </summary>
    /// <param name="athleteForm">The form object containing the athlete's data.</param>
    public AthleteFormInformation(AthleteForm athleteForm)
    {
        InitializeComponent();

        // Store the athlete form locally for use in this class
        this.athleteForm = athleteForm;

        // Initialize the BusinessLogic instance for interacting with the database and other operations
        _businessLogic = new BusinessLogic(
                         new ContactsDatabase(),
                         new FormsDatabase(),
                         new UsersDatabase(),
                         new SearchDatabase(),
                         new Database());

        // Set the BindingContext to the athlete form for data binding in the UI
        BindingContext = athleteForm;
    }

    /// <summary>
    /// Event triggered when the page is displayed.
    /// Refreshes the contact information and updates any fields that may have been changed in the edit view.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Refresh and load the latest contact information
        LoadContactInformation();
        
        // Refresh form fields to ensure they reflect any updates
        if (BindingContext is AthleteForm currentForm)
        {
            // Update the Status label with the current status or a default message if null/empty
            StatusLabel.Text = string.IsNullOrWhiteSpace(currentForm.Status) ? "No Status" : currentForm.Status;

            // Update the TreatmentType label with the current treatment type or a default message if null/empty
            TreatmentTypeLabel.Text = string.IsNullOrWhiteSpace(currentForm.TreatmentType) ? "No Treatment Type" : currentForm.TreatmentType;

            // Update the AthleteComments label with the current comments or a default message if null/empty
            AthleteCommentsLabel.Text = string.IsNullOrWhiteSpace(currentForm.AthleteComments) ? "No Comments" : currentForm.AthleteComments;
        }
    }

    /// <summary>
    /// Loads the contact information for the current form and updates the UI labels.
    /// Handles cases where contact information is missing or incomplete.
    /// </summary>
    private void LoadContactInformation()
    {
        // Check if the FormKey exists for the athlete form
        if (athleteForm.FormKey.HasValue)
        {
            // Fetch contacts using the BusinessLogic layer
            var contacts = _businessLogic.SelectContactsByFormKey(athleteForm.FormKey.Value);

            // If contacts exist, display the primary contact's information
            if (contacts.Any())
            {
                var primaryContact = contacts.First();

                // Set contact type label or a default message if the field is empty
                ContactTypeLabel.Text = string.IsNullOrWhiteSpace(primaryContact.ContactType)
                    ? "No Contact Type"
                    : primaryContact.ContactType;

                // Set contact phone number label or a default message if the field is empty
                ContactPhoneNumberLabel.Text = string.IsNullOrWhiteSpace(primaryContact.PhoneNumber)
                    ? "No Phone Number"
                    : primaryContact.PhoneNumber;
            }
            else
            {
                // If no contacts exist, display default messages
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

    /// <summary>
    /// Event handler for the "Edit" button click.
    /// Navigates to the AthleteFormInformationEdit screen for editing the current form.
    /// Displays an alert if the FormKey is null.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">Event arguments.</param>
    public async void OnEditClicked(object sender, EventArgs e)
    {
        // Check if the FormKey exists before navigating to the edit screen
        if (athleteForm.FormKey.HasValue)
        {
            // Navigate to the edit page and pass the current athlete form
            await Navigation.PushAsync(new AthleteFormInformationEdit(athleteForm));
        }
        else
        {
            // Show an alert if the FormKey is not available
            await DisplayAlert("Error", "FormKey is not available.", "OK");
        }
    }
}
