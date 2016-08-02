using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Core;
using System;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace Conarh_2016.Application.UI.Login
{
    public sealed class ResetEmailView : ContentPage
    {
        private readonly ExtendedEntry _emailEntry;

        public ResetEmailView()
        {
            // LeftBorder = AppProvider.Screen.ConvertPixelsToDp(20);

            var layout = new StackLayout { BackgroundColor = Color.Transparent };

            BoxView transpSpace = new BoxView();
            transpSpace.Color = Color.Transparent;
            transpSpace.WidthRequest = 1;
            transpSpace.HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 8);

            layout.Children.Add(transpSpace);

            Label ResetEmailLabel = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = AppResources.MenuColor,
                FontSize = 16,
                Text = AppResources.LoginForgetPasswordEnterEmail,
            };

            layout.Children.Add(ResetEmailLabel);

            layout.Children.Add(GetEntry(Keyboard.Email, AppResources.LoginEmailDefaultEntry, AppResources.LoginEmailImage, 15, false, 15, out _emailEntry));

            var ResetBtn = new Button
            {
                BorderRadius = 5,
                //HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height/10),
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp((AppProvider.Screen.Width / 4) * 3),
                HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 14),
                TextColor = Color.White,
                BackgroundColor = AppResources.MenuColor,
                Text = AppResources.EventsActionPostQuestion,
                FontSize = 16
            };
            /*
            var TESTEBtn = new Button
            {
                BorderRadius = 5,
                //HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height/10),
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp((AppProvider.Screen.Width / 4) * 3),
                HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 14),
                TextColor = Color.White,
                BackgroundColor = AppResources.MenuColor,
                Text = "teste",
                FontSize = 16
            };
            */


            ResetBtn.Clicked += OnResetClicked;
//            TESTEBtn.Clicked += OnTestClicked;
            layout.Children.Add(new ContentView { Padding = new Thickness(10, 10, 10, 0), HorizontalOptions = LayoutOptions.Center, Content = ResetBtn });
            //layout.Children.Add(new ContentView { Padding = new Thickness(10, 10, 10, 0), HorizontalOptions = LayoutOptions.Center, Content = TESTEBtn });

            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, true, true);
            //Content = new ScrollView { Content = layout };
            Content = new ScrollView { Content = bgLayout };
        }


        private void OnResetClicked(object sender, EventArgs e)
        {
            if (_emailEntry.Text != null)
            {
                if (_emailEntry.Text.Trim().Length > 0)
                    AppController.Instance.ResetForgetPassword(_emailEntry.Text.Trim());
            }
        }

        private AbsoluteLayout GetEntry(Keyboard keyboard, string defaultValue, string icon, int topPadding, bool isPassword, int iconHeight, out ExtendedEntry textEntry)
        {
            var layout = new AbsoluteLayout { Padding = new Thickness(20, topPadding, 0, 0) };

            layout.Children.Add(new Image { Source = ImageLoader.Instance.GetImage(icon, false), HeightRequest = iconHeight }, new Point(5, 3));

            textEntry = new ExtendedEntry
            {
                Keyboard = keyboard,
                Placeholder = defaultValue,
                TextColor = Color.Black,
                BackgroundColor = Color.Transparent,
                HasBorder = false,
                IsPassword = isPassword,
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width - 30 * 2 - 30),
                Font = Font.SystemFontOfSize(16)
            };

            textEntry.HasBorder = false;

            Point entryPosition = new Point(30, 0);
            int width = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width - 30 * 2);

            if (Device.OS == TargetPlatform.Android)
            {
                entryPosition = new Point(30, -3);
            }

            layout.Children.Add(textEntry, entryPosition);

            layout.Children.Add(new BoxView { BackgroundColor = AppResources.LoginActiveTextColor, HeightRequest = 0.5f, WidthRequest = width }, new Point(0, 20));
            layout.Children.Add(new BoxView { BackgroundColor = AppResources.LoginActiveTextColor, HeightRequest = 5, WidthRequest = 0.5f }, new Point(0, 15));

            return layout;
        }
    }
}