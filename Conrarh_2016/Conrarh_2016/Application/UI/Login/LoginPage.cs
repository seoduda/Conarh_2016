using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.UI.Main;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Login
{
    public sealed class LoginPage : ContentPage
    {
        // public LoginPage() : this("fernando@i9acao.com.br", "mytobas45")
        public LoginPage() : this(string.Empty, string.Empty)
        {
        }

        public LoginPage(string email, string password)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            //this.BackgroundImage = AppResources.LoginBgImage;
            
            Title = " ";
            LoginView loginView = new LoginView();
            loginView.SignUp += OpenSignUp;
            loginView.LogIn += OnLogIn;
            loginView.ResetEmail += OpenResetEmail;

            loginView.InitWithData(email, password);
            Content = loginView;
        }

        private void OnLogIn(LoginUserData data)
        {
            if (data != null)
                UserController.Instance.LoginUser(data);
            else
                (Parent.Parent as RootPage).NavigateTo(MainMenuItemData.AgendaCongressoPage, false);
        }

        private void OpenSignUp()
        {
            var signUpView = new SignUpView();
            signUpView.SignUp += OnCreateUser;
            Navigation.PushAsync(signUpView);
        }

        private void OpenResetEmail()
        {
            var resetEmailView = new ResetEmailView();
           // resetEmailView.SignUp += OnCreateUser;
            Navigation.PushAsync(resetEmailView);
        }


        private void OnCreateUser(CreateUserData userData)
        {
            UserController.Instance.RegisterUser(userData);
        }
    }
}