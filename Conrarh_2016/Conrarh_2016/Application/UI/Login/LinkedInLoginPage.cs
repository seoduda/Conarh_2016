using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Core;
using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Login
{
    public class LinkedInLoginPage : ContentPage
    {
        public event Action PostSignUp;

        private CreateUserData _newUserData;
        private String _serverImagePath;

        public LinkedInLoginPage(CreateUserData data, String serverImagePath)
        {
            _newUserData = data;
            _serverImagePath = serverImagePath;
            NavigationPage.SetHasNavigationBar(this, false);

            Title = " ";
            LinkedInLoginView loginView = new LinkedInLoginView();
            loginView.LogIn += OnLogIn;
            Content = loginView;
        }

        private void OnLogIn(String password)
        {
            if (String.IsNullOrEmpty(password.Trim()))
            {
                AppProvider.PopUpFactory.ShowMessage(AppResources.PasswordIsEmpty, AppResources.Error);
            }
            else
            {
                _newUserData.Password = password;
                UserController.Instance.RegisterUserLinkedin(_newUserData, _serverImagePath);
            }
        }
    }
}