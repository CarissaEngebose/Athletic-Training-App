using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace RecoveryAT
{
    public partial class AthleteTabbedPage : Microsoft.Maui.Controls.TabbedPage
    {
        private string SchoolCode { get; set; }

        public AthleteTabbedPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            // Retrieve SchoolCode dynamically from the user's profile
            var user = ((App)Microsoft.Maui.Controls.Application.Current).User;
            string email = user.Email;
            
            if (!string.IsNullOrWhiteSpace(email))
            {
                var userData = ((App)Microsoft.Maui.Controls.Application.Current).BusinessLogic.GetUserByEmail(email);
                if (userData != null && userData.ContainsKey("SchoolCode"))
                {
                    SchoolCode = userData["SchoolCode"];
                }
                else
                {
                    SchoolCode = "DefaultCode"; // Fallback value in case of error
                }
            }
            else
            {
                SchoolCode = "DefaultCode"; // Fallback value if email is unavailable
            }

            // Create instances of tabs and pass the required parameters
            var athleteInformationPage = new NavigationPage(new AthleteInformation())
            {
                Title = "Athlete Information",
                IconImageSource = "athlete_icon.png"
            };

            var athletePastFormsPage = new NavigationPage(new AthletePastForms())
            {
                Title = "Past Forms",
                IconImageSource = "forms_icon.png" 
            };

            var injuryStatisticsPage = new NavigationPage(new InjuryStatistics())
            {
                Title = "Statistics",
                IconImageSource = "statistics_icon.png" 
            };

            var athleteStatusesPage = new NavigationPage(new AthleteStatuses(SchoolCode))
            {
                Title = "Athlete Statuses",
                IconImageSource = "statuses_icon.png" 
            };

            // Add tabs to the TabbedPage
            Children.Add(athleteInformationPage);
            Children.Add(athletePastFormsPage);
            Children.Add(injuryStatisticsPage);
            Children.Add(athleteStatusesPage);

            BindingContext = this;
        }
    }
}
