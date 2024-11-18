namespace RecoveryAT;

public partial class App : Application
{
	public AuthenticationService AuthService { get; private set; } // creates an authentication service
    public IBusinessLogic BusinessLogic { get; private set; } // Publicly accessible BusinessLogic instance

	public App()
	{
		InitializeComponent();
        AuthService = new AuthenticationService(); // creates an authentication service instance

        var navPage = new NavigationPage(new WelcomeScreen())
        {
            BarTextColor = Colors.Blue 
        };

        // Initialize BusinessLogic with the appropriate implementation
        BusinessLogic = new BusinessLogic(new Database());

        MainPage = navPage;

	}
}
