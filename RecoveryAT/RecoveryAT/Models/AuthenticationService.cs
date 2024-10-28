/**
    Name: Carissa Engebose
    Date: 10/24/24
    Description: Implementation for log in and log out services for the user. Right now, it is only used to show
    the main tabbed bar or not because we haven't implemented create account or log in.
    Bugs: None that I know of.
    Reflection: This class didn't take long at all and was only used to display the main tabbed bar if the log in
    or create account buttons were clicked.
**/

public class AuthenticationService {
    public bool IsLoggedIn { get; private set; }

    public void Login() {
        IsLoggedIn = true;
    }

    public void Logout() {
        IsLoggedIn = false;
    }
}
