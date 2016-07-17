using Android.App;
using Android.OS;
using Android.Content.PM;

namespace Conarh_2016.Droid
{
	using System.Threading;

	[Activity (Label = "CONARH 2016", MainLauncher=true, NoHistory=true, Theme="@style/Theme.SplashActivity", ScreenOrientation = ScreenOrientation.Portrait)]
	public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Wait for 3,3 seconds
			Thread.Sleep(3300); 

			//Moving to next activity
			StartActivity(typeof(MainActivity));
		}
	}
}