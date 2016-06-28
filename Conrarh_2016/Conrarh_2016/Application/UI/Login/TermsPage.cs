using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Login
{
	public class TermsPage:ContentPage
	{
		public TermsPage ()
		{
			Title = "Termos de Uso";
			UrlWebViewSource source = new UrlWebViewSource ();
			source.Url = "https://dl.dropboxusercontent.com/u/8812136/CONARH/Termos%20de%20Uso.html";

			Content = new WebView { Source = source };
		}
	}
}

