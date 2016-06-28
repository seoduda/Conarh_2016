using System;
using Conarh_2016.Core.Services;
using Android.Content;
using Xamarin.Forms;

namespace Conarh_2016.Droid.Services
{
	public sealed class ShareServiceAndroid:ShareService
	{
		#region implemented abstract members of ShareService
		public override void ShareFacebook (string title, string status, string link, Action onShareDone)
		{
			ShareOnService (status, title, link, onShareDone);
		}
		public override void ShareTwitter (string title, string status, string link, Action onShareDone)
		{
			ShareOnService (status, title, link, onShareDone);
		}
		#endregion


		private void ShareOnService(string status, string title, string link, Action onShareDone)
		{
			var intent = new Intent(Intent.ActionSend);
			intent.PutExtra(Intent.ExtraText, String.Format("{0} - {1}",status ?? string.Empty,link ?? string.Empty));
			intent.PutExtra(Intent.ExtraSubject, title ?? string.Empty);
			intent.SetType("text/plain");
			intent.SetFlags(ActivityFlags.ClearTop);
			intent.SetFlags(ActivityFlags.NewTask);
			Forms.Context.StartActivity(intent);
		}
	}
}

