namespace RecoveryAT;

public partial class App : Application
{
	public AuthenticationService AuthService { get; private set; } // creates an authentication service

	public App()
	{
		InitializeComponent();
        AuthService = new AuthenticationService(); // creates an authentication service instance

        var navPage = new NavigationPage(new WelcomeScreen())
        {
            BarTextColor = Colors.Blue 
        };

        MainPage = navPage;

	}
}
