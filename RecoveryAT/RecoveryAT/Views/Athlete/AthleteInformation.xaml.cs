using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace RecoveryAT;

public partial class AthleteInformation : FlyoutPage, INotifyPropertyChanged
{
    public ICommand NavigateToPastFormsCommand { get; }
    public ICommand NavigateToStatisticsCommand { get; }
    public ICommand NavigateToAthleteStatusesCommand { get; }
    public ICommand NavigateToHomeCommand { get; }

    private readonly IBusinessLogic _businessLogic;
    private ObservableCollection<AthleteDetail> _displayList = new();
    private ObservableCollection<AthleteDetail> _allItems = new();

    public event PropertyChangedEventHandler PropertyChanged;

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

    private readonly string SchoolCode = "THS24";

    public AthleteInformation()
    {
        InitializeComponent();
        _businessLogic = new BusinessLogic(new Database());

        LoadData();

        NavigateToHomeCommand = new Command(NavigateToHome);
        NavigateToPastFormsCommand = new Command(NavigateToPastForms);
        NavigateToStatisticsCommand = new Command(NavigateToStatistics);
        NavigateToAthleteStatusesCommand = new Command(NavigateToAthleteStatuses);

        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadData(); // Refresh data when the page appears
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void LoadData()
    {
        _allItems.Clear();
        _displayList.Clear();

        // Fetch forms from business logic
        var athleteForms = _businessLogic.GetForms(schoolCode: SchoolCode);
        if (athleteForms != null && athleteForms.Any())
        {
            foreach (var form in athleteForms)
            {
                // Ensure AthleteComments defaults to "No Comments" if null or empty
                form.AthleteComments = string.IsNullOrWhiteSpace(form.AthleteComments) ? "No Comments" : form.AthleteComments;

                // Fetch associated contacts
                var contacts = _businessLogic.GetContactsByFormKey(form.FormKey ?? 0);

                if (contacts.Any())
                {
                    foreach (var contact in contacts)
                    {
                        var detail = AthleteDetail.FromFormAndContact(form, contact);
                        _allItems.Add(detail);
                    }
                }
                else
                {
                    // Add form without contacts
                    var detail = new AthleteDetail(
                        fullName: form.FullName,
                        relationship: "No Contact",
                        phoneNumber: string.Empty, // No need to show phone number
                        treatmentType: form.TreatmentType,
                        athleteComments: form.AthleteComments,
                        dateOfBirth: form.DateOfBirth
                    );
                    _allItems.Add(detail);
                }
            }
        }

        // Update the DisplayList
        DisplayList = new ObservableCollection<AthleteDetail>(_allItems);
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            DisplayList = new ObservableCollection<AthleteDetail>(_allItems);
        }
        else
        {
            var query = e.NewTextValue.ToLowerInvariant(); // Case-insensitive search
            var filteredItems = _allItems.Where(item =>
                (item.FullName?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                (item.Relationship?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                (item.PhoneNumber?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                (item.TreatmentType?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                (item.AthleteComments?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) ||
                (item.DateOfBirth.ToString("MM/dd/yyyy")?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) // Date of Birth formatted as string
            );

            DisplayList = new ObservableCollection<AthleteDetail>(filteredItems);
        }
    }

    private async void OnTileTapped(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var tappedItem = frame.BindingContext as AthleteDetail;

        if (tappedItem != null)
        {
            // Fetch the corresponding AthleteForm for the selected AthleteDetail
            var athleteForm = _businessLogic.GetForms(schoolCode: SchoolCode)
                                            .FirstOrDefault(form => form.FullName == tappedItem.FullName);

            if (athleteForm != null)
            {
                await Detail.Navigation.PushAsync(new AthleteFormInformation(athleteForm));
            }
            else
            {
                await DisplayAlert("Error", "Athlete information not found.", "OK");
            }
        }
    }

    private void NavigateToHome(object obj)
    {
        if (Application.Current != null)
        {
            Application.Current.MainPage = new NavigationPage(new MainTabbedPage());
        }
        IsPresented = false;
    }

    private void NavigateToPastForms()
    {
        Detail = new NavigationPage(new AthletePastForms());
        IsPresented = false;
    }

    private void NavigateToStatistics()
    {
        Detail = new NavigationPage(new InjuryStatistics());
        IsPresented = false;
    }

    private void NavigateToAthleteStatuses()
    {
        Detail = new NavigationPage(new AthleteStatuses(SchoolCode));
        IsPresented = false;
    }
}
