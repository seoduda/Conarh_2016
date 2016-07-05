using Conarh_2016.Application;
using Conarh_2016.Core;
using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Main
{
    public sealed class RootPage : MasterDetailPage
    {
        /*
        public RootPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
    */
		MainMenuPage menuPage;


        public string TesteNome
        { set; get; }


        public Page CurrentPage
        { private set; get; }

        public Page PreviousPage
        { private set; get; }

        private MainMenuItemData _currentPage = null;

        public RootPage()
        {
            AppController.Instance.AppRootPage = this;
            menuPage = new MainMenuPage();
            menuPage.ItemSelected += (e) => NavigateTo(e, false);

            Master = menuPage;
            if (AppModel.Instance.CurrentUser == null)
                NavigateTo(MainMenuItemData.LoginPage, false);
            else
                NavigateTo(MainMenuItemData.ProfilePage, true);

            AppModel.Instance.UserChanged += OnUserChanged;
        }

        private void OnUserChanged(UserModel previous, UserModel current)
        {
            Device.BeginInvokeOnMainThread(() => NavigateTo(AppModel.Instance.CurrentUser != null ? MainMenuItemData.ProfilePage : MainMenuItemData.LoginPage, false));
        }

        public void NavigateTo(MainMenuItemData menu, bool force, params object[] args)
        {
            if (!force && _currentPage == menu)
                return;

            if (menu.IsLoginRequired && AppModel.Instance.CurrentUser == null)
            {
                AppProvider.PopUpFactory.ShowMessage(AppResources.LoginFirstMessage, AppResources.Warning);
                return;
            }

            _currentPage = menu;

            if (CurrentPage != null)
                PreviousPage = CurrentPage;

            CurrentPage = (Page)Activator.CreateInstance(menu.TargetType, args);

            var navPage = new NavigationPage(CurrentPage);

            navPage.BarBackgroundColor = AppResources.MenuColor;
            navPage.BarTextColor = AppResources.MenuTitleTextColor;
            Detail = navPage;

            IsPresented = false;
        }
    }

}
