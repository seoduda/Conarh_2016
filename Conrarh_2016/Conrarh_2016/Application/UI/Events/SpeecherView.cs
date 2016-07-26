using Xamarin.Forms;
using Conarh_2016.Core;
using Conarh_2016.Application.Domain;
using System;
using Conarh_2016.Application.UI.Controls;

namespace Conarh_2016.Application.UI.Events
{
	public sealed class SpeecherView: ContentView
	{
		const float ItemHeight = 60;

		public readonly Speaker Data;
		public event Action<Speaker> SelectItem;

		public SpeecherView (Speaker data, Color eventColor)
		{
			Data = data;
			var absoluteLayout = new AbsoluteLayout {Padding = new Thickness(10, 5, 0, 5)};

			absoluteLayout.Children.Add (new BoxView {
				WidthRequest = AppProvider.Screen.Width,
				HeightRequest = ItemHeight,
				BackgroundColor = AppResources.SpeecherBgColor
			});

			absoluteLayout.Children.Add (new BoxView {
				WidthRequest = 10,
				HeightRequest = ItemHeight,
				BackgroundColor = eventColor
			});

            String speecherImagePath = String.IsNullOrEmpty(Data.ProfileImagePath.Trim()) ?
                AppResources.DefaultUserImage :
                Data.ProfileImagePath.Trim();

            var speecherImage = new DownloadedImage (speecherImagePath) {
				HeightRequest = ItemHeight,
				WidthRequest = ItemHeight,
				Aspect = Aspect.AspectFill
			};
			speecherImage.UpdateAtTime = Data.UpdatedAtTime;
			speecherImage.ServerImagePath = Data.ProfileImagePath;
			absoluteLayout.Children.Add (speecherImage, new Point(10, 0));

			absoluteLayout.Children.Add (new Label {
				FontSize = 12,
				HorizontalTextAlignment = TextAlignment.Start,
				Text = AppResources.Speakers.ToUpper(),
				TextColor = AppResources.SpeecherTextColor
            }, new Point(85, 10));
					
			absoluteLayout.Children.Add (new Label () {
				FontAttributes = FontAttributes.Bold,
				FontSize = 14,
                HorizontalTextAlignment = TextAlignment.Start,
				Text = Data.Name,
				TextColor = AppResources.SpeecherTextColor
            }, new Point(85, 30));


			Content = absoluteLayout;
            /* TODO - Speeker bio Desabilitado SpeecherView
			TapGestureRecognizer tapRecognizer = new TapGestureRecognizer ();
			tapRecognizer.Tapped += OnViewTapped;
			Content.GestureRecognizers.Add (tapRecognizer);
            */
        }

        void OnViewTapped (object sender, EventArgs e)
		{
			if (SelectItem != null)
				SelectItem (Data);
		}
	}
}