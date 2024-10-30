namespace RecoveryAT;

// MainTabbedPage.xaml.cs
public partial class MainTabbedPage : TabbedPage
{
    public MainTabbedPage()
    {
        InitializeComponent();
        CurrentPageChanged += OnTabChanged;
    }

    private void OnTabChanged(object sender, EventArgs e)
    {
        // Check if the current tab is the "Athletes" tab
        if (CurrentPage is NavigationPage navigationPage &&
            navigationPage.CurrentPage is AthleteInformation)
        {
            // Set AthleteInformation as the MainPage
            Application.Current.MainPage = new AthleteInformation();
        }
    }
}
