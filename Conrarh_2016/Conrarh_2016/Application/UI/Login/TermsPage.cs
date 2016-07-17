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
            //source.Url = "https://dl.dropboxusercontent.com/u/8812136/CONARH/Termos%20de%20Uso.html";
            source.Url = "https://sites.google.com/a/i9acao.com.br/termos-de-servico-conarh-2/";

            Content = new WebView { Source = source };
		}
	}
}

