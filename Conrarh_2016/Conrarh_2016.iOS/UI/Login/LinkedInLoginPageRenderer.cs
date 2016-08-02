using Conarh_2016.Application;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.UI.Login;
using Conarh_2016.Application.UI.Main;
using Conarh_2016.iOS.UI.Login;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LinkedInLoginPage), typeof(LinkedInLoginPageRenderer))]

namespace Conarh_2016.iOS.UI.Login
{
    internal class LinkedInLoginPageRenderer : PageRenderer
    {
        private OAuth2Authenticator auth;
        private CreateUserData udata;
        private bool IsShown = false;

        public event Action PostSignUp;

        public override void ViewDidAppear(bool animated)
        {
            if (!IsShown)
            {
                IsShown = true;

                udata = new CreateUserData();
                base.ViewDidAppear(animated);

                auth = new OAuth2Authenticator(
                           clientId: "77c1rz71bj79xe",
                           clientSecret: "ixk8ykW2zSTqYLes",
                           scope: "r_basicprofile r_emailaddress",
                           authorizeUrl: new Uri("https://www.linkedin.com/uas/oauth2/authorization"),
                           redirectUrl: new Uri("https://www.i9acao.com.br/"),
                           accessTokenUrl: new Uri("https://www.linkedin.com/uas/oauth2/accessToken")
                   );
                auth.Completed += authCompleted;
                PresentViewController(auth.GetUI(), true, PostSignUp);
            }
        }

        private void authCompleted(object sender, AuthenticatorCompletedEventArgs eventArgs)
        {
            if (eventArgs.IsAuthenticated)

            {
                string dd = eventArgs.Account.Username;
                string imageServerPath;
                var values = eventArgs.Account.Properties;
                //
                var access_token = values["access_token"];
                try
                {
                    var request = System.Net.HttpWebRequest.Create(string.Format(@"https://api.linkedin.com/v1/people/~:(firstName,lastName,headline,picture-url,positions,email-address )?oauth2_access_token=" + access_token + "&format=json", ""));
                    request.ContentType = "application/json";
                    request.Method = "GET";

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        System.Console.Out.WriteLine("Stautus Code is: {0}", response.StatusCode);

                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            var content = reader.ReadToEnd();
                            if (!string.IsNullOrWhiteSpace(content))
                            {
                                System.Console.Out.WriteLine(content);
                            }

                            var result = JsonConvert.DeserializeObject<dynamic>(content);
                            Dictionary<string, string> map = decodeprettyJasonString(content);
                            udata.Job = map["headline"];
                            udata.Email = map["emailAddress"];
                            udata.Name = map["firstName"] + " " + map["lastName"];
                            udata.Password = access_token;
                            udata.Phone = " ";
                            udata.ProfileImage = map["pictureUrl"];
                            imageServerPath = map["pictureUrl"];
                            udata.ScorePoints = 0;

                            UserController.Instance.RegisterUserLinkedin(udata, imageServerPath);

                            //DismissViewController(true, null);
                            //AppController.Instance.SuccessfulLinkedinLoginAction();

                            //base.DismissViewControllerAsync(true);
                            AppController.Instance.SuccessfulLinkedinLoginAction();
                           // AppController.Instance.AppRootPage.CurrentPage.Navigation.PopAsync();
                            //AppController.Instance.AppRootPage.NavigateTo(MainMenuItemData.LoginPage, true);

                            //postSignupLinkedin();
                        }
                    }
                }
                catch (Exception exx)
                {
                    System.Console.WriteLine(exx.ToString());
                }
            }
            else
            {
                //DismissViewController(true, null);
            }
        }

        private void postSignupLinkedin()
        {
            //DismissViewController(true, null);
            //base.DangerousAutorelease();
            //Dispose();
        }

        public Dictionary<string, string> decodeprettyJasonString(string response)
        {
            //string response = "{\n  \"emailAddress\": \"eduardo.furtado@gmail.com\",\n  \"firstName\": \"Eduardo\",\n  \"headline\": \"IT Manager na i9Ação _ Jogar para Aprender\",\n  \"lastName\": \"Furtado\",\n  \"pictureUrl\": \"https://media.licdn.com/mpr/mprx/0_y_g-sBZOUC-8nkIapTfTsq4lRX6Sn3EaYFWSszeCX3-yW5gmr5SOqvpuzEQPz6wGg8x2vt99Aa3c\",\n  \"positions\": {\n    \"_total\": 2,\n    \"values\": [\n      {\n        \"company\": {\n          \"id\": 1278724,\n          \"industry\": \"Treinamento profissional\",\n          \"name\": \"i9Ação _ Jogar para Aprender\",\n          \"size\": \"11-50\",\n          \"type\": \"Privately Held\"\n        },\n        \"id\": 820165701,\n        \"isCurrent\": true,\n        \"location\": {\n          \"country\": {\n            \"code\": \"br\",\n            \"name\": \"Brazil\"\n          },\n          \"name\": \"São Paulo Area, Brazil\"\n        },\n        \"startDate\": {\n          \"month\": 3,\n          \"year\": 2016\n        },\n        \"summary\": \"Development of digital games, interactive learning and gamification solutions for different business needs: training simulations, skill development tools, team building, user engagement, etc. Development of mobile apps (for Android and iOS) with gamification elements. Promoted and implemented agile methodology, improving efficiency in the company internal processes.\",\n        \"title\": \"IT Manager\"\n      },\n      {\n        \"company\": {\n          \"id\": 1706163,\n          \"industry\": \"Construção\",\n          \"name\": \"Varias empresas\"\n        },\n        \"id\": 667257746,\n        \"isCurrent\": true,\n        \"location\": {\n          \"country\": {\n            \"code\": \"br\",\n            \"name\": \"Brazil\"\n          },\n          \"name\": \"São Paulo Area, Brazil\"\n        },\n        \"startDate\": {\n          \"month\": 9,\n          \"year\": 2011\n        },\n        \"summary\": \"Consulting to several companies (publishing houses, ecommerce and retail). Lead infrastructure restructuring projects, optimizing the use of resources and reducing costs by 40%. Implemented a new data lifecycle management policy. Third-party management. Design and implementation of software solutions for digital processing of historical documents. Lead the development of new ecommerce functionalities and redesign.\",\n        \"title\": \"IT Consultant\"\n      }\n    ]\n  }\n}";
            string temp = response.Replace("\n", "");
            // temp = temp.Replace(" ", "");
            temp = temp.Replace("\"", "");
            temp = temp.Replace("https://", "https!//");
            temp = temp.Substring(1, temp.Length - 2);
            temp = processSubOjects(temp);
            Dictionary<string, string> decodedMap = new Dictionary<string, string>();
            string[] items = temp.Split(',');
            string[] pair;
            for (int i = 0; i < items.Length; i++)
            {
                pair = items[i].Split(':');
                decodedMap.Add(pair[0].Trim(), pair[1].Trim());
            }

           
            if (decodedMap.ContainsKey("pictureUrl"))
            {
                temp = decodedMap["pictureUrl"];
                temp = temp.Replace("https!//", "https://");
                decodedMap["pictureUrl"] = temp;
            }

            return decodedMap;
        }

        private string processSubOjects(string jsonString)
        {
            string tmp;
            int pos = jsonString.IndexOf('{');
            StringBuilder sb = new StringBuilder(jsonString.Substring(0, pos));
            tmp = jsonString.Substring(pos);
            tmp = tmp.Replace(":", "<*-*>");
            sb.Append(tmp.Replace(',', ';'));
            return sb.ToString();
        }
    }

    /*
    internal class LinkedInLoginPageRenderer : PageRenderer
    {
        private OAuth2Authenticator auth;
        private CreateUserData udata;

        public event Action PostSignUp;

        public override void ViewDidAppear(bool animated)
        {
            udata = new CreateUserData();
            base.ViewDidAppear(animated);

            auth = new OAuth2Authenticator(
                       clientId: "77c1rz71bj79xe",
                       clientSecret: "ixk8ykW2zSTqYLes",
                       scope: "r_basicprofile r_emailaddress",
                       authorizeUrl: new Uri("https://www.linkedin.com/uas/oauth2/authorization"),
                       redirectUrl: new Uri("https://www.i9acao.com.br/"),
                       accessTokenUrl: new Uri("https://www.linkedin.com/uas/oauth2/accessToken")
               );
            auth.Completed += (sender2, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    string dd = eventArgs.Account.Username;
                    string imageServerPath;
                    var values = eventArgs.Account.Properties;

                    var access_token = values["access_token"];
                    try
                    {
                        var request = System.Net.HttpWebRequest.Create(string.Format(@"https://api.linkedin.com/v1/people/~:(firstName,lastName,headline,picture-url,positions,email-address )?oauth2_access_token=" + access_token + "&format=json", ""));
                        request.ContentType = "application/json";
                        request.Method = "GET";

                        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                        {
                            System.Console.Out.WriteLine("Stautus Code is: {0}", response.StatusCode);

                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                var content = reader.ReadToEnd();
                                if (!string.IsNullOrWhiteSpace(content))
                                {
                                    System.Console.Out.WriteLine(content);
                                }

                                var result = JsonConvert.DeserializeObject<dynamic>(content);
                                Dictionary<string, string> map = decodeprettyJasonString(content);
                                udata.Job = map["headline"];
                                udata.Email = map["emailAddress"];
                                udata.Name = map["firstName"] + " " + map["lastName"];
                                udata.Password = access_token;
                                udata.Phone = " ";
                                udata.ProfileImage = map["pictureUrl"];
                                imageServerPath = map["pictureUrl"];
                                udata.ScorePoints = 0;

                                //this.DismissViewControllerAsync(true);
                                //this.RemoveFromParentViewController();
                                //this.DismissModalViewController(true);
                                //this.DismissViewController(true, PostSignUp);
                                //this.NavigationController.PopToRootViewController(true);
                                //this.NavigationController.PopViewController(true);
                                DismissViewController(true, null);
                                UserController.Instance.RegisterUserLinkedin(udata, imageServerPath);
                                AppController.Instance.SuccessfulLinkedinLoginAction();

                                //postSignupLinkedin();
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        System.Console.WriteLine(exx.ToString());
                    }
                }
                else
                {
                    DismissViewController(true, null);
                }
                DismissViewController(true, null);
            };

            PresentViewController(auth.GetUI(), true, postSignupLinkedin);
        }
        */
}