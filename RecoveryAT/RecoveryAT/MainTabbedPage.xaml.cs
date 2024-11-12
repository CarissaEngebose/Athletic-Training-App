namespace RecoveryAT;

// MainTabbedPage.xaml.cs
public partial class MainTabbedPage : TabbedPage
{
    public MainTabbedPage(String SchoolCode)
    {
        InitializeComponent();
        // Moved instantiation to code behind so that I can pass in a businessLogic and SchoolCode. 
        Children.Add(new NavigationPage(new TrainerHomeScreen(MauiProgram.BusinessLogic, SchoolCode)) { IconImageSource = "home_icon.png", Title = "Home" });
        Children.Add(new NavigationPage(new AthleteInformation()) {IconImageSource = "running_icon.png", Title = "Athletes" });
        Children.Add(new NavigationPage(new SchoolCodeScreen()) {IconImageSource = "clipboard_icon.png", Title = "Form" });
        Children.Add(new NavigationPage(new UserProfile()) {IconImageSource = "profile_icon.png", Title = "Profile" });
        
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
