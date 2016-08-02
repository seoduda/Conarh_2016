using Conarh_2016.Core;
using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Shared
{
    internal class SponsorBannerView : ContentView
    {
        private float ImageHeight = AppProvider.Screen.ConvertPixelsToDp(90);

        public SponsorBannerView()
        {
            Image bannerImage = new Image();
            bannerImage.Source = ImageLoader.Instance.GetImage(AppResources.SponsorBanner, true);
            bannerImage.Aspect = Aspect.AspectFit;
            var bannerRecognizer = new TapGestureRecognizer();
            bannerRecognizer.Tapped += OnBannerClicked;
            bannerImage.GestureRecognizers.Add(bannerRecognizer);
            HeightRequest = ImageHeight;
            WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width);
           // Padding = new Thickness(2, 2, 2, 2);
            HorizontalOptions = LayoutOptions.Center;
            Content = bannerImage;
            BackgroundColor = Color.White;
        }

        private void OnBannerClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(AppResources.SponsorUri));
        }
    }
}