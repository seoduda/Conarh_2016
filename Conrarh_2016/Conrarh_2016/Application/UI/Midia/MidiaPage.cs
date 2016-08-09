using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Core;
using System;
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

            MediaItem mi4 = new MediaItem("Astrid Fontenelle convida você para o CONARH 2016!!");
            mi4.MediaType = MediaType.Video;
            mi4.ImagePath = AppResources.MidiaImage4;
            mi4.Url = "https://www.youtube.com/watch?v=dMJhu-5GXdY";



            StackLayout mdL1 = getMediaItemView(mi1.Title, mi1.ImagePath, mi1.Url);
            var MediaClickRecognizer1 = new TapGestureRecognizer();
            MediaClickRecognizer1.Tapped += OpenMedia1;
            mdL1.GestureRecognizers.Add(MediaClickRecognizer1);

            StackLayout mdL2 = getMediaItemView(mi2.Title, mi2.ImagePath, mi2.Url);
            var MediaClickRecognizer2 = new TapGestureRecognizer();
            MediaClickRecognizer2.Tapped += OpenMedia2;
            mdL2.GestureRecognizers.Add(MediaClickRecognizer2);

            StackLayout mdL3 = getMediaItemView(mi3.Title, mi3.ImagePath, mi3.Url);
            var MediaClickRecognizer3 = new TapGestureRecognizer();
            MediaClickRecognizer3.Tapped += OpenMedia3;
            mdL3.GestureRecognizers.Add(MediaClickRecognizer3);

            StackLayout mdL4 = getMediaItemView(mi4.Title, mi4.ImagePath, mi4.Url);
            var MediaClickRecognizer4 = new TapGestureRecognizer();
            MediaClickRecognizer4.Tapped += OpenMedia4;
            mdL4.GestureRecognizers.Add(MediaClickRecognizer4);


            var mediaLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 5,
                Padding = new Thickness(0)
            };
            
            //if (Device.OS != TargetPlatform.iOS)
            //{
                mediaLayout.Children.Add(new ContentView { Content = mdL1 });
                mediaLayout.Children.Add(new ContentView { Content = mdL2 });
                mediaLayout.Children.Add(new ContentView { Content = mdL3 });
                mediaLayout.Children.Add(new ContentView { Content = mdL4 });

            //}
            /*
            else
            {
                var MediaBtn1 = new Button
                {
                    BorderRadius = 5,
                    //HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height/10),
                    WidthRequest = AppProvider.Screen.ConvertPixelsToDp((AppProvider.Screen.Width / 4) * 3),
                    HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 14),
                    TextColor = Color.White,
                    BackgroundColor = AppResources.MenuColor,
                    Text = AppResources.OpenVideoMedia,
                    FontSize = 16
                };
                MediaBtn1.Clicked += OpenMedia1;

                var MediaBtn2 = new Button
                {
                    BorderRadius = 5,
                    //HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height/10),
                    WidthRequest = AppProvider.Screen.ConvertPixelsToDp((AppProvider.Screen.Width / 4) * 3),
                    HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 14),
                    TextColor = Color.White,
                    BackgroundColor = AppResources.MenuColor,
                    Text = AppResources.OpenVideoMedia,
                    FontSize = 16
                };
                MediaBtn2.Clicked += OpenMedia2;

                var MediaBtn3 = new Button
                {
                    BorderRadius = 5,
                    //HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height/10),
                    WidthRequest = AppProvider.Screen.ConvertPixelsToDp((AppProvider.Screen.Width / 4) * 3),
                    HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 14),
                    TextColor = Color.White,
                    BackgroundColor = AppResources.MenuColor,
                    Text = AppResources.OpenVideoMedia,
                    FontSize = 16
                };
                MediaBtn3.Clicked += OpenMedia3;

                mediaLayout.Children.Add(new ContentView { Content = mdL1 });
               // mediaLayout.Children.Add(MediaBtn1);
                mediaLayout.Children.Add(new ContentView { Content = mdL2 });
               // mediaLayout.Children.Add(MediaBtn2);
                mediaLayout.Children.Add(new ContentView { Content = mdL3 });
                //mediaLayout.Children.Add(MediaBtn3);
            }
            */
            layout.Children.Add(mediaLayout);


            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, true, true);
            Content = new ScrollView { Content = bgLayout };
        }




        private StackLayout getMediaItemView(String _title, String _imagePath, String _url)
        {
            StackLayout linearLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = AppResources.AgendaExpoColor,
                Padding = new Thickness(10, 1, 10, 1),
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
            };
            linearLayout.Children.Add(mediaImage);

            /*
            var MediaImageClick = new TapGestureRecognizer();
            MediaImageClick.Tapped += OpenMedia2;
            mediaImage.GestureRecognizers.Add(MediaImageClick);
            */

            Label mediaLabel = new Label()
            {
                Text = _title,
                FontSize = 16,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center
            };
            linearLayout.Children.Add(mediaLabel);

            return linearLayout;
        }


        /*
        private void OnButtonClicked(object sender, EventArgs e, String url)
        {
            Device.OpenUri(new Uri(url));
            // Device.OpenUri(new Uri("https://www.youtube.com/watch?v=Ec-drZPLRco"));
        }
        */

        private void OpenMedia1(object sender, EventArgs e)
        {
             Device.OpenUri(new Uri("https://www.youtube.com/watch?v=Ec-drZPLRco"));
        }

        private void OpenMedia2(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.youtube.com/watch?v=5HgKtL3SEwY"));
        }

        private void OpenMedia3(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.youtube.com/watch?v=ngN5CImS_bA"));
        }

        private void OpenMedia4(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.youtube.com/watch?v=dMJhu-5GXdY"));
        }


    }
}