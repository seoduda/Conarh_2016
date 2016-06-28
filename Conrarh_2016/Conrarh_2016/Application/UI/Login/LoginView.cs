using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Core;
using System;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace Conarh_2016.Application.UI.Login
{
    public sealed class LoginView : ContentView
    {
        private readonly ExtendedEntry _emailEntry;
        private readonly ExtendedEntry _passwordEntry;

        private int LeftBorder = 30;

        public event Action SignUp;

        public event Action<LoginUserData> LogIn;

        public LoginView() : base()
        {
            LeftBorder = AppProvider.Screen.ConvertPixelsToDp(30);
            var relatLayout = new RelativeLayout { BackgroundColor = Color.Transparent };
            Image loginBGImage = new Image();
            loginBGImage.Source = ImageLoader.Instance.GetImage(AppResources.LoginBGImage, false);

            relatLayout.Children.Add(loginBGImage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.Constant(AppProvider.Screen.Width),
            Constraint.Constant(AppProvider.Screen.Height));


            var layout = new StackLayout { BackgroundColor = Color.Transparent };


            layout.Children.Add(new Image
            {
                Source = ImageLoader.Instance.GetImage(AppResources.LoginHeaderImage, false)
            });

            layout.Children.Add(GetEntry(Keyboard.Email, AppResources.LoginEmailDefaultEntry, AppResources.LoginEmailImage, 30, false, 15, out _emailEntry));
            layout.Children.Add(GetEntry(Keyboard.Text, AppResources.LoginPasswordDefaultEntry, AppResources.LoginPasswordImage, 20, true, 10, out _passwordEntry));

            var loginBtn = new Button
            {
                BorderRadius = 5,
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width),
                TextColor = Color.White,
                BackgroundColor = AppResources.LoginButtonColor,
                Text = AppResources.Login,
                FontSize = 20
            };
            loginBtn.Clicked += OnLoginClicked;
            layout.Children.Add(new ContentView { Padding = new Thickness(LeftBorder, 20, LeftBorder, 0), HorizontalOptions = LayoutOptions.Center, Content = loginBtn });

            var skipLoginBtn = new Button
            {
                BorderRadius = 5,
                WidthRequest = AppProvider.Screen.Width,
                TextColor = Color.Black,
                BackgroundColor = AppResources.LoginSkipButtonColor,
                Text = AppResources.LoginSkip,
                FontSize = 20
            };
            skipLoginBtn.Clicked += OnLoginSkipClicked;
            layout.Children.Add(new ContentView { Padding = new Thickness(LeftBorder, 10, LeftBorder, 0), HorizontalOptions = LayoutOptions.Center, Content = skipLoginBtn });

            var stackLayout = new StackLayout { Spacing = 10, Orientation = StackOrientation.Horizontal, Padding = new Thickness(0, 30, 0, 0) };

            var forgetPasswordLayout = new ContentView
            {
                BackgroundColor = AppResources.LoginBottomButtonColor,
                HeightRequest = 60,
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width / 2),
                Content = new Label
                {
                    FontSize = 16,
                    Text = AppResources.LoginForgetPassword,
                    TextColor = AppResources.LoginBottomButtonTextColor,
                    XAlign = TextAlignment.Center,
                    YAlign = TextAlignment.Center
                },
                HorizontalOptions = LayoutOptions.Fill
            };
            var forgetPassRecognizer = new TapGestureRecognizer();
            forgetPassRecognizer.Tapped += OnForgetPasswordClicked;
            forgetPasswordLayout.GestureRecognizers.Add(forgetPassRecognizer);

            var signupLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 60,
                BackgroundColor = AppResources.LoginBottomButtonColor,
                Orientation = StackOrientation.Horizontal,
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width / 2 - 10)
            };

            signupLayout.Children.Add(new ContentView
            {
                Content = new Image
                {
                    Source = ImageLoader.Instance.GetImage(AppResources.SignUpButtonImage, false),
                    HeightRequest = 40
                },
                Padding = new Thickness(10, 10, 10, 5)
            });
            signupLayout.Children.Add(new Label
            {
                FontSize = 16,
                Text = AppResources.LoginCreateUser,
                TextColor = AppResources.LoginBottomButtonTextColor,
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Center
            });

            var signupRecognizer = new TapGestureRecognizer();
            signupRecognizer.Tapped += OnSignUpClicked;
            signupLayout.GestureRecognizers.Add(signupRecognizer);

            stackLayout.Children.Add(forgetPasswordLayout);
            stackLayout.Children.Add(signupLayout);

            layout.Children.Add(stackLayout);

            relatLayout.Children.Add(layout,
                Constraint.Constant(1),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            /*
            relatLayout.Children.Add(layout, Constraint.RelativeToParent((parent) => {
                return parent.X;
            }), Constraint.RelativeToParent((parent) => {
                return parent.Y * .15;
            }), Constraint.RelativeToParent((parent) => {
                return parent.Width;
            }), Constraint.RelativeToParent((parent) => {
                return parent.Height * .8;
            }));
            */

            Content = new ScrollView { Content = relatLayout };
        }

        private void OnLoginSkipClicked(object sender, EventArgs e)
        {
            if (LogIn != null)
                LogIn(null);
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            if (LogIn != null)
                LogIn(new LoginUserData { Email = _emailEntry.Text, Password = _passwordEntry.Text });
        }

        private void OnForgetPasswordClicked(object sender, EventArgs e)
        {
            AppController.Instance.TryResetPassword();
        }

        private void OnSignUpClicked(object sender, EventArgs e)
        {
            if (SignUp != null)
                SignUp();
        }

        private AbsoluteLayout GetEntry(Keyboard keyboard, string defaultValue, string icon, int topPadding, bool isPassword, int iconHeight, out ExtendedEntry textEntry)
        {
            var layout = new AbsoluteLayout { Padding = new Thickness(LeftBorder, topPadding, 0, 0) };

            layout.Children.Add(new Image { Source = ImageLoader.Instance.GetImage(icon, false), HeightRequest = iconHeight }, new Point(5, 3));

            textEntry = new ExtendedEntry
            {
                Keyboard = keyboard,
                Placeholder = defaultValue,
                TextColor = AppResources.LoginActiveTextColor,
                BackgroundColor = Color.Transparent,
                HasBorder = false,
                IsPassword = isPassword,
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width - 30 * 2 - 30),
                Font = Font.SystemFontOfSize(12)
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

        public void InitWithData(string email, string password)
        {
            _emailEntry.Text = email;
            _passwordEntry.Text = password;
        }
    }
}