using Conarh_2016.Application.UI.Shared;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Map
{
	public sealed class MapPage : ShareContentPage
	{
		private readonly WebView _webView;
		public MapPage ():base()
		{
			Title = AppResources.Map;
			_webView = new WebView ();
			Content = _webView;
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

            /*
              <img src='https://sites.google.com/a/i9acao.com.br/projeto-conarh-2016/mapa/Mapa_Conarh_2016.jpg' style=""width:100%""/>
            */

            var htmlSource = new HtmlWebViewSource ();
            
            htmlSource.Html = @"<html>
			<head>
				<meta name=""viewport"" content=""width=device-width, initial-scale=2.0, maximum-scale=10.0 user-scalable=1"">
			</head>
			<body>
		   		 <img src='https://sites.google.com/a/i9acao.com.br/projeto-conarh-2016/mapa/Mapa_Conarh_2016.jpg' style=""width:100%""/>
		    </body>
			</html>";

			_webView.Source = htmlSource;
		}
	}

}
 