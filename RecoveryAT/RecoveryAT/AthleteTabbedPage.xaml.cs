using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace RecoveryAT
{
    public partial class AthleteTabbedPage : Microsoft.Maui.Controls.TabbedPage
    {
        private readonly string SchoolCode = "THS24";
        
        public AthleteTabbedPage()
        {
            On<iOS>().SetUseSafeArea(true);

            InitializeComponent();

            // Create instances of tabs and pass the required parameters
            var athleteInformationPage = new Microsoft.Maui.Controls.NavigationPage(new AthleteInformation())
            {
                Title = "Athlete Information",
            };

            var athletePastFormsPage = new Microsoft.Maui.Controls.NavigationPage(new AthletePastForms())
            {
                Title = "Past Forms",
            };

            var injuryStatisticsPage = new Microsoft.Maui.Controls.NavigationPage(new InjuryStatistics())
            {
                Title = "Statistics",
            };

            var athleteStatusesPage = new Microsoft.Maui.Controls.NavigationPage(new AthleteStatuses(SchoolCode))
            {
                Title = "Athlete Statuses",
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
