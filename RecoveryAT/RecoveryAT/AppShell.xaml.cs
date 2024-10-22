namespace RecoveryAT;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		MainPage = new AthleteStatuses();
	}

    public AthleteStatuses MainPage { get; private set; }
}
