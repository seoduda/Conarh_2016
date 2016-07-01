using Xamarin.Forms;
using Conarh_2016.Application.Domain;
using Conarh_2016.Core;
using Conarh_2016.Application.UI.Controls;

namespace Conarh_2016.Application.UI.Events
{
	public sealed class SpeecherDetailView:ContentPage
	{
		public readonly Speaker Data;
		const int FirstItemHeight = 120;
		const int SecondItemHeight = 80;
		const int PaddingTopFisrtLabel = 20;
		const int PaddingLeftFisrtLabel = 25;

		private DownloadedImage _speecherImage;

		public SpeecherDetailView (Speaker data, Color eventColor)
		{
			Data = data;
			Title = Data.Name;

			var absoluteLayout = new AbsoluteLayout {};

			absoluteLayout.Children.Add (new BoxView {
				WidthRequest = AppProvider.Screen.Width,
				HeightRequest = FirstItemHeight,
				BackgroundColor = AppResources.AgendaPageBackgroundColor
			});

			absoluteLayout.Children.Add (new BoxView {
				WidthRequest = 10,
				HeightRequest = FirstItemHeight,
				BackgroundColor = eventColor
			});

			absoluteLayout.Children.Add (new BoxView {
				WidthRequest = AppProvider.Screen.Width,
				HeightRequest = SecondItemHeight,
				BackgroundColor = AppResources.SpeecherBgColor
			}, new Point(0, FirstItemHeight));

			absoluteLayout.Children.Add (new Label {
				FontSize = 80,
				Text = AppResources.BioHeader,
				TextColor = AppResources.SpeecherTextColor
			}, new Point(125, 46));

			_speecherImage = new DownloadedImage (AppResources.DefaultUserImage) {
				HeightRequest = FirstItemHeight,
				WidthRequest = FirstItemHeight,
				Aspect = Aspect.AspectFill
			};

			absoluteLayout.Children.Add (_speecherImage, new Point(10, 0));

			var stackTextLayout = new StackLayout { Padding = new Thickness (PaddingLeftFisrtLabel, 10, 0, 0) };

			stackTextLayout.Children.Add (new Label {
				FontSize = 12,
				XAlign = TextAlignment.Start,
				Text = AppResources.Speakers.ToUpper(),
				TextColor = eventColor
			});

			stackTextLayout.Children.Add (new Label {
				FontAttributes = FontAttributes.Bold,
				FontSize = 15,
				XAlign = TextAlignment.Start,
				Text = Data.Name,
				TextColor = eventColor
			});

			absoluteLayout.Children.Add (stackTextLayout, new Point (0, FirstItemHeight));

			var descriptionContent = new ContentView {
				Content = new ContentView {
					Content = new Label {
						FontSize = 13,
						XAlign = TextAlignment.Start,
						Text = Data.Bio,
						TextColor = Color.Black},
					Padding = new Thickness(PaddingLeftFisrtLabel, PaddingLeftFisrtLabel / 2, PaddingLeftFisrtLabel, PaddingLeftFisrtLabel / 2)
				},
				BackgroundColor = AppResources.AgendaPageBackgroundColor
			};

			var stackLayout = new StackLayout { BackgroundColor = AppResources.AgendaPageBackgroundColor };
			stackLayout.Children.Add (absoluteLayout);
			stackLayout.Children.Add (descriptionContent);

			Content = new ScrollView {Content = stackLayout};
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			_speecherImage.UpdateAtTime = Data.UpdatedAtTime;
			_speecherImage.ServerImagePath = Data.ProfileImagePath;
		}
	}
}

