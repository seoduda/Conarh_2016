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
            BackgroundColor = AppResources.MenuColor;

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
            var label1 = new Label { Text = "Minha Conta", TextColor = AppResources.MenuTitleTextColor, FontSize = 18 };

            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleTextColor));
            layout.Children.Add(label1);
            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleTextColor));
            layout.Children.Add(GetMenuButton(MainMenuItemData.ProfilePage));
            layout.Children.Add(GetMenuButton(MainMenuItemData.HowtoPlayPage));
            layout.Children.Add(GetMenuButton(MainMenuItemData.ConnectPage));
            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleTextColor));
            layout.Children.Add(new Label { Text = "Agendas", TextColor = AppResources.MenuTitleTextColor, FontSize = 18 });
            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleTextColor));
            layout.Children.Add(GetMenuButton(MainMenuItemData.AgendaCongressoPage));
            layout.Children.Add(GetMenuButton(MainMenuItemData.AgendaExpoPage));
            layout.Children.Add(GetMenuButton(MainMenuItemData.FavoriteEventsPage));
            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleTextColor));
            layout.Children.Add(new Label { Text = "Evento", TextColor = AppResources.MenuTitleTextColor, FontSize = 18 });
            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleTextColor));

            layout.Children.Add(GetMenuButton(MainMenuItemData.ExhibitorsPage));
            layout.Children.Add(GetMenuButton(MainMenuItemData.MapPage));
            layout.Children.Add(GetMenuButton(MainMenuItemData.WallPage));
            //if (Device.OS != TargetPlatform.iOS)
            //{
                layout.Children.Add(GetMenuButton(MainMenuItemData.MidiaPage));
            //}

            layout.Children.Add(GetSeparator(1, AppProvider.Screen.Width, AppResources.MenuTitleTextColor));
            //layout.Children.Add(GetMenuButton(MainMenuItemData.TestPage));

            if (AppModel.Instance.CurrentUser == null)
                layout.Children.Add(GetMenuButton(MainMenuItemData.LoginPage));
            else
                AddLogout(layout);
            AddI9Logo(layout);

            AppModel.Instance.UserChanged += OnUserChanged;

            Content = layout;
        }

        private void AddLogout(StackLayout layout)
        {
            var logOutBtn = new MainMenuButton(MainMenuItemData.LogOutPage);
            logOutBtn.Select += OnLogOut;
            layout.Children.Add(logOutBtn);
        }

        private void AddI9Logo(StackLayout layout)
        {
            layout.Children.Add(new ContentView
            {
                Padding = new Thickness(20, 10, 10, 20),
                Content = new Image()
                {
                    HeightRequest = 70,
                    HorizontalOptions = LayoutOptions.End,
                    Source = ImageLoader.Instance.GetImage(AppResources.I9acaoLogo, false)
                }
            });
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
                        layout.Children.RemoveAt(layout.Children.Count - 1);
                        AddLogout(layout);
                        AddI9Logo(layout);
                    }
                }
                else
                {
                    StackLayout layout = (StackLayout)Content;
                    layout.Children.RemoveAt(layout.Children.Count - 1);
                    layout.Children.RemoveAt(layout.Children.Count - 1);
                    layout.Children.Add(GetMenuButton(MainMenuItemData.LoginPage));
                    AddI9Logo(layout);
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