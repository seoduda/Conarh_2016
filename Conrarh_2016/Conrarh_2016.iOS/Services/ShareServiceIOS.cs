using System;
using Conarh_2016.Core.Services;
using Foundation;
using Social;
using UIKit;

namespace Conarh_2016.iOS.Services
{  
	public class ShareServiceIOS : ShareService 
	{
		#region implemented abstract members of SharingService


		public override void ShareFacebook (string title, string status, string link, Action onShareDone)
		{
			ShareOnService (SLServiceKind.Facebook, title, status, link, onShareDone);
		}


		public override void ShareTwitter (string title, string status, string link, Action onShareDone)
		{
			ShareOnService (SLServiceKind.Twitter, title, status, link, onShareDone);
		}


		#endregion

		private void ShareOnService(SLServiceKind service, string title, string status, string link, Action onShareDone)  
		{  
			if (SLComposeViewController.IsAvailable(service))  
			{  
				var slComposer = SLComposeViewController.FromService(service); 
				slComposer.Title = title;
				slComposer.SetInitialText(status);  
				slComposer.AddUrl(new NSUrl(link));  

				slComposer.CompletionHandler += (result) => {
					if(result == SLComposeViewControllerResult.Done && onShareDone != null)
						onShareDone.Invoke();
				};

				ViewController.PresentViewController(slComposer, true, null);  
			}  
		}  

		private UIViewController ViewController
		{
			get {
				return UIApplication.SharedApplication.KeyWindow.RootViewController;
			}
		}
	}  
}  