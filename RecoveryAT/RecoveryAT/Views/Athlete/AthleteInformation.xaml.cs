/*
    Date: 12/10/2024
    Description: AthleteInformation screen, useful for searching for athletes and viewing their information.
    Bugs: None known
    Reflection: Displaying information was easy enough but getting it to work with pulling up form info was (initially) challenging.
*/

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace RecoveryAT;

public partial class AthleteInformation : ContentPage, INotifyPropertyChanged
{
    // Business logic layer instance for accessing and modifying application data
    private readonly IBusinessLogic _businessLogic;

    // Observable collection for displaying filtered athlete details
    private ObservableCollection<AthleteDetail> _displayList = [];

    // All athlete details, used as the source for filtering
    private readonly ObservableCollection<AthleteDetail> _allItems = [];

    // Event for notifying the UI when a property changes
    public new event PropertyChangedEventHandler? PropertyChanged;

    // Public property for the filtered display list, bound to the UI
    public ObservableCollection<AthleteDetail> DisplayList
    {
        get => _displayList;
        set
        {
            _displayList = value;
            OnPropertyChanged(nameof(DisplayList));
        }
    }

    // Search query entered by the user
    public string SearchQuery { get; set; }

    // School code for filtering data based on the current user's organization
    private string SchoolCode { get; set; }

    /// <summary>
    /// Constructor for initializing the AthleteInformation screen.
    /// </summary>
    public AthleteInformation()
    {
        InitializeComponent();

        // Initialize the business logic layer
        _businessLogic = new BusinessLogic(
            new ContactsDatabase(),
            new FormsDatabase(),
            new UsersDatabase(),
            new SearchDatabase(),
            new Database());

        // Initialize default values
        SearchQuery = string.Empty;
        SchoolCode = string.Empty;

        // Load the school code and set up data binding
        LoadSchoolCode();
        BindingContext = this;
    }

    /// <summary>
    /// Event triggered when the page is displayed.
    /// Refreshes the school code and reloads athlete data.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadSchoolCode(); // Refresh SchoolCode when the page appears
        LoadData(); // Refresh data when the page appears
    }

    /// <summary>
    /// Notifies the UI of property changes.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    private new void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Loads the school code dynamically based on the current user's profile.
    /// </summary>
    private void LoadSchoolCode()
    {
        // Retrieve the current user from the application instance
        var user = ((App)Microsoft.Maui.Controls.Application.Current).User;
        string email = user?.Email ?? string.Empty;

        // Assign SchoolCode based on the user's profile, with a default fallback
        if (!string.IsNullOrWhiteSpace(email))
        {
            SchoolCode = user?.SchoolCode ?? "DefaultCode";
        }
        else
        {
            SchoolCode = "DefaultCode"; // Default SchoolCode if no email is available
        }
    }

    /// <summary>
    /// Loads athlete data from the business logic layer and populates the display list.
    /// </summary>
    private void LoadData()
    {
        try
        {
            // Clear the existing items
            _allItems.Clear();

            // Fetch athlete forms associated with the current SchoolCode
            var athleteForms = _businessLogic.GetForms(schoolCode: SchoolCode) ?? new ObservableCollection<AthleteForm>();

            // Process each athlete form
            foreach (var form in athleteForms)
            {
                // Ensure comments are not null or empty
                form.AthleteComments = string.IsNullOrWhiteSpace(form.AthleteComments) ? "No Comments" : form.AthleteComments;

                // Fetch contacts associated with the current form
                var contacts = _businessLogic.GetContactsByFormKey(form.FormKey ?? 0) ?? new ObservableCollection<AthleteContact>();

                bool detailsAdded = false;

                // Create AthleteDetail objects for each contact
                foreach (var contact in contacts)
                {
                    var detail = AthleteDetail.FromFormAndContact(form, contact);
                    if (detail != null && !string.IsNullOrWhiteSpace(detail.FullName))
                    {
                        _allItems.Add(detail);
                        detailsAdded = true;
                    }
                    else
                    {
                        Debug.WriteLine($"Skipped invalid detail for contact: {contact.ContactType}, Phone={contact.PhoneNumber}");
                    }
                }

                // If no contacts exist, create a placeholder detail
                if (!detailsAdded)
                {
                    var detail = new AthleteDetail(
                        fullName: form.FullName,
                        relationship: "No Contact Information",
                        phoneNumber: string.Empty,
                        treatmentType: form.TreatmentType,
                        athleteComments: form.AthleteComments,
                        dateOfBirth: form.DateOfBirth
                    );
                    _allItems.Add(detail);
                }
            }

            // Update the display list on the main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DisplayList = [.. _allItems];
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading data: {ex.Message}");
        }
    }

    /// <summary>
    /// Handles text changes in the search bar and filters the display list.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">TextChangedEventArgs containing the new search text.</param>
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            // Reset the display list if the search query is empty
            DisplayList = [.. _allItems];
        }
        else
        {
            // Filter the display list based on the search query
            var query = e.NewTextValue.ToLowerInvariant();
            var filteredItems = _allItems.Where(item =>
                (item.FullName?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                (item.Relationship?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                (item.PhoneNumber?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                (item.TreatmentType?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                (item.AthleteComments?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                (item.DateOfBirth.ToString("MM/dd/yyyy")?.Contains(query, StringComparison.OrdinalIgnoreCase) == true)
            );

            DisplayList = [.. filteredItems];
        }
    }

    /// <summary>
    /// Handles tapping on a tile to view detailed information about an athlete.
    /// </summary>
    /// <param name="sender">The object that triggered the event.</param>
    /// <param name="e">Event arguments.</param>
    private async void OnTileTapped(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var tappedItem = frame.BindingContext as AthleteDetail;

        if (tappedItem != null)
        {
            // Find the athlete form matching the tapped item's full name
            var athleteForms = _businessLogic.GetForms(schoolCode: SchoolCode);
            var athleteForm = athleteForms?.FirstOrDefault(form => form.FullName == tappedItem.FullName);

            if (athleteForm != null)
            {
                // Navigate to the AthleteFormInformation screen
                await Navigation.PushAsync(new AthleteFormInformation(athleteForm));
            }
            else
            {
                // Display an error if the athlete form is not found
                await DisplayAlert("Error", "Athlete information not found.", "OK");
            }
        }
    }
}
