using Conarh_2016.Application.Domain;
using Conarh_2016.Core;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace Conarh_2016.Application.UI.Events
{
    public sealed class EventFastCell : FastGridCell
    {
        public Image _favImage;
        public EventData Model;

        private BoxView _coloredBoxView;
        private Image _eventImage;
        private Label _nameLabel;
        private Label _dateLabel;
        private Label _locationLabel;
        private StackLayout _nameLocationLayout;
        private BoxView _timeBoxView;
        private Label _timeLabel;
        private Button _favEventBoxView;

        private void OnUserChanged(UserModel previous, UserModel current)
        {
            if (previous != null)
                previous.FavouriteActions.CollectionChanged -= OnFavouriteEventsChanged;

            if (current != null)
                current.FavouriteActions.CollectionChanged += OnFavouriteEventsChanged;

            OnFavouriteEventsChanged(null);
        }

        private void OnFavouriteEventsChanged(List<FavouriteEventData> events)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _favImage.Source = FavEventImage(AppModel.Instance.GetIsEventFavourite(BindingContext as EventData));
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

        public EventFastCell()
        {
        }

        #region implemented abstract members of FastCell

        protected override void InitializeCell()
        {
            _coloredBoxView = new BoxView
            {
                WidthRequest = 10,
                Opacity = 0.5
            };

            _eventImage = new Image
            {
                Aspect = Aspect.AspectFill,
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width),
                HeightRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 3)
            };

            var stackLayout = new StackLayout { Spacing = 0, Orientation = StackOrientation.Horizontal };
            stackLayout.Children.Add(_coloredBoxView);
            stackLayout.Children.Add(_eventImage);

            _dateLabel = new Label
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                FontAttributes = FontAttributes.Bold,
                FontSize = 13,
                TextColor = Color.Lime,
                HorizontalTextAlignment = TextAlignment.End,
                HeightRequest = 20,
                BackgroundColor = Color.Yellow,
                WidthRequest = 75
            };

            _nameLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.None,
                FontSize = 14,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                HeightRequest = 50
            };

            _locationLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End,
                FontSize = 14,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                HeightRequest = 20
            };

            _nameLocationLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = {
                    _dateLabel,
                    _nameLabel,
                    _locationLabel
                },
                Padding = new Thickness(8, 2, 0, 8),
                Spacing = 4,
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width)
            };

            var timeLayout = new StackLayout
            {
                Padding = new Thickness(10, 100, 0, 0),
                Spacing = 0,
                Orientation = StackOrientation.Horizontal
            };
            _timeBoxView = new BoxView
            {
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 5),
                HeightRequest = 80,
                Opacity = 0.6,
                BackgroundColor = AppResources.AgendaCongressoColor
            };

            /*
            var timeLayout = new AbsoluteLayout() { Padding = new Thickness(10, 100, 0, 0) };
            _timeBoxView = new BoxView
            {
                WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Height / 5),
                HeightRequest = 80,
                Opacity = 0.7,
                BackgroundColor = AppResources.AgendaCongressoColor
            };

            _timeBoxView = new Button () {
                WidthRequest = 110,
                HeightRequest = 25,
                BorderRadius = 12,
                IsEnabled = false
            };
            */
            timeLayout.Children.Add(_timeBoxView);

            var timeClockImage = new Image()
            {
                WidthRequest = 40,
                HeightRequest = 40,
                Source = ImageLoader.Instance.GetImage(AppResources.EventClockImage, true)
            };
            //timeLayout.Children.Add(timeClockImage, new Point(2, 2));
            timeLayout.Children.Add(timeClockImage);

            _timeLabel = new Label()
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 12,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                WidthRequest = 800,
                HeightRequest = 40
            };
            //timeLayout.Children.Add(_timeLabel, new Point(45, 10));
            timeLayout.Children.Add(_timeLabel);

            _favEventBoxView = new Button()
            {
                WidthRequest = 35,
                HeightRequest = 35,
                BorderRadius = 17,
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

            View = absoluteLayout;

            AppModel.Instance.UserChanged += OnUserChanged;
            if (AppModel.Instance.CurrentUser != null)
                AppModel.Instance.CurrentUser.FavouriteActions.CollectionChanged += OnFavouriteEventsChanged;
        }

        protected override void SetupCell(bool isRecycled)
        {
            Model = BindingContext as EventData;

            if (Model != null)
            {
                _favEventBoxView.BackgroundColor = Model.BackgroundColor;
                _timeLabel.Text = Model.TimeDuration;
                _dateLabel.Text = String.Format("{0:dd/MM/yyyy}", Model.Date);
                _timeBoxView.BackgroundColor = Model.BackgroundColorNonOpacity;
                _nameLocationLayout.BackgroundColor = Model.BackgroundColor;
                _locationLabel.Text = Model.EventPlace;
                _nameLabel.Text = Model.Title;
                _eventImage.Source = Model.BackgroundImageSource;
                _coloredBoxView.BackgroundColor = Model.BackgroundColorNonOpacity;
                _favImage.Source = FavEventImage(AppModel.Instance.GetIsEventFavourite(BindingContext as EventData));
            }
        }

        #endregion implemented abstract members of FastCell
    }
}