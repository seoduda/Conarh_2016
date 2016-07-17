using Xamarin.Forms;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Wrappers;
using System;
using Conarh_2016.Core;
using Conarh_2016.Application.UI.Shared;

namespace Conarh_2016.Application.UI.Events
{
	public sealed class EventListWrapper: PullRefreshListWrapper
	{
		private int progress = 0;

		public override void OnAction ()
		{
			if (IsBusy)
				return;

			IsBusy = true;
			progress = 0;

			
			AppController.Instance.DownloadEventsData (false, doTheOtherOne);
		}

        public void doTheOtherOne()
        {
            progress += 1;
            AppController.Instance.DownloadEventsData(true, Increase);
        }

		public void Increase()
		{
			progress += 1;
			if (progress == 2)
				Done ();
		}
	}

    
     public class AgendaExpoPage : AgendaPage
    {
        public AgendaExpoPage() : base()
        {
            base.SelectExpo();
        }
    }


    public class AgendaPage : ShareContentPage
	{
		public bool IsFreeEventsOpened = false; 
		private readonly ListView _eventListView;
		private EventListWrapper _wrapper;

		public AgendaPage ():base()
		{
			Title = AppResources.Agenda.ToUpper();
			BackgroundColor = AppResources.AgendaPageBackgroundColor;
				
			var buttonLayout = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Spacing = 0,
				HorizontalOptions = LayoutOptions.Fill,
				Children =  { 
					new ContentView {Content = GetButton (AppResources.Congresso, AppResources.AgendaCongressoColor, OnCongressoClicked), WidthRequest = AppProvider.Screen.Width / 2}, 
					new ContentView {Content = GetButton(AppResources.Expo, AppResources.AgendaExpoColor, OnExpoClicked), WidthRequest = AppProvider.Screen.Width / 2}, 
				},
				Padding = new Thickness(0)
			};

			_wrapper = new EventListWrapper ();

			_eventListView = new ListView {
				HasUnevenRows = true,
				IsGroupingEnabled = true,
				GroupHeaderTemplate = new DataTemplate (typeof(EventGroupItem)),
				BackgroundColor = Color.Transparent,
				SeparatorVisibility = SeparatorVisibility.None,
				ItemTemplate = new DataTemplate (typeof (EventCell)),
				RefreshCommand = _wrapper.RefreshCommand,
				IsPullToRefreshEnabled = true,
				ItemsSource = GetItemSource(),
				BindingContext = _wrapper
			};
					
			_eventListView.ItemSelected += OnItemSelected;
			_eventListView.SetBinding<EventListWrapper> (ListView.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);

			var pageLayout = new StackLayout 
			{ 
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Vertical
			};

			pageLayout.Children.Add(buttonLayout);
			pageLayout.Children.Add(_eventListView);

			Content = pageLayout;

			if (AppModel.Instance.CurrentUser != null)
				UserController.Instance.DownloadFavouriteEvents ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			_eventListView.BeginRefresh ();
		}
			
		private EventsDataWrapper GetItemSource()
		{
			return IsFreeEventsOpened ? AppModel.Instance.FreeEventsWrapper :
				AppModel.Instance.PayedEventsWrapper;
		}

		private Button GetButton(string title, Color color, EventHandler handler )
		{
			var btn = new Button () {
				BorderRadius = 0,
				Text = title.ToUpper(),
				BackgroundColor = color,
                TextColor = Color.White,
                FontSize = 15,
			};
			btn.Clicked += handler;

			return btn;
		}

		private void OnItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null) return;

			_eventListView.SelectedItem = null;
			EventData eventData = e.SelectedItem as EventData;

			if (eventData != null) {
				var navPage = new EventDetailView (eventData);
				Navigation.PushAsync (navPage);
			}
		}


		void OnCongressoClicked (object sender, System.EventArgs e)
		{
			if (IsFreeEventsOpened) 
			{
				IsFreeEventsOpened = false;
				_eventListView.ItemsSource = GetItemSource ();
			}
		}

		void OnExpoClicked (object sender, System.EventArgs e)
		{
			if (!IsFreeEventsOpened) 
			{
				IsFreeEventsOpened = true;
				_eventListView.ItemsSource = GetItemSource ();
			}
		}

        public void SelectExpo()
        {
            if (!IsFreeEventsOpened)
            {
                IsFreeEventsOpened = true;
                _eventListView.ItemsSource = GetItemSource();
            }
        }
    }

}