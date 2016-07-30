using Conarh_2016.Application.Domain;
using Conarh_2016.Core;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Events
{
    public sealed class EventView : ContentView
    {
        public Image _favImage;
        public EventData Model;

        //private string _TimeText;
        //private BoxView _coloredBoxView;
        private Image _eventImage;
        private Label _nameLabel;
        private Label _locationLabel;
        private Label _SponsorLabel;
        private StackLayout _nameLocationLayout;

        private Label _timeLabel;

        private Button _favEventBoxView;

        public EventView()
        {
            BackgroundColor = Color.Transparent;

            _eventImage = new Image
            {
                Aspect = Aspect.AspectFill,
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width),
                HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 3)
            };

            var stackLayout = new StackLayout { Spacing = 0, Orientation = StackOrientation.Horizontal };
            stackLayout.Children.Add(_eventImage);

            _nameLabel = new Label
            {
                FontAttributes = FontAttributes.None,
                FontSize = 16,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start
            };

            _locationLabel = new Label()
            {
                FontSize = 11,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start
            };

            _SponsorLabel = new Label()
            {
                FontSize = 11,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start
            };


            _nameLocationLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = {
                    _nameLabel,
                    _locationLabel,
                    _SponsorLabel
                },
                Padding = new Thickness(10, 4, 0, 10),
                Spacing = 5,
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width)
            };

            var timeLayout = new AbsoluteLayout()
            {
                Padding = new Thickness(8, 120, 0, 0),
            };

           // double hDiv6 = (AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 6));

            var timeClockImage = new Image()
            {
                WidthRequest = 40,
                HeightRequest = 40,
                Source = ImageLoader.Instance.GetImage(AppResources.EventClockImage, true)
            };

            //timeLayout.Children.Add (timeClockImage, new Point(2, hDiv6 +2));
            timeLayout.Children.Add(timeClockImage, new Point(8, 5));


            _timeLabel = new Label()
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 16,
                TextColor = Color.White,
                //WidthRequest = 42,
                HorizontalTextAlignment = TextAlignment.Start
            };
            timeLayout.Children.Add(_timeLabel, new Point(50, 28));
            stackLayout.Children.Add(timeLayout);
            

            _favEventBoxView = new Button()
            {
                WidthRequest = 35,
                HeightRequest = 35,
                //BorderRadius = 17,
                IsEnabled = false
            };

            _favImage = new Image()
            {
                WidthRequest = 30,
                HeightRequest = 30
            };

            TapGestureRecognizer tapFavEvent = new TapGestureRecognizer();
            tapFavEvent.Tapped += TapFavEvent_Tapped;
            _favImage.GestureRecognizers.Add(tapFavEvent);

            
            var favLayout = new AbsoluteLayout
            {
                Children = { _favEventBoxView },
                Padding = new Thickness(AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - 50, 120, 0, 0)
            };
            favLayout.Children.Add(_favImage, new Point(2.5f, 2.5f));
            
            

                
            var absoluteLayout = new AbsoluteLayout { Padding = new Thickness(0, 5) };
            absoluteLayout.Children.Add(stackLayout);

            absoluteLayout.Children.Add(_nameLocationLayout);
            absoluteLayout.Children.Add(timeLayout, new Point(20, 0));
            absoluteLayout.Children.Add(favLayout);

            Content = absoluteLayout;

            AppModel.Instance.UserChanged += OnUserChanged;
            if (AppModel.Instance.CurrentUser != null)
                AppModel.Instance.CurrentUser.FavouriteActions.CollectionChanged += OnFavouriteEventsChanged;
        }

        private void OnUserChanged(UserModel previous, UserModel current)
        {
            if (previous != null)
                previous.FavouriteActions.CollectionChanged -= OnFavouriteEventsChanged;

            if (current != null)
                current.FavouriteActions.CollectionChanged += OnFavouriteEventsChanged;

            OnFavouriteEventsChanged(null);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            Model = BindingContext as EventData;

            if (Model != null)
            {
                _favEventBoxView.BackgroundColor = Model.BackgroundColor;
                _timeLabel.Text = Model.TimeDuration.Replace("-", "\n");
                _SponsorLabel.Text = string.IsNullOrEmpty(Model.PointsImagePath.Trim()) ? "": string.Format("Patrocínio {0}", Model.PointsImagePath.Trim());

                //_timeBoxView.BackgroundColor = Model.BackgroundColorNonOpacity;
                _nameLocationLayout.BackgroundColor = Model.BackgroundColorNonOpacity;
                _nameLocationLayout.Opacity = 0.95;
                _nameLocationLayout.HeightRequest = (AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 6)-10);
                _locationLabel.Text = Model.EventPlace;
                _nameLabel.Text = Model.Title;
                _eventImage.Source = Model.BackgroundImageSource;
                //_coloredBoxView.BackgroundColor = Model.BackgroundColorNonOpacity;

                _favImage.Source = FavEventImage(AppModel.Instance.GetIsEventFavourite(BindingContext as EventData));
            }
        }

        private void OnFavouriteEventsChanged(List<FavouriteEventData> events)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Model != null)
                    _favImage.Source = FavEventImage(AppModel.Instance.GetIsEventFavourite(Model));
            });
        }

        private void TapFavEvent_Tapped(object sender, System.EventArgs e)
        {
            EventData data = BindingContext as EventData;

            if (data != null)
            {
                UserController.Instance.AddEventToFavourites(data);
            }
        }

        public ImageSource FavEventImage(bool isFav)
        {
            string path = isFav ? AppResources.EventFavouriteImage : AppResources.EventNoFavouriteImage;
            return ImageLoader.Instance.GetImage(path, true);
        }
    }
}