using Xamarin.Forms;
using Conarh_2016.Application.Domain;
using System.Collections.Generic;
using Conarh_2016.Core;

namespace Conarh_2016.Application.UI.Events
{
	public sealed class EventView: ContentView
	{
		public Image _favImage;
		public EventData Model;

		private BoxView _coloredBoxView;
		private Image _eventImage;
		private Label _nameLabel;
		private Label _locationLabel;
		private StackLayout _nameLocationLayout;
		private Button _timeBoxView;
		private Label _timeLabel;
		private Button _favEventBoxView;

		public EventView ()
		{
			BackgroundColor = Color.Transparent;

			_coloredBoxView = new BoxView () { WidthRequest = 10 };

			_eventImage = new Image { Aspect = Aspect.AspectFill, 
				WidthRequest = AppProvider.Screen.ConvertPixelsToDp (AppProvider.Screen.Width),
				HeightRequest = AppProvider.Screen.ConvertPixelsToDp (AppProvider.Screen.Height / 3)
			};

			var stackLayout = new StackLayout { Spacing = 0, Orientation = StackOrientation.Horizontal };
			stackLayout.Children.Add (_coloredBoxView);
			stackLayout.Children.Add (_eventImage);

			_nameLabel = new Label {
				FontAttributes = FontAttributes.Bold,
				FontSize = 13,
				TextColor = Color.White,
				XAlign = TextAlignment.Start
			};

			_locationLabel = new Label () 
			{
				FontSize = 9,
				TextColor = Color.White,
				XAlign = TextAlignment.Start
			};

			_nameLocationLayout = new StackLayout () {
				Orientation = StackOrientation.Vertical,
				Children = { 
					_nameLabel,
					_locationLabel
				},
				Padding = new Thickness(10, 4, 0, 10),
				Spacing = 10,
				WidthRequest  = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width)
			};

			var timeLayout = new AbsoluteLayout () {Padding = new Thickness(10, 130, 0, 0)};
			_timeBoxView = new Button () {
				WidthRequest = 110,
				HeightRequest = 25,
				BorderRadius = 12,
				IsEnabled = false
			};
			timeLayout.Children.Add (_timeBoxView);

			var timeClockImage = new Image () {
				WidthRequest = 21,
				HeightRequest = 21,
				Source = ImageLoader.Instance.GetImage(AppResources.EventClockImage, true)
			};
			timeLayout.Children.Add (timeClockImage, new Point(2, 2));

			_timeLabel = new Label () 
			{
				FontAttributes = FontAttributes.Bold,
				FontSize = 11,
				TextColor = Color.White,
				XAlign = TextAlignment.Start
			};
			timeLayout.Children.Add (_timeLabel, new Point(30, 5));

			_favEventBoxView = new Button () {
				WidthRequest = 35,
				HeightRequest = 35,
				BorderRadius = 17,
				IsEnabled = false
			};

			_favImage = new Image () {
				WidthRequest = 30,
				HeightRequest = 30
			};

			TapGestureRecognizer tapFavEvent = new TapGestureRecognizer ();
			tapFavEvent.Tapped += TapFavEvent_Tapped;
			_favImage.GestureRecognizers.Add (tapFavEvent);

			var favLayout = new AbsoluteLayout {
				Children = {_favEventBoxView},
				Padding = new Thickness(AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - 50, 120, 0, 0)
			};
			favLayout.Children.Add (_favImage, new Point (2.5f, 2.5f));

			var absoluteLayout = new AbsoluteLayout {Padding = new Thickness(0, 5)};
			absoluteLayout.Children.Add(stackLayout);

			absoluteLayout.Children.Add (_nameLocationLayout);
			absoluteLayout.Children.Add (timeLayout, new Point(20, 0));
			absoluteLayout.Children.Add (favLayout);

			Content = absoluteLayout;

			AppModel.Instance.UserChanged += OnUserChanged;
			if (AppModel.Instance.CurrentUser != null)
				AppModel.Instance.CurrentUser.FavouriteActions.CollectionChanged += OnFavouriteEventsChanged;
		}

		void OnUserChanged (UserModel previous, UserModel current)
		{
			if (previous != null)
				previous.FavouriteActions.CollectionChanged -= OnFavouriteEventsChanged;

			if (current != null)
				current.FavouriteActions.CollectionChanged += OnFavouriteEventsChanged;

			OnFavouriteEventsChanged (null);
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			Model = BindingContext as EventData;

			if (Model != null) {
				_favEventBoxView.BackgroundColor = Model.BackgroundColor;
				_timeLabel.Text = Model.TimeDuration;
				_timeBoxView.BackgroundColor = Model.BackgroundColorNonOpacity;
				_nameLocationLayout.BackgroundColor = Model.BackgroundColorNonOpacity;
				_locationLabel.Text = Model.EventPlace;
				_nameLabel.Text = Model.Title;
				_eventImage.Source = Model.BackgroundImageSource;
				_coloredBoxView.BackgroundColor = Model.BackgroundColorNonOpacity;
				_favImage.Source = FavEventImage (AppModel.Instance.GetIsEventFavourite (BindingContext as EventData));
			}
		}

		void OnFavouriteEventsChanged (List<FavouriteEventData> events)
		{
			Device.BeginInvokeOnMainThread (() => {
				if(Model != null)
					_favImage.Source = FavEventImage(AppModel.Instance.GetIsEventFavourite(Model));
			});
		}

		void TapFavEvent_Tapped (object sender, System.EventArgs e)
		{
			EventData data = BindingContext as EventData;

			if (data != null) {
				UserController.Instance.AddEventToFavourites (data);
			}
		}

		public ImageSource FavEventImage(bool isFav)
		{ 
			string path = isFav ? AppResources.EventFavouriteImage : AppResources.EventNoFavouriteImage;
			return ImageLoader.Instance.GetImage(path, true);
		}
	}
}