using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Core;
using Conrarh_2016.Application.UI.Shared;
using Conrarh_2016.Core.DataAccess;
//using KinveyXamarin;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
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

            var layout = new StackLayout { BackgroundColor = Color.Transparent };

            BoxView transpSpace = new BoxView();
            transpSpace.Color = Color.Transparent;
            transpSpace.WidthRequest = 1;
            transpSpace.HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height/4);
            
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
                BackgroundColor = AppResources.MenuColor,
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

            
            layout.Children.Add(loginLinkedInBtn);

            var stackLayout = new StackLayout { Spacing = 5, Orientation = StackOrientation.Horizontal, Padding = new Thickness(0, 30, 0, 0) };

            var forgetPasswordLayout = new ContentView
            {
                BackgroundColor = AppResources.MenuColor,
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
                BackgroundColor = AppResources.MenuColor,
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


            BGLayoutView bgLayout = new BGLayoutView(AppResources.LoginBgImage, layout, true, true);
            //Content = new ScrollView { Content = layout };
            Content = new ScrollView { Content = bgLayout };
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
            _emailEntry.TextColor = AppResources.LoginNormalTextColor;
            _emailEntry.Font = Font.SystemFontOfSize(14, FontAttributes.Bold);
            _passwordEntry.Text = password;
            _passwordEntry.TextColor = AppResources.LoginNormalTextColor;
            _passwordEntry.Font = Font.SystemFontOfSize(18, FontAttributes.Bold);

        }

        private void tapImage_Tapped(object sender, EventArgs e)
        {
            signupLinkedin();
            //DisplayAlert("Alert", "This is an image button", "OK");
        }

        private  void signupLinkedin()
        {
            
             AppProvider.LinkedinLogin.createUserLinkedin();
            //var l = udata.Name;
            //AppProvider.LinkedinLogin.openPage();

            /*
            //Client kinveyClient = new Client.Builder(Config.KinveyKey, Config.KinveySecret).build();
            //Client mKinveyClient = new Client.Builder(your_app_key, your_app_secret).build();
            //User user = await kinveyClient.User().lin
            //User user =  kinveyClient.User().LoginLinkedInAsync(AppResources.ApiLinkedinAccessTokenUrl,
                                                                     AppResources.ApiLinkedinAuthorizeUrl,
                                                                        AppResources.ApiLinkedinClientId,
                                                                        AppResources.ApiLinkedinClientSecret);
            /*
            var auth = new OAuth2Authenticator(
                       clientId: AppResources.ApiLinkedinClientId,
                       clientSecret: AppResources.ApiLinkedinClientSecret,
                       scope: AppResources.ApiLinkedinScope,
                       authorizeUrl: new Uri(AppResources.ApiLinkedinAuthorizeUrl),
                       redirectUrl: new Uri(AppResources.ApiLinkedinRedirectUrl),
                       accessTokenUrl: new Uri(AppResources.ApiLinkedinAccessTokenUrl)
                        );

            //var se = sender;
            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.AllowCancel = true;
            #if __ANDROID__
                StartActivity(auth.GetUI(this));
			
            #endif      



            auth.Completed += async (sender2, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    string dd = eventArgs.Account.Username;
                    var values = eventArgs.Account.Properties;

                    var access_token = values["access_token"];
                    try
                    {
                        var request = System.Net.HttpWebRequest.Create(string.Format(@"https://api.linkedin.com/v1/people/~:(firstName,lastName,headline,picture-url,positions,email-address )?oauth2_access_token=" + access_token + "&format=json", ""));
                        request.ContentType = "application/json";
                        request.Method = "GET";

                        //using (HttpWebResponse response = await request.GetResponseAsync() request.GetResponse() as HttpWebResponse)
                        using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                        {
                            //System.Console.Out.WriteLine("Stautus Code is: {0}", response.StatusCode);

                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                var content = reader.ReadToEnd();
                                if (!string.IsNullOrWhiteSpace(content))
                                {
                                    //System.Console.Out.WriteLine(content);
                                }
                                //infoText.Text = content.ToString();
                                var result = JsonConvert.DeserializeObject<dynamic>(content);
                                String info = (string)result["firstName"] + " " + (string)result["lastName"]
                                + " " + (string)result["headline"] + "-" + (string)result["emailAddress"] + "X" + (string)result["positions"]["values"][0]["company"]["name"];

                                
                                //infoText.Text = content.ToString();
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                       // System.Console.WriteLine(exx.ToString());
                    }
                }
            };
            */
        }
    }
}
 