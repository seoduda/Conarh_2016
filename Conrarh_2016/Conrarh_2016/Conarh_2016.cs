using Conarh_2016.Application.UI.Main;

namespace Conarh_2016
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            MainPage = new RootPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}