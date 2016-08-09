using Xamarin.Forms;
using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Core;
using System;
using Conarh_2016.Application.UI.Profile;

namespace Conarh_2016.Application.UI.Connect
{

    public enum ContactListType
    {
        All,
        Pending
    }

    public sealed class ConnectListWrapper: PullRefreshListWrapper
	{
		public ConnectionsDataWrapper CurrentModel;
		private int progress = 0;

		private bool IsSearching = false;
		private ConnectionsDataWrapper SearchDataWrapper;
		private string Pattern = string.Empty;
        

        public ConnectListWrapper(ConnectionsDataWrapper currentModel)
		{
			CurrentModel = currentModel;
		}

		public override void OnAction ()
		{
			if (IsBusy)
				return;

			IsBusy = true;
			progress = 0;

			if (IsSearching)
				AppController.Instance.SearchUsers (SearchDataWrapper.SearchUsers, Pattern, Increase);
			else
				AppController.Instance.DownloadAllUsers (Increase);

			UserController.Instance.UpdateProfileData (CurrentModel.LoginedUser, true, Increase);

        }

		public void Increase()
		{
			progress += 1;
			if (progress == 2)
				Done ();
		}


		public void Search(ConnectionsDataWrapper dataWrapper, string pattern)
		{
			IsBusy = false;

			IsSearching = true;
			Pattern = pattern;
			SearchDataWrapper = dataWrapper;
			OnAction ();
		}

		public void ClearSearch()
		{
			IsBusy = false;
			IsSearching = false;

			OnAction ();
		}
	}

	public sealed class ConnectionPage : ContentPage
	{
        private  ContactListType selectedContactedList = ContactListType.All;
        public ConnectionsDataWrapper CurrentModel { private set; get; }
        public ConnectionsDataWrapper PendingConnectionsDataWrapper { private set; get; }
        public readonly ListView UserListView;
        StackLayout layout;
        private StackLayout lowerLayout;
        private SearchBarView searchBarView;


        private readonly ConnectListWrapper _wrapper;

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			if (AppProvider.ImageCache != null)
				AppProvider.ImageCache.Clear ();

			if (AppProvider.FastCellCache != null)
				AppProvider.FastCellCache.FlushAllCaches ();
		}

		public ConnectionPage ()
		{
			Title = AppResources.Connect;
			BackgroundColor = Color.Transparent;

            CurrentModel = AppModel.Instance.CurrentConnectionsWrapper;
            PendingConnectionsDataWrapper = GetPendingConnectionsDataWrapper();
            UserController.Instance.UpdateProfileData(CurrentModel.LoginedUser);

            searchBarView = new SearchBarView ();
			searchBarView.Clear += OnSearchClear;
			searchBarView.Search += OnSearch;

			_wrapper = new ConnectListWrapper (CurrentModel);
			UserListView = new ListView {
				RefreshCommand = _wrapper.RefreshCommand,
				HasUnevenRows = true,
				ItemTemplate = new DataTemplate (typeof(ConnectionCell)),
				SeparatorVisibility = SeparatorVisibility.None,
				IsPullToRefreshEnabled = false,
                ItemsSource = GetItemsSource(),
                //ItemsSource = CurrentModel,
                BindingContext = _wrapper
			};
			UserListView.SetBinding<ConnectListWrapper> (ListView.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);


            var buttonLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                HorizontalOptions = LayoutOptions.Fill,
                Children =  {
                    new ContentView {Content = GetButton(AppResources.ProfileAllContacsBtnHeader, AppResources.MenuColor, OnAllContactsClicked), WidthRequest = AppProvider.Screen.Width / 3},
                    new ContentView {Content = GetButton (AppResources.ProfilePendingContacsBtnHeader, AppResources.AgendaCongressoColor, OnPendingContactsClicked), WidthRequest = AppProvider.Screen.Width / 3},
                    new ContentView {Content = GetButton(AppResources.ProfileRatingBtnHeader, AppResources.AgendaExpoColor, OnRankingClicked), WidthRequest = AppProvider.Screen.Width / 3},
                },
                Padding = new Thickness(0)
            };


            lowerLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };
            GetLowerLayoutChidren(selectedContactedList);

            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = {
                    new UserHeaderView (CurrentModel.LoginedUser, false, false, false),
                    buttonLayout,
                    lowerLayout
                }
            };

            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, true, true);
            // BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, false, true);

            //Content = layout;
            Content = bgLayout;

        }

        private ConnectionsDataWrapper GetPendingConnectionsDataWrapper()
        {
            ConnectionsDataWrapper PendingConnectionsDataWrapper = new ConnectionsDataWrapper();
            
            for (int i=0; i< CurrentModel.Count; i++)
            {
                if (CurrentModel[i].State == ConnectState.RequestedToAccept)
                {
                    PendingConnectionsDataWrapper.Add(CurrentModel[i]);
                }
            }
            return PendingConnectionsDataWrapper;

        }

        private ConnectionsDataWrapper GetItemsSource()
        {
            switch (selectedContactedList)
            {
                case ContactListType.All:
                    {
                        return CurrentModel;
                        
                        break;
                    }
                case ContactListType.Pending:
                    {
                        return PendingConnectionsDataWrapper;
                        break;
                    }
            }
            return null;
        }


        private void GetLowerLayoutChidren(ContactListType listType)
        {
            lowerLayout.Children.Clear();
            if (listType == ContactListType.All)
            {
                lowerLayout.Children.Add(searchBarView);

            }
            lowerLayout.Children.Add(UserListView);
        }


        private void OnRankingClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RatingPage());
        }

        private void OnPendingContactsClicked(object sender, EventArgs e)
        {
            if (selectedContactedList == ContactListType.Pending)
                return;

            selectedContactedList = ContactListType.Pending;
            GetLowerLayoutChidren(selectedContactedList);
            UserListView.ItemsSource = GetItemsSource();

        }

        private void OnAllContactsClicked(object sender, EventArgs e)
        {
            if (selectedContactedList == ContactListType.All)
                return;

            selectedContactedList = ContactListType.All;
            GetLowerLayoutChidren(selectedContactedList);
            UserListView.ItemsSource = GetItemsSource();
        }

        protected override void OnAppearing ()
		{
			base.OnAppearing ();
			UserListView.BeginRefresh ();
		}


        private Button GetButton(string title, Color color, EventHandler handler)
        {
            var btn = new Button()
            {
                BorderRadius = 0,
                Text = title.ToUpper(),
                BackgroundColor = color,
                TextColor = Color.White,
                FontSize = 15,
            };
            btn.Clicked += handler;
            return btn;
        }


        private string previousPattern = string.Empty;
		void OnSearch (string pattern)
		{
			if (previousPattern.Equals (pattern))
				return;

			previousPattern = pattern;
			CurrentModel = new ConnectionsDataWrapper(AppModel.Instance.CurrentUser,
				new DynamicListData<User>(), AppModel.Instance.UsersModelsWrapper);
			
			UserListView.ItemsSource = CurrentModel;

			_wrapper.Search (CurrentModel, previousPattern);
		}

		void OnSearchClear ()
		{
			previousPattern = string.Empty;
			CurrentModel = AppModel.Instance.CurrentConnectionsWrapper;
			UserListView.ItemsSource = CurrentModel;

			_wrapper.ClearSearch ();
		}
	}

}