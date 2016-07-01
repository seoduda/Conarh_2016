using Conarh_2016.Application;
using System;
using Xamarin.Forms;

namespace Conrarh_2016.Application.UI.Shared
{
    internal class SponsorBannerView : ContentView
    {
        private Image bannerImage;
        private const int ImageHeight = 60;

        public SponsorBannerView()
        {
            Image bannerImage = new Image();
            bannerImage.Source = ImageLoader.Instance.GetImage(AppResources.SponsorBanner, true);
            bannerImage.Aspect = Aspect.AspectFill;
            var bannerRecognizer = new TapGestureRecognizer();
            bannerRecognizer.Tapped += OnBannerClicked;
            bannerImage.GestureRecognizers.Add(bannerRecognizer);
            HeightRequest = ImageHeight;
            Padding = new Thickness(10, 5, 10, 2);
            HorizontalOptions = LayoutOptions.Center;
            Content = bannerImage;
        }

        private void OnBannerClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(AppResources.SponsorUri));
        }
    }
}