using System;
using Conarh_2016.Application.Domain.PostData;
using Xamarin.Auth;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Conarh_2016.Application;
using UIKit;


namespace Conarh_2016.iOS.Services
{
    public class LinkedinLogin : Core.Services.ILinkedinLogin
    {
        public UIViewController loginScreenVC { get; private set; }

        public void createUserLinkedin()
        {
            CreateUserData udata = new CreateUserData();
            var auth = new OAuth2Authenticator(
               clientId: "77c1rz71bj79xe",
               clientSecret: "ixk8ykW2zSTqYLes",
               scope: "r_basicprofile r_emailaddress",
               authorizeUrl: new Uri("https://www.linkedin.com/uas/oauth2/authorization"),
               redirectUrl: new Uri("http://www.i9acao.com.br/"),
               accessTokenUrl: new Uri("https://www.linkedin.com/uas/oauth2/accessToken")
            );

            
            
            var items = loginScreenVC.NavigationItem;
            
            

            

            auth.AllowCancel = true;
            //auth.GetUI();
            ViewController.PresentViewController(auth.GetUI(), true, null);
            //PresentViewController(auth.GetUI(), true, null);
            ///Forms.Context.StartActivity(auth.GetUI(Forms.Context));
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
                                udata.Job = (string)result["headline"];
                                udata.Email = (string)result["emailAddress"];
                                udata.Name = (string)result["firstName"] + " " + (string)result["lastName"];
                                udata.Password = access_token;
                                udata.Phone = " ";
                                udata.ProfileImage = (string)result["pictureUrl"];
                                imageServerPath = (string)result["pictureUrl"];
                                udata.ScorePoints = 0;

                                UserController.Instance.RegisterUserLinkedin(udata, imageServerPath);
                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        System.Console.WriteLine(exx.ToString());
                    }
                }
            };

        }

        private UIViewController ViewController
        {
            get
            {
                return UIApplication.SharedApplication.KeyWindow.RootViewController;
            }
        }


        public void openPage()
        {
            throw new NotImplementedException();
        }
    }

}