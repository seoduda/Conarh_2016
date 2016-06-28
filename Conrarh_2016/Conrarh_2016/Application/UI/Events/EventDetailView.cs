using System;
using Xamarin.Forms;
using Conarh_2016.Core;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.UI.Controls;

namespace Conarh_2016.Application.UI.Events
{
	public sealed class EventDetailView: ContentPage
	{
		public readonly EventData Data;

		public EventDetailView(EventData data)
		{
			Data = data;

			Title = AppResources.EventDetailsHeader.ToUpper();
			BackgroundColor = AppResources.AgendaPageBackgroundColor;

			StackLayout layout = new StackLayout () { VerticalOptions = LayoutOptions.StartAndExpand };

			EventView eventView = new EventView ();
			eventView.BindingContext = Data;
			layout.Children.Add (eventView);

			if (!Data.FreeAttending && (!string.IsNullOrEmpty(Data.PointsImagePath) || !string.IsNullOrEmpty(Data.SponsorImagePath)) )
			{
				var whiteBoxLayout = new AbsoluteLayout { Padding = new Thickness (0, -10, 0, 0) };
				whiteBoxLayout.Children.Add (new BoxView {
					WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width),
					HeightRequest = 60,
					BackgroundColor = Color.White,
				});

				var imagesStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal,
					WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width),
					Spacing = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - 60 * 2,
					Padding = new Thickness(5)
				};

				if (!string.IsNullOrEmpty (Data.PointsImagePath)) {
					var pointsImage = new DownloadedImage (AppResources.DefaultPointsImage) { HeightRequest = 50 };
					pointsImage.UpdateAtTime = Data.UpdatedAtTime;
					pointsImage.ServerImagePath = Data.PointsImagePath;
					imagesStackLayout.Children.Add (pointsImage);
				}

				if (!string.IsNullOrEmpty (Data.SponsorImagePath)) {
					var sponsorImage = new DownloadedImage (AppResources.DefaultSponsorImage) { HeightRequest = 50 };
					sponsorImage.UpdateAtTime = Data.UpdatedAtTime;
					sponsorImage.ServerImagePath = Data.SponsorImagePath;
					imagesStackLayout.Children.Add (sponsorImage);
				}

				whiteBoxLayout.Children.Add (imagesStackLayout);
				layout.Children.Add (whiteBoxLayout);
			}

			var descriptionContent = new ContentView () {
				Content = new Label {
					FontSize = 11,
					TextColor = Color.Black,
					Text = Data.Description
				},
				Padding = new Thickness (10, 10, 0, 0)
			};
			layout.Children.Add (descriptionContent);

			foreach (Speaker speecherData in Data.Speechers)
			{
				layout.Children.Add (GetSpeecherItem(speecherData));
			}

			if (!Data.FreeAttending) 
			{
				var absoluteBtnLayout = new AbsoluteLayout { HorizontalOptions = LayoutOptions.Center,
					Padding = new Thickness(0, 0, 0, 20)};

				var eventsActionBtn = new Button { 
					BorderRadius = 25,
					HeightRequest = 50,
					WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width / 2),
					BackgroundColor = Data.BackgroundColorNonOpacity
				};
				absoluteBtnLayout.Children.Add (eventsActionBtn);

				var btnLabel = new Label { 
					WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width / 2) - 50,
					Text = AppResources.EventsActionBtnHeader,
					TextColor = AppResources.SpeecherBackColor,
					FontSize = 15
				};
				absoluteBtnLayout.Children.Add (btnLabel, new Point(50, 6));

				var btnIcon = new Image { 
					Source =  ImageLoader.Instance.GetImage(AppResources.EventButtonActionImage, true),
					HeightRequest = 40
				};
				absoluteBtnLayout.Children.Add (btnIcon, new Point(20, 6));

				if (Device.OS == TargetPlatform.iOS) {
					TapGestureRecognizer gesture = new TapGestureRecognizer ();
					gesture.Tapped += OnEventsActionsClicked;
					absoluteBtnLayout.GestureRecognizers.Add (gesture);
				}
				else
					eventsActionBtn.Clicked += OnEventsActionsClicked;

				layout.Children.Add (absoluteBtnLayout);
			}

			Content = new ScrollView {Content = layout};
		}
			
		private SpeecherView GetSpeecherItem(Speaker speecherData)
		{
			var item = new SpeecherView (speecherData, Data.BackgroundColorNonOpacity);
			item.SelectItem += OnSpeecherSelected;
			return item;
		}

		private void OnEventsActionsClicked (object sender, EventArgs e)
		{
			Navigation.PushAsync (new EventActionsView (
				new UserEventActionsModel(Data, AppModel.Instance.GetVoteDataByEvent(Data))));
		}

		private void OnSpeecherSelected (Speaker speecherData)
		{
			Navigation.PushAsync (new SpeecherDetailView (speecherData, Data.BackgroundColorNonOpacity));
		}
	}
}

