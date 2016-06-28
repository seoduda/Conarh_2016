using System;
using Conarh_2016.Core.UI;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI
{
	public class PopUpFactory:IPopUpFactory
	{
		#region IPopUpFactory implementation
	
		public void ShowMessage (string message, string header, string buttonKey = "", Action action = null)
		{
			Device.BeginInvokeOnMainThread (() => MainPage.DisplayAlert (header, message, "OK"));
		}

		public void ShowConfirmation(string message, string header, Action yesAction, Action noAction, string yesBtnName, string noBtnName)
		{
			Device.BeginInvokeOnMainThread (() => 
				{
					MainPage.DisplayAlert (header, message, yesBtnName, noBtnName).ContinueWith((delegate(System.Threading.Tasks.Task<bool> task) {

						if(task.Result && yesAction != null)
							yesAction.Invoke();
						else if(!task.Result && noAction != null)
							noAction.Invoke();
					}));

				});
		}
	
		public Page MainPage { get { return AppController.Instance.AppRootPage; } }
		#endregion
		
	}
}

