namespace RecoveryAT
{
    public partial class App : Application
    {
        public User User { get; set; } // Creates a user service
        public IBusinessLogic BusinessLogic { get; private set; } // Publicly accessible BusinessLogic instance

        public App()
        {
            InitializeComponent();
            User = new User(); // Creates a user instance

            var navPage = new NavigationPage(new WelcomeScreen())
            {
                BarTextColor = Colors.Blue
            };

            // Initialize dependencies for BusinessLogic
            var contactsDatabase = new ContactsDatabase();
            var formsDatabase = new FormsDatabase();
            var usersDatabase = new UsersDatabase();
            var searchDatabase = new SearchDatabase();
            var database = new Database();

            // Initialize BusinessLogic with all required dependencies
            BusinessLogic = new BusinessLogic(contactsDatabase, formsDatabase, usersDatabase, searchDatabase, database);

            MainPage = navPage;
        }
    }
}
