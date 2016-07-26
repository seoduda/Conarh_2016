using Xamarin.Forms;
using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Core;
using Conrarh_2016.Application.UI.Shared;

namespace Conarh_2016.Application.UI.Connect
{
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
		public ConnectionsDataWrapper CurrentModel { private set; get; }
		public readonly ListView UserListView;

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

            UserController.Instance.UpdateProfileData(CurrentModel.LoginedUser);

            var searchBarView = new SearchBarView ();
			searchBarView.Clear += OnSearchClear;
			searchBarView.Search += OnSearch;

			_wrapper = new ConnectListWrapper (CurrentModel);
			UserListView = new ListView {
				RefreshCommand = _wrapper.RefreshCommand,
				HasUnevenRows = true,
				ItemTemplate = new DataTemplate (typeof(ConnectionCell)),
				SeparatorVisibility = SeparatorVisibility.None,
				IsPullToRefreshEnabled = true,
				ItemsSource = CurrentModel,
				BindingContext = _wrapper
			};
			UserListView.SetBinding<ConnectListWrapper> (ListView.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);

            var layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = {
                    new UserHeaderView (CurrentModel.LoginedUser, false, false),
                    searchBarView,
                    UserListView
                }
            };

            //BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, true, true);
            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, false, true);

            Content = bgLayout;

        }

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			UserListView.BeginRefresh ();
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