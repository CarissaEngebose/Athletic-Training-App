/*
    Date: 12/10/2024
    Description: AthleteFormInformationEdit screen
    Bugs: None known
    Reflection: This was one of the easier screens. The information box was the hardest to get 
                to look right, but it wasn't too bad. 
                I also just had to change the comment boxes to be editable and add a save changes button.
*/

using System.Text.RegularExpressions;

namespace RecoveryAT;

public partial class AthleteFormInformationEdit : ContentPage
{
    private readonly IBusinessLogic _businessLogic;
    private AthleteForm _currentForm;
    private List<AthleteContact> _currentContacts;

    public AthleteFormInformationEdit(AthleteForm form)
    {
        InitializeComponent();
        _businessLogic = new BusinessLogic(
                         new ContactsDatabase(),
                         new FormsDatabase(),
                         new UsersDatabase(),
                         new SearchDatabase(),
                         new Database()); // Initialize BusinessLogic internally
        _currentForm = form;

        // Set initial values for editable fields
        BindingContext = _currentForm;
        TreatmentType.SelectedItem = _currentForm.TreatmentType;
        StatusPicker.SelectedItem = _currentForm.Status;

        // Load contact information
        LoadContactInformation();
    }

    private void LoadContactInformation()
    {
        if (_currentForm.FormKey.HasValue)
        {
            // Fetch contacts using BusinessLogic
            _currentContacts = _businessLogic.SelectContactsByFormKey(_currentForm.FormKey.Value).ToList();

            // Assuming we want to display the first contact, if it exists
            if (_currentContacts.Count > 0)
            {
                var primaryContact = _currentContacts[0];

                // Set the current values as placeholders
                ContactTypeEntry.Text = primaryContact.ContactType;
                PhoneNumberEntry.Text = primaryContact.PhoneNumber;
            }
        }
    }

    public async void OnSaveChangesClicked(object sender, EventArgs e)
    {
        // Retrieve and validate phone number
        var phoneNumber = PhoneNumberEntry.Text;
        if (!string.IsNullOrEmpty(phoneNumber) && !IsValidPhoneNumber(phoneNumber))
        {
            await DisplayAlert("Invalid Input", "Please enter a valid phone number.", "OK");
            return;
        }

        // Update form details
        _currentForm.TreatmentType = (string)TreatmentType.SelectedItem;
        _currentForm.Status = (string)StatusPicker.SelectedItem;

        // Update or insert contact details
        if (_currentContacts.Count > 0)
        {
            // Update existing contact
            _currentContacts[0].ContactType = string.IsNullOrWhiteSpace(ContactTypeEntry.Text)
                ? _currentContacts[0].ContactType
                : ContactTypeEntry.Text;

            _currentContacts[0].PhoneNumber = string.IsNullOrWhiteSpace(PhoneNumberEntry.Text)
                ? _currentContacts[0].PhoneNumber
                : phoneNumber;
        }
        else
        {
            // Insert new contact
            var newContact = new AthleteContact(
                contactID: 0, // The database should auto-generate this ID
                formKey: _currentForm.FormKey.Value,
                contactType: ContactTypeEntry.Text ?? "Unknown",
                phoneNumber: phoneNumber
            );

            var insertResult = _businessLogic.InsertContact(newContact.FormKey, newContact.ContactType, newContact.PhoneNumber);

            if (!insertResult.Contains("successfully"))
            {
                await DisplayAlert("Error", "Failed to add new contact. Please try again.", "OK");
                return;
            }

            // Add the new contact to the current contacts list
            _currentContacts.Add(newContact);
        }

        // Save changes to the form
        var resultMessage = _businessLogic.SaveUpdatedForm(_currentForm, _currentContacts);

        await DisplayAlert("Save Status", resultMessage, "OK");

        if (resultMessage.Contains("successfully"))
        {
            await Navigation.PopAsync(); // Go back to the main page
        }
    }

    // Helper method to validate phone number format
    private bool IsValidPhoneNumber(string phoneNumber)
    {
        var phonePattern = @"^\d{3}-\d{3}-\d{4}$";
        return Regex.IsMatch(phoneNumber, phonePattern);
    }
}
