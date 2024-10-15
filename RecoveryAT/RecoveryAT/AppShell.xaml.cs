namespace RecoveryAT;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		MainPage = new AthleteContacts();
	}

    public AthleteContacts MainPage { get; private set; }
}
