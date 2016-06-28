using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.ComponentModel;
using Conarh_2016.Droid.UI;

[assembly: ExportRenderer(typeof(WebView), typeof(CustomWebViewRenderer))]
namespace Conarh_2016.Droid.UI
{
	public class CustomWebViewRenderer : WebViewRenderer
	{
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (Control != null)
			{
				Control.Settings.BuiltInZoomControls = true;
				Control.Settings.DisplayZoomControls = true;
			}
			base.OnElementPropertyChanged(sender, e);
		}

	}
}

