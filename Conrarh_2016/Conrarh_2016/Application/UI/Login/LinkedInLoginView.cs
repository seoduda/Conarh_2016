using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Core;
using System;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace Conarh_2016.Application.UI.Login
{
    public sealed class LinkedInLoginView : ContentView
    {
        private readonly ExtendedEntry _passwordEntry;

        private int LeftBorder = 30;
  
        public event Action<string> LogIn;

        public LinkedInLoginView() : base()
        {
            LeftBorder = AppProvider.Screen.ConvertPixelsToDp(20);

            var layout = new StackLayout { BackgroundColor = Color.Transparent };

            BoxView transpSpace = new BoxView();
            transpSpace.Color = Color.Transparent;
            transpSpace.WidthRequest = 1;
            transpSpace.HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 4);

            layout.Children.Add(transpSpace);

            Image LinkedinLogo = new Image()
            {
                Source = ImageLoader.Instance.GetImage(AppResources.LinkedinLogo, false),
                HeightRequest = AppProvider.Screen.ConvertPixelsToDp(94),
                HorizontalOptions = LayoutOptions.Center
        };
            layout.Children.Add(LinkedinLogo);

            var fs = new FormattedString();

            string firstPart = "Para concluir o seu cadastro, por favor entre com uma senha ";
            string secondPart = "para o aplicativo Conarh 2016. ";
            string thirdPart = "As outras informações do seu cadastro já foram importadas do Linkedin.";


            fs.Spans.Add(new Span { Text = firstPart, ForegroundColor = Color.Black, FontSize = 16 });
            fs.Spans.Add(new Span { Text = secondPart, ForegroundColor = Color.Black, FontSize = 16 });
            fs.Spans.Add(new Span { Text = thirdPart, ForegroundColor = Color.Black, FontSize = 16 });

            var labelTerms = new Label
            {
                FormattedText = fs,
            };


            layout.Children.Add(LinkedinLogo);
            layout.Children.Add(new ContentView { Content = labelTerms, Padding = new Thickness(10, 5, 10, 5) });



            layout.Children.Add(GetEntry(Keyboard.Text, AppResources.LoginPasswordDefaultEntry, AppResources.LoginPasswordImage, 10, true, 10, out _passwordEntry));

            var loginBtn = new Button
            {
                BorderRadius = 5,
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp((AppProvider.Screen.Width / 4) * 3),
                HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 14),
                TextColor = Color.White,
                BackgroundColor = AppResources.MenuColor,
                Text = AppResources.Login,
                FontSize = 16
            };
            loginBtn.Clicked += OnLoginClicked;
            layout.Children.Add(new ContentView { Padding = new Thickness(LeftBorder, 10, LeftBorder, 0), HorizontalOptions = LayoutOptions.Center, Content = loginBtn });


            BGLayoutView bgLayout = new BGLayoutView(AppResources.LoginBgImage, layout, true, true);

            Content = new ScrollView { Content = bgLayout };
           

        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            string passwd = _passwordEntry.Text.Trim();

            if (String.IsNullOrEmpty(passwd))
            {
                AppProvider.PopUpFactory.ShowMessage(AppResources.PasswordIsEmpty, AppResources.Error);
            }
            else
            {
                if (LogIn != null)
                    LogIn(passwd);
            }
        }


        private AbsoluteLayout GetEntry(Keyboard keyboard, string defaultValue, string icon, int topPadding, bool isPassword, int iconHeight, out ExtendedEntry textEntry)
        {
            var layout = new AbsoluteLayout { Padding = new Thickness(LeftBorder, topPadding, 0, 0) };

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