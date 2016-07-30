using Conarh_2016.Core;
using System;

using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Main
{
    /* retirado do if andoid
        public static MainMenuItemData AgendaExpoPage = new MainMenuItemData(false, AppResources.AgendaExpo, typeof(Events.AgendaExpoGridPage), true, false);
        public static MainMenuItemData AgendaCongresso = new MainMenuItemData(false, AppResources.AgendaCongresso, typeof(Events.AgendaGridPage), true, false);
        */

    public sealed class MainMenuItemData
    {
#if __ANDROID__

              public static MainMenuItemData ConnectPage = new MainMenuItemData(true, AppResources.Connect, typeof(Connect.ConnectionGridPage), true, false);
              public static MainMenuItemData ProfilePage = new MainMenuItemData(true, AppResources.Profile, typeof(Profile.ProfileGridPage), true, false);
#else

        public static MainMenuItemData ConnectPage = new MainMenuItemData(true, AppResources.Connect, typeof(Connect.ConnectionPage), false, false);
        public static MainMenuItemData ProfilePage = new MainMenuItemData(true, AppResources.Profile, typeof(Profile.ProfilePage), false, false);

#endif
        public static MainMenuItemData AgendaCongressoPage = new MainMenuItemData(false, AppResources.AgendaCongresso, typeof(Events.AgendaPage), false, false);
        public static MainMenuItemData AgendaExpoPage = new MainMenuItemData(false, AppResources.AgendaExpo, typeof(Events.AgendaExpoPage), false, false);
        public static MainMenuItemData FavoriteEventsPage = new MainMenuItemData(true, AppResources.FavoriteEvents, typeof(Events.FavoriteEventsPage), false, false);

        public static MainMenuItemData ExhibitorsPage = new MainMenuItemData(false, AppResources.Exhibitors, typeof(Exhibitors.ExhibitorsDynamicPage), false, false);
        public static MainMenuItemData MapPage = new MainMenuItemData(false, AppResources.Map, typeof(Map.MapPage), false, false);
        public static MainMenuItemData WallPage = new MainMenuItemData(false, AppResources.Wall, typeof(Wall.WallPage), false, false);

        public static MainMenuItemData HowtoPlayPage = new MainMenuItemData(false, AppResources.HowToPlay, typeof(Connect.HowToPlayPage), false, false);
        public static MainMenuItemData MidiaPage = new MainMenuItemData(false, AppResources.Midia, typeof(Midia.MediaPage), false, false);
        public static MainMenuItemData LoginPage = new MainMenuItemData(false, AppResources.Login, typeof(Login.LoginPage), false, false);
        public static MainMenuItemData TestPage = new MainMenuItemData(false, "TESTE", typeof(Connect.ConnectionGridPage), false, false);
        public static MainMenuItemData LogOutPage = new MainMenuItemData(false, AppResources.LogOut, null, false, false);

        public string Title { get; set; }
        public Type TargetType { get; set; }

        public bool HasTopBorder { get; set; }
        public bool HasBottomBorder { get; set; }

        public bool IsLoginRequired { get; set; }

        public MainMenuItemData(bool isLoginRequired, string title, Type target, bool hasTopBorder, bool hasBottomBorder)
        {
            Title = title;
            TargetType = target;
            HasTopBorder = hasTopBorder;
            HasBottomBorder = hasBottomBorder;

            IsLoginRequired = isLoginRequired;
        }
    }

    public sealed class MainMenuButton : ContentView
    {
        public event Action<MainMenuItemData> Select;

        public readonly MainMenuItemData Data;

        private BoxView GetBoxView(int height, int width)
        {
            return new BoxView()
            {
                HeightRequest = height,
                WidthRequest = width,
                BackgroundColor = Color.White
            };
        }

        public MainMenuButton(MainMenuItemData data)
        {
            Data = data;
            var itemVerticalLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical
            };

            if (Data.HasTopBorder)
                itemVerticalLayout.Children.Add(GetBoxView(1, AppProvider.Screen.Width));

            var menuItemContentView = new ContentView()
            {
                BackgroundColor = AppResources.MenuColor,
                Padding = new Thickness(14, 0, 0, 0),
                HeightRequest = 30,
                VerticalOptions = LayoutOptions.Center,
                Content = new Label()
                {
                    Text = Data.Title,
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Color.White,
                    FontSize = 18
                }
            };

            TapGestureRecognizer gesture = new TapGestureRecognizer();
            gesture.Tapped += OnButtonClicked;
            menuItemContentView.GestureRecognizers.Add(gesture);

            itemVerticalLayout.Children.Add(menuItemContentView);

            if (Data.HasBottomBorder)
                itemVerticalLayout.Children.Add(GetBoxView(1, AppProvider.Screen.Width));

            Content = itemVerticalLayout;
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            if (Select != null)
                Select(Data);
        }
    }
}