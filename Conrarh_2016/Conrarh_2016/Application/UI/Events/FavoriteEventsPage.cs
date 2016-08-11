using Acr.UserDialogs;
using Conarh_2016.Application.BackgroundTasks;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Application.UI.Shared;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Events
{
    public sealed class FavoriteEventListWrapper : PullRefreshListWrapper
    {
        public override void OnAction()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            AppController.Instance.DownloadEventsData(false, Done);
        }
    }

    internal class FavoriteEventsPage : ContentPage
    {
        private ListView _favEventListView;
        private FavoriteEventListWrapper _wrapper;
        private EventsDataWrapper FavEventListWrapper;

        public FavoriteEventsPage() : base()
        {
            Title = AppResources.FavoriteEvents.ToUpper();
            BackgroundColor = AppResources.AgendaPageBackgroundColor;
            GetItemSource();


            _wrapper = new FavoriteEventListWrapper();
            _favEventListView = new ListView
            {
                HasUnevenRows = true,
                IsGroupingEnabled = true,
                GroupHeaderTemplate = new DataTemplate(typeof(EventGroupItem)),
                BackgroundColor = Color.Transparent,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(EventCell)),
                RefreshCommand = _wrapper.RefreshCommand,
                IsPullToRefreshEnabled = false,
                ItemsSource = FavEventListWrapper,
                //ItemsSource = FavEventList,
                BindingContext = _wrapper
            };
            

            _favEventListView.SetBinding<EventListWrapper>(ListView.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);
            _favEventListView.ItemSelected += OnItemSelected;
            
            var pageLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical
            };

            var infoBox = new Label
            {
                HeightRequest = 40,
                TextColor = Color.White,
                Text = "Monte sua programação selecionando eventos favoritos.",
                FontSize = 12,
                BackgroundColor = AppResources.MenuColor
            };


            if (FavEventListWrapper.Count < 1)
            {
                pageLayout.Children.Add(infoBox);
            }
            
            
            pageLayout.Children.Add(_favEventListView);
            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, pageLayout, true, true);
            Content = bgLayout;
            /*
            if (AppModel.Instance.CurrentUser != null)
                UserController.Instance.DownloadFavouriteEvents();
                *///*//
        }

        
        private void GetItemSource()
        {
            UserDialogs.Instance.ShowLoading();
            AppModel.Instance.CurrentUser.FavoriteEvents.ClearData();
            DynamicListData<EventData> allEvents = new DynamicListData<EventData>();

            if (!(AppModel.Instance.PayedEvents.Items.Count >0) || !(AppModel.Instance.FreeEvents.Items.Count > 0))
            {
                var task1 = new DownloadEventsDataBackgroundTask(true, AppModel.Instance.FreeEvents);
                var task2 = new DownloadEventsDataBackgroundTask(false, AppModel.Instance.PayedEvents);
                task1.Execute();
                task2.Execute();
            }

            
            allEvents.UpdateData(AppModel.Instance.PayedEvents.Items);
            allEvents.UpdateData(AppModel.Instance.FreeEvents.Items);

            foreach (FavouriteEventData fed in AppModel.Instance.CurrentUser.FavouriteActions.Items)
            {
                EventData ed = null;

                ed = allEvents.Find(fed.EventId);
                if (ed != null) { }
                AppModel.Instance.CurrentUser.FavoriteEvents.AddOne(ed);
            }
            UserDialogs.Instance.HideLoading();
            FavEventListWrapper = new EventsDataWrapper(AppModel.Instance.CurrentUser.FavoriteEvents);
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _favEventListView.BeginRefresh();
        }


        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            _favEventListView.SelectedItem = null;
            EventData eventData = e.SelectedItem as EventData;

            if (eventData != null)
            {
                var navPage = new EventDetailView(eventData);
                Navigation.PushAsync(navPage);
            }
        }
    }
}