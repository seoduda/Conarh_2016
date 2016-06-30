using Conarh_2016.Core;
using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Main
{
    public sealed class MainMenuPage : ContentPage
    {
        public event Action<MainMenuItemData> ItemSelected;

        public MainMenuPage()
        {
            Icon = AppResources.SettingsImage;
            Title = "menu"; // The Title property must be set.
            BackgroundColor = AppResources.MenuGreen;

            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            if (Device.OS == TargetPlatform.iOS)
            {
                layout.Padding = new Thickness(0, 24, 0, 0);
                layout.Children.Add(new ContentView
                {
                    Padding = new Thickness(5, 0, 0, 0),
                    Content = new Label
                    {
                        Text = "CONARH - 2016",
                        FontSize = 25,
                        TextColor = Color.White,
                        HeightRequest = 40,
                        FontAttributes = FontAttributes.Bold
                    }
                });
            }
            var label1 = new Label { Text = "Minha Conta", TextColor = AppResources.MenuTitleGreen, FontSize = 18 };

            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleGreen));
            layout.Children.Add(label1);
            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleGreen));
            layout.Children.Add(GetMenuButton(MainMenuItemData.ProfilePage));
            layout.Children.Add(GetMenuButton(MainMenuItemData.ConnectPage));
            if (AppModel.Instance.CurrentUser == null)
                layout.Children.Add(GetMenuButton(MainMenuItemData.LoginPage));
            else
                AddLogout(layout);
            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleGreen));
            layout.Children.Add(new Label { Text = "Agendas", TextColor = AppResources.MenuTitleGreen, FontSize = 18 });
            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleGreen));
            layout.Children.Add(GetMenuButton(MainMenuItemData.AgendaCongressoPage));
            layout.Children.Add(GetMenuButton(MainMenuItemData.AgendaExpoPage));
            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleGreen));
            layout.Children.Add(new Label { Text = "Evento", TextColor = AppResources.MenuTitleGreen, FontSize = 18 });
            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleGreen));

            layout.Children.Add(GetMenuButton(MainMenuItemData.ExhibitorsPage));
            layout.Children.Add(GetMenuButton(MainMenuItemData.MapPage));
            layout.Children.Add(GetMenuButton(MainMenuItemData.WallPage));

            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleGreen));
            var logoI9acao = new Image();
            logoI9acao.Source = ImageLoader.Instance.GetImage(AppResources.I9acaoLogo, false);

            layout.Children.Add(logoI9acao);
            
            AppModel.Instance.UserChanged += OnUserChanged;

            Content = layout;
        }

        private void AddLogout(StackLayout layout)
        {
            var logOutBtn = new MainMenuButton(MainMenuItemData.LogOutPage);
            logOutBtn.Select += OnLogOut;
            layout.Children.Add(logOutBtn);
        }

        private void OnUserChanged(UserModel previous, UserModel current)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (AppModel.Instance.CurrentUser != null)
                {
                    StackLayout layout = (StackLayout)Content;

                    if (AppModel.Instance.CurrentUser != null)
                    {
                        layout.Children.RemoveAt(layout.Children.Count - 1);
                        AddLogout(layout);
                    }
                }
                else
                {
                    StackLayout layout = (StackLayout)Content;
                    layout.Children.RemoveAt(layout.Children.Count - 1);
                    layout.Children.Add(GetMenuButton(MainMenuItemData.LoginPage));
                }
            });
        }

        private void OnLogOut(MainMenuItemData obj)
        {
            UserController.Instance.Logout();
        }

        private MainMenuButton GetMenuButton(MainMenuItemData data)
        {
            var button = new MainMenuButton(data);
            button.Select += OnMenuItemSelected;
            return button;
        }

        private void OnMenuItemSelected(MainMenuItemData item)
        {
            if (ItemSelected != null)
                ItemSelected(item);
        }

        private BoxView GetSeparator(int height, int width, Color _color)
        {
            return new BoxView()
            {
                HeightRequest = height,
                WidthRequest = width,
                BackgroundColor = _color
            };
        }
    }
}