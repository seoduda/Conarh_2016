using Conarh_2016.Core;
using Conrarh_2016.Application.UI.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Midia
{
    public enum MediaType
    {
        Video,
        Image
    }

    public class MediaItem
    {
        public String Title;
        public String ImagePath;
        public String Url;
        public MediaType MediaType;

        public MediaItem(String title)
        {
            Title = title;
        }

    }

    public class MediaPage : ContentPage
    {
        private StackLayout layout;

        public MediaPage() : base()
        {
            Title = AppResources.Midia;
            layout = new StackLayout { BackgroundColor = Color.Transparent };

           // _mediaList = GetMediaList();

            MediaItem mi1 = new MediaItem("Cultura que inova e se transforma - Lilian Guimarães");
            mi1.MediaType = MediaType.Video;
            mi1.ImagePath = AppResources.MidiaImage1;
            mi1.Url = "https://www.youtube.com/watch?v=Ec-drZPLRco";

            MediaItem mi2 = new MediaItem("Geraldo Carbone no CONARH 2016 - Os próximos anos do Brasil");
            mi2.MediaType = MediaType.Video;
            mi2.ImagePath = AppResources.MidiaImage2;
            mi2.Url = "https://www.youtube.com/watch?v=5HgKtL3SEwY";


            MediaItem mi3 = new MediaItem("CONARH 2016 - O que faz a ética...E o que faz a falta dela");
            mi3.MediaType = MediaType.Video;
            mi3.ImagePath = AppResources.MidiaImage3;
            mi3.Url = "https://www.youtube.com/watch?v=ngN5CImS_bA";
 

            /*
            layout.Children.Add(getMediaItemView(mi1));
            layout.Children.Add(getMediaItemView(mi2));
            layout.Children.Add(getMediaItemView(mi3));
            */

            var mediaLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 5,
                //HorizontalOptions = LayoutOptions.Fill,
                Children =  {
                    new ContentView {Content = getMediaItemView(mi1.Title,mi1.ImagePath,mi1.Url)}   ,
                    new ContentView {Content = getMediaItemView(mi2.Title,mi2.ImagePath,mi2.Url)}   ,
                    new ContentView {Content = getMediaItemView(mi3.Title,mi3.ImagePath,mi3.Url)}   ,
                },
                Padding = new Thickness(0)
            };

            layout.Children.Add(mediaLayout);
            Content = layout;
            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, true, true);
            Content = new ScrollView { Content = bgLayout };
        }

        private List<MediaItem> GetMediaList()
        {
            throw new NotImplementedException();
        }

        private View getMediaItemView(String _title, String _imagePath, String _url)
        {
            StackLayout linearLayout = new StackLayout()
            {

                Orientation = StackOrientation.Horizontal,
                BackgroundColor = AppResources.AgendaExpoColor,
                Padding = new Thickness(10, 1, 10, 1),
                //Spacing = 10,
                Opacity = 0.75,
                WidthRequest = AppProvider.Screen.Width - 20,
                HorizontalOptions = LayoutOptions.Center,
            };

            BoxView box = new BoxView()
            {
                Color = AppResources.MenuColor,
                WidthRequest = 10,

            };
            linearLayout.Children.Add(box);
            Image mediaImage = new Image
            {
                Source = ImageLoader.Instance.GetImage(_imagePath, false),
                HeightRequest = 45,
                WidthRequest = 80,
                //WidthRequest = AppProvider.Screen.ConvertPixelsToDp(100),
                //HorizontalOptions = LayoutOptions.CenterAndExpand,
                //VerticalOptions = LayoutOptions.CenterAndExpand

            };
            linearLayout.Children.Add(mediaImage);

            Label mediaLabel = new Label()
            {
                Text = _title,
                FontSize = 16,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center
            };
            linearLayout.Children.Add(mediaLabel);

            var MediaClickRecognizer = new TapGestureRecognizer();
            MediaClickRecognizer.Tapped += (sender, EventArgs) => { OnButtonClicked(sender, EventArgs, _url); };
            linearLayout.GestureRecognizers.Add(MediaClickRecognizer);


            return linearLayout;
        }

        private void OnButtonClicked(object sender, EventArgs e, String url)
        {
            Device.OpenUri(new Uri(url));
            // Device.OpenUri(new Uri("https://www.youtube.com/watch?v=Ec-drZPLRco"));
        }

        
        private static string addQuotes(string str)
        {
            StringBuilder sb = new StringBuilder("\"");
            sb.Append(str);
            sb.Append("\"");
            return sb.ToString();
        }

        private static string FnGetVideoID(string strVideoURL)
        {
            const string regExpPattern = @"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)";
            //for Vimeo: vimeo\.com/(?:.*#|.*/videos/)?([0-9]+)
            var regEx = new Regex(regExpPattern);
            var match = regEx.Match(strVideoURL);
            return match.Success ? match.Groups[1].Value : null;
        }

    }
}