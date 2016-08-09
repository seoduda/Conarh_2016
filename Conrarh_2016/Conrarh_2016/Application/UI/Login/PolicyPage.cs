using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Login
{
       public class PolicyPage : ContentPage
    {
        public PolicyPage()
        {
            Title = "Política de Privacidade";
            UrlWebViewSource source = new UrlWebViewSource();

            source.Url = "https://sites.google.com/a/i9acao.com.br/termos-de-servico-conarh-2/politica-de-privacidade";

            Content = new WebView { Source = source };
        }
    }
}
