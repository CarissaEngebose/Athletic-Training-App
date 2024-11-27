namespace RecoveryAT;

public partial class App : Application
{
	public User User { get; set; } // creates an user service
    public IBusinessLogic BusinessLogic { get; private set; } // Publicly accessible BusinessLogic instance

	public App()
	{
		InitializeComponent();
        User = new User(); // creates a user instance

        var navPage = new NavigationPage(new WelcomeScreen())
        {
            BarTextColor = Colors.Blue 
        };

        // Initialize BusinessLogic with the appropriate implementation
        BusinessLogic = new BusinessLogic(new Database());

        MainPage = navPage;

	}
}
