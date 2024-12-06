using CommunityToolkit.Maui;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;

namespace RecoveryAT;

public static class MauiProgram
{
    // Initialize all database dependencies
    private static readonly IContactsDatabase ContactsDatabase = new ContactsDatabase();
    private static readonly IFormsDatabase FormsDatabase = new FormsDatabase();
    private static readonly IUsersDatabase UsersDatabase = new UsersDatabase();
    private static readonly ISearchDatabase SearchDatabase = new SearchDatabase();
    private static readonly IDatabase GeneralDatabase = new Database();

    // Create an instance of BusinessLogic with all required dependencies
    public static IBusinessLogic BusinessLogic = new BusinessLogic(
        ContactsDatabase, 
        FormsDatabase, 
        UsersDatabase, 
        SearchDatabase, 
        GeneralDatabase
    );

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMicrocharts()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
