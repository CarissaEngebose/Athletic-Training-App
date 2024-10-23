public class AuthenticationService {
    public bool IsLoggedIn { get; private set; }

    public void Login() {
        IsLoggedIn = true;
    }

    public void Logout() {
        IsLoggedIn = false;
    }
}
