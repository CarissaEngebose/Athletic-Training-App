namespace RecoveryAT;

// MainTabbedPage.xaml.cs
public partial class MainTabbedPage : TabbedPage
{
    public MainTabbedPage(BusinessLogic businessLogic, String SchoolCode)
    {
        InitializeComponent();
        // Moved instantiation to code behind so that I can pass in a businessLogic and SchoolCode. This allows all screens to use the same Business logic
        Children.Add(new NavigationPage(new TrainerHomeScreen(businessLogic, SchoolCode)) { IconImageSource = "home_icon.png", Title = "Home" });

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
