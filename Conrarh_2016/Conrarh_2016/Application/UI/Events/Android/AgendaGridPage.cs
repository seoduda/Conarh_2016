using Xamarin.Forms;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Wrappers;
using System;
using Conarh_2016.Core;
using XLabs.Forms.Controls;
using TwinTechs.Controls;
using Conarh_2016.Application.UI.Shared;

namespace Conarh_2016.Application.UI.Events
{
    public class AgendaExpoGridPage : AgendaGridPage
    {
        public AgendaExpoGridPage() : base()
        {
            base.SelectExpo();
        }
    }

    public class AgendaGridPage : ShareContentPage
	{
		public bool IsFreeEventsOpened = false; 
		private readonly GridView _eventGridView;

		public AgendaGridPage ():base()
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
				
			int width = AppProvider.Screen.ConvertPixelsToDp (AppProvider.Screen.Width) - 8;
			_eventGridView = new GridView {
				RowSpacing = 5,
				ColumnSpacing = 5,
				ContentPaddingBottom = 0,
				ContentPaddingTop = 0,
				ContentPaddingLeft = 0,
				ContentPaddingRight = 0,
				ItemWidth = width,
				ItemHeight = 200,
				ItemsSource = new EventsGridWrapper(AppModel.Instance.PayedEvents),
				ItemTemplate = new DataTemplate (typeof(EventFastCell))
			};
					
			var container = new PageViewContainer {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new ContentPage {Content = _eventGridView}
			};

			Content = new StackLayout () {
				Children = { 
					buttonLayout,
					container
				}};
            /* pq isso aqui?*/
			AppController.Instance.DownloadExhibitorsData (null);
			_eventGridView.ItemSelected += OnItemSelected;;

		}

		void OnItemSelected (object sender, XLabs.GridEventArgs<object> e)
		{
			if (e.Value == null) return;

			EventData eventData = e.Value as EventData;

			if (eventData != null) {
				var navPage = new EventDetailView (eventData);
				Navigation.PushAsync (navPage);
			}
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			AppController.Instance.DownloadEventsData (IsFreeEventsOpened, null);
			AppController.Instance.DownloadEventsData (!IsFreeEventsOpened, null);

			if (AppModel.Instance.CurrentUser != null)
				UserController.Instance.DownloadFavouriteEvents ();
		}

		private EventsGridWrapper GetItemSource()
		{
			return IsFreeEventsOpened ? AppModel.Instance.FreeEventsGridWrapper :
				AppModel.Instance.PayedEventsGridWrapper;
		}

		private Button GetButton(string title, Color color, EventHandler handler )
		{
            var btn = new Button() {
                BorderRadius = 0,
                Text = title.ToUpper(),
                BackgroundColor = color,
                TextColor = (color.Equals(AppResources.AgendaCongressoColor) ? AppResources.AgendaExpoColor : AppResources.AgendaCongressoColor),
				FontSize = 15,
			};
			btn.Clicked += handler;

			return btn;
		}
			
		void OnCongressoClicked (object sender, System.EventArgs e)
		{
			if (IsFreeEventsOpened) 
			{
				IsFreeEventsOpened = false;
				_eventGridView.ItemsSource = GetItemSource ();
			}
		}

		void OnExpoClicked (object sender, System.EventArgs e)
		{
			if (!IsFreeEventsOpened) 
			{
				IsFreeEventsOpened = true;
				_eventGridView.ItemsSource = GetItemSource ();
			}
		}

        public void SelectExpo()
        {
            if (!IsFreeEventsOpened)
            {
                IsFreeEventsOpened = true;
                _eventGridView.ItemsSource = GetItemSource();
            }

        }
    }

}