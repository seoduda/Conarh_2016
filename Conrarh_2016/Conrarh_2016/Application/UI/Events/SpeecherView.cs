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
			var absoluteLayout = new AbsoluteLayout {Padding = new Thickness(0, 5, 0, 5)};

			absoluteLayout.Children.Add (new BoxView {
				WidthRequest = AppProvider.Screen.Width,
				HeightRequest = ItemHeight,
				BackgroundColor = AppResources.SpeecherBackColor
			});

			absoluteLayout.Children.Add (new BoxView {
				WidthRequest = 10,
				HeightRequest = ItemHeight,
				BackgroundColor = eventColor
			});

			var speecherImage = new DownloadedImage (AppResources.DefaultUserImage) {
				HeightRequest = ItemHeight,
				WidthRequest = ItemHeight,
				Aspect = Aspect.AspectFill
			};
			speecherImage.UpdateAtTime = Data.UpdatedAtTime;
			speecherImage.ServerImagePath = Data.ProfileImagePath;
			absoluteLayout.Children.Add (speecherImage, new Point(10, 0));

			absoluteLayout.Children.Add (new Label {
				FontSize = 10,
				XAlign = TextAlignment.Start,
				Text = AppResources.Speakers.ToUpper(),
				TextColor = eventColor
			}, new Point(85, 10));
					
			absoluteLayout.Children.Add (new Label () {
				FontAttributes = FontAttributes.Bold,
				FontSize = 14,
				XAlign = TextAlignment.Start,
				Text = Data.Name,
				TextColor = eventColor
			}, new Point(85, 30));


			Content = absoluteLayout;
			TapGestureRecognizer tapRecognizer = new TapGestureRecognizer ();
			tapRecognizer.Tapped += OnViewTapped;
			Content.GestureRecognizers.Add (tapRecognizer);
		}

		void OnViewTapped (object sender, EventArgs e)
		{
			if (SelectItem != null)
				SelectItem (Data);
		}
	}
}