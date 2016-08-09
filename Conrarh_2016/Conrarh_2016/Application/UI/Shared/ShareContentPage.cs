using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Shared
{
	public abstract class ShareContentPage:ContentPage
	{
		private void OnShare()
		{
			UserController.Instance.Share ();
		}

		protected ShareContentPage ():base()
		{
            /* TODO  - Reativar share no IOS* ShareContentPage */
            //if (Device.OS != TargetPlatform.iOS)
            //{
                ToolbarItems.Add(new ToolbarItem("share", AppResources.ShareBtnImage, OnShare, ToolbarItemOrder.Default, 0));
            //}
		}

	}
}

