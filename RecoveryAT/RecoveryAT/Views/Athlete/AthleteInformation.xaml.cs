/*
    Date: 12/10/2024
    Description: AthleteInformation screen, useful for searching for athletes and viewing their information.
    Bugs: None known
    Reflection: Displaying information was easy enough but getting it to work with pulling up form infor was (initially) challenging.
*/


using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace RecoveryAT;

public partial class AthleteInformation : ContentPage, INotifyPropertyChanged
{
    private readonly IBusinessLogic _businessLogic;
    private ObservableCollection<AthleteDetail> _displayList = [];
    private readonly ObservableCollection<AthleteDetail> _allItems = [];
    public new event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<AthleteDetail> DisplayList
    {
        get => _displayList;
        set
        {
            _displayList = value;
            OnPropertyChanged(nameof(DisplayList));
        }
    }

    public string SearchQuery { get; set; }

    private string SchoolCode { get; set; }

    public AthleteInformation()
    {
        InitializeComponent();
        _businessLogic = new BusinessLogic(
                         new ContactsDatabase(),
                         new FormsDatabase(),
                         new UsersDatabase(),
                         new SearchDatabase(),
                         new Database());

        SearchQuery = string.Empty;
        SchoolCode = string.Empty;

        LoadSchoolCode();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadSchoolCode(); // Refresh SchoolCode when the page appears
        LoadData(); // Refresh data when the page appears
    }

    private new void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void LoadSchoolCode()
    {
        // Retrieve SchoolCode dynamically from the user's profile
        var user = ((App)Microsoft.Maui.Controls.Application.Current).User;
        string email = user?.Email ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(email))
        {
            if (user != null)
            {
                SchoolCode = user?.SchoolCode ?? "DefaultCode";
            }
            else
            {
                SchoolCode = "DefaultCode"; // Fallback value in case of error
            }
        }
        else
        {
            SchoolCode = "DefaultCode"; // Fallback value if email is unavailable
        }
    }

    private void LoadData()
    {
        try
        {
            _allItems.Clear();

            var athleteForms = _businessLogic.GetForms(schoolCode: SchoolCode) ?? new ObservableCollection<AthleteForm>();

            foreach (var form in athleteForms)
            {
                form.AthleteComments = string.IsNullOrWhiteSpace(form.AthleteComments) ? "No Comments" : form.AthleteComments;

                var contacts = _businessLogic.GetContactsByFormKey(form.FormKey ?? 0) ?? new ObservableCollection<AthleteContact>();

                bool detailsAdded = false;

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

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            DisplayList = [.. _allItems];
        }
        else
        {
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

    private async void OnTileTapped(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var tappedItem = frame.BindingContext as AthleteDetail;

        if (tappedItem != null)
        {
            var athleteForms = _businessLogic.GetForms(schoolCode: SchoolCode);
            var athleteForm = athleteForms?.FirstOrDefault(form => form.FullName == tappedItem.FullName);

            if (athleteForm != null)
            {
                await Navigation.PushAsync(new AthleteFormInformation(athleteForm));
            }
            else
            {
                await DisplayAlert("Error", "Athlete information not found.", "OK");
            }
        }
    }
}
