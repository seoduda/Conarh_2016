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
        //private int LBorder = 30;

        public event Action SignUp;

        public event Action<LoginUserData> LogIn;

        public LoginView() : base()
        {
            LeftBorder = AppProvider.Screen.ConvertPixelsToDp(20);

            RelativeLayout relatlay = new RelativeLayout
            {
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width)
                // BackgroundColor =
            };
            Image bg = new Image()
            {
                Source = ImageLoader.Instance.GetImage(AppResources.LoginBgImage, false),
                Aspect = Aspect.AspectFill
            };

            relatlay.Children.Add(bg,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            var layout = new StackLayout { BackgroundColor = Color.Transparent };

            BoxView transpSpace = new BoxView();
            transpSpace.Color = Color.Transparent;
            transpSpace.WidthRequest = 1;
            transpSpace.HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height/5);
            
            layout.Children.Add(transpSpace);

            layout.Children.Add(GetEntry(Keyboard.Email, AppResources.LoginEmailDefaultEntry, AppResources.LoginEmailImage, 15, false, 15, out _emailEntry));
            layout.Children.Add(GetEntry(Keyboard.Text, AppResources.LoginPasswordDefaultEntry, AppResources.LoginPasswordImage, 10, true, 10, out _passwordEntry));

            var loginBtn = new Button
            {
                BorderRadius = 5,
                //HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height/10),
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp((AppProvider.Screen.Width/4)*3),
                HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 14),
                TextColor = Color.White,
                BackgroundColor = AppResources.MenuGreen,
                Text = AppResources.Login,
                FontSize = 16
            };
            loginBtn.Clicked += OnLoginClicked;
            layout.Children.Add(new ContentView { Padding = new Thickness(LeftBorder, 10, LeftBorder, 0), HorizontalOptions = LayoutOptions.Center, Content = loginBtn});

            /** 
             * skipLoginBtn retirado e substituido por login Linkedin
             */
            Image loginLinkedInBtn = new Image()
            {
                Source = ImageLoader.Instance.GetImage(AppResources.LoginBtLinkedin, false),
                HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 12),
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp((AppProvider.Screen.Width / 4) * 3),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var loginLinkedInRecognizer = new TapGestureRecognizer();
            //Binding events
            loginLinkedInRecognizer.Tapped += tapImage_Tapped;
            //Associating tap events to the image buttons
            loginLinkedInBtn.GestureRecognizers.Add(loginLinkedInRecognizer);

            //layout.Children.Add(new ContentView { HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 12), HorizontalOptions = LayoutOptions.Center, Content = loginLinkedInBtn  });
            layout.Children.Add(loginLinkedInBtn);

            var stackLayout = new StackLayout { Spacing = 5, Orientation = StackOrientation.Horizontal, Padding = new Thickness(0, 30, 0, 0) };

            var forgetPasswordLayout = new ContentView
            {
                BackgroundColor = AppResources.MenuGreen,
                HeightRequest = 40,
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width / 2),
                Content = new Label
                {
                    FontSize = 16,
                    Text = AppResources.LoginForgetPassword,
                    TextColor = Color.White,
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
                HeightRequest = 40,
                BackgroundColor = AppResources.MenuGreen,
                Orientation = StackOrientation.Horizontal,
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width / 2 - 10)
            };

            signupLayout.Children.Add(new ContentView
            {
                Content = new Image
                {
                    Source = ImageLoader.Instance.GetImage(AppResources.SignUpButtonImage, false),
                    HeightRequest = 35
                },
                Padding = new Thickness(10, 5, 10, 5)
            });
            signupLayout.Children.Add(new Label
            {
                FontSize = 16,
                Text = AppResources.LoginCreateUser,
                TextColor = Color.White,
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Center
            });

            var signupRecognizer = new TapGestureRecognizer();
            signupRecognizer.Tapped += OnSignUpClicked;
            signupLayout.GestureRecognizers.Add(signupRecognizer);

            stackLayout.Children.Add(forgetPasswordLayout);
            stackLayout.Children.Add(signupLayout);

            layout.Children.Add(stackLayout);


            Image bannerImage  = new Image();
            bannerImage.Source = ImageLoader.Instance.GetImage(AppResources.SponsorBanner, true);

            var bannerRecognizer = new TapGestureRecognizer();
            bannerRecognizer.Tapped += OnBannerClicked;
            bannerImage.GestureRecognizers.Add(bannerRecognizer);
            //layout.Children.Add(bannerImage);

            layout.Children.Add(new ContentView { HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 10), Padding = new Thickness(LeftBorder, 5, LeftBorder, 0), HorizontalOptions = LayoutOptions.Center, Content = bannerImage });

            relatlay.Children.Add(layout,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));



            //Content = new ScrollView { Content = layout };
            Content = new ScrollView { Content = relatlay };
        }

        private void OnBannerClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(AppResources.SponsorUri));
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

        public void InitWithData(string email, string password)
        {
            _emailEntry.Text = email;
            _emailEntry.TextColor = Color.Black;
            _emailEntry.Font = Font.SystemFontOfSize(18, FontAttributes.Bold);
            _passwordEntry.Text = password;
            _passwordEntry.TextColor = Color.Black;
            _passwordEntry.Font = Font.SystemFontOfSize(18, FontAttributes.Bold);

        }

        private void tapImage_Tapped(object sender, EventArgs e)
        {
            // handle the tap
            //DisplayAlert("Alert", "This is an image button", "OK");
        }
    }
}
 