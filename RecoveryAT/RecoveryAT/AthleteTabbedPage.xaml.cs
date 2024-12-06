/*
    Date: 12/06/2024
    Description: This page creates a tabbed interface for navigating various athlete-related 
                 functionalities in the RecoveryAT application. The tabs include athlete information, 
                 past forms, injury statistics, and statuses, with SchoolCode dynamically loaded 
                 based on the user's profile.
    Bugs: None Known
    Reflection: These tabs were a little hard to implement because the AthleteStatuses page takes
                in the SchoolCode, so the tabs needed to be added to as children in the cs file.
*/

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace RecoveryAT
{
    public partial class AthleteTabbedPage : Microsoft.Maui.Controls.TabbedPage
    {
        // Property to store the school code for the logged-in user
        private string SchoolCode { get; set; }

        // Constructor to initialize the AthleteTabbedPage
        public AthleteTabbedPage()
        {
            InitializeComponent(); // Load the XAML components

            // Remove the navigation bar for this page
            NavigationPage.SetHasNavigationBar(this, false);

            // Retrieve the user's email and dynamically fetch their SchoolCode
            var user = ((App)Microsoft.Maui.Controls.Application.Current).User; // Access the current user from the App class
            string email = user.Email; // Retrieve the email of the current user
            
            if (!string.IsNullOrWhiteSpace(email)) // Check if the email is valid
            {
                var userData = MauiProgram.BusinessLogic.GetUserFromEmail(email); // Fetch user data using email

                if (user != null && user.SchoolCode != null) // Check if user and SchoolCode are valid
                {
                    SchoolCode = user.SchoolCode; // Assign the SchoolCode from the user's profile
                }
                else
                {
                    SchoolCode = "DefaultCode"; // Assign a default value if SchoolCode is not available
                }
            }
            else
            {
                SchoolCode = "DefaultCode"; // Assign a default value if email is unavailable
            }

            // Create instances of each tab with appropriate content and icons
            var athleteInformationPage = new NavigationPage(new AthleteInformation()) // Tab for athlete information
            {
                Title = "Info", // Set the title of the tab
                IconImageSource = "athlete_icon.png" // Set the icon for the tab
            };

            var athletePastFormsPage = new NavigationPage(new AthletePastForms()) // Tab for past forms
            {
                Title = "Forms", // Set the title of the tab
                IconImageSource = "forms_icon.png" // Set the icon for the tab
            };

            var injuryStatisticsPage = new NavigationPage(new InjuryStatistics()) // Tab for injury statistics
            {
                Title = "Statistics", // Set the title of the tab
                IconImageSource = "statistics_icon.png" // Set the icon for the tab
            };

            var athleteStatusesPage = new NavigationPage(new AthleteStatuses(SchoolCode)) // Tab for athlete statuses
            {
                Title = "Statuses", // Set the title of the tab
                IconImageSource = "statuses_icon.png" // Set the icon for the tab
            };

            // Add each tab to the TabbedPage
            Children.Add(athleteInformationPage); // Add the Info tab
            Children.Add(athletePastFormsPage); // Add the Forms tab
            Children.Add(injuryStatisticsPage); // Add the Statistics tab
            Children.Add(athleteStatusesPage); // Add the Statuses tab

            BindingContext = this; // Set the BindingContext for data binding
        }
    }
}
