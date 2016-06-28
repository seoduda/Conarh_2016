using Xamarin.Forms;
using Conarh_2016.Application;
using XLabs.Forms.Controls;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Core;
using Conarh_2016.Application.UI.Shared;
using TwinTechs.Controls;
using Conarh_2016.Application.Domain;


namespace Conarh_2016.Application.UI.Connect
{
	public sealed class ConnectionGridPage : ShareContentPage
	{
		private string _previousPattern = string.Empty;
		private GridView _gridView; 

		public ConnectionGridPage ():base()
		{
			Title = AppResources.Connect;
			BackgroundColor = Color.White;
		}

		private ConnectionsDataWrapper CurrentDataWrapper;
		private ConnectionsDataWrapper DefaultDataWrapper;

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			DefaultDataWrapper = AppModel.Instance.CurrentConnectionsWrapper;
			CurrentDataWrapper = DefaultDataWrapper;

			var searchBarView = new SearchBarView ();
			searchBarView.Clear += OnSearchClear;
			searchBarView.Search += OnSearch;

			int width = AppProvider.Screen.ConvertPixelsToDp (AppProvider.Screen.Width) - 20;
			_gridView = new GridView {
				RowSpacing = 5,
				ColumnSpacing = 5,
				ContentPaddingBottom = 0,
				ContentPaddingTop = 0,
				ContentPaddingLeft = 0,
				ContentPaddingRight = 0,
				ItemWidth = width,
				ItemHeight = 60,
				ItemsSource = CurrentDataWrapper,
				ItemTemplate = new DataTemplate (typeof(ConnectionFastCell))
			};

			var container = new PageViewContainer {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new ContentPage {Content = _gridView}
			};

			Content = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Children = {
					new UserHeaderView (CurrentDataWrapper.LoginedUser, false),
					searchBarView,
					container
				}
			};

			AppController.Instance.DownloadAllUsers (null);
			UserController.Instance.UpdateProfileData (AppModel.Instance.CurrentUser, true);
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			if (AppProvider.ImageCache != null)
				AppProvider.ImageCache.Clear ();

			if (AppProvider.FastCellCache != null)
				AppProvider.FastCellCache.FlushAllCaches ();
		}

		void OnSearch (string pattern)
		{
			if (_previousPattern.Equals (pattern))
				return;

			_previousPattern = pattern;

			CurrentDataWrapper = new ConnectionsDataWrapper(AppModel.Instance.CurrentUser,
				new DynamicListData<User>(), AppModel.Instance.UsersModelsWrapper);
			_gridView.ItemsSource = CurrentDataWrapper;

			AppController.Instance.SearchUsers (CurrentDataWrapper.SearchUsers, _previousPattern, null);
		}

		void OnSearchClear ()
		{
			_previousPattern = string.Empty;
			CurrentDataWrapper = DefaultDataWrapper;
			_gridView.ItemsSource = CurrentDataWrapper;
		}
	}
}