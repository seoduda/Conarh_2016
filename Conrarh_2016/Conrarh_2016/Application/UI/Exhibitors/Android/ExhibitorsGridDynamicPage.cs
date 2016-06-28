using Xamarin.Forms;
using Conarh_2016.Application;
using XLabs.Forms.Controls;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Core;
using Conarh_2016.Application.UI.Shared;
using TwinTechs.Controls;
using Conarh_2016.Application.Domain;


namespace Conarh_2016.Application.UI.Exhibitors
{
	public sealed class ExhibitorsGridDynamicPage : ShareContentPage
	{
		private string _previousPattern = string.Empty;
		private GridView _gridView; 

		public ExhibitorsGridDynamicPage ():base()
		{
			Title = AppResources.Exhibitors;
			BackgroundColor = Color.White;
		}

		private ExhibitorsGridWrapper CurrentDataWrapper;
		private ExhibitorsGridWrapper DefaultDataWrapper;

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			var searchBarView = new SearchBarView ();
			searchBarView.Padding = new Thickness (20, 20, 20, 5);
			searchBarView.Clear += OnSearchClear;
			searchBarView.Search += OnSearch;

			int width = AppProvider.Screen.ConvertPixelsToDp (AppProvider.Screen.Width) - 20;

			DefaultDataWrapper = new ExhibitorsGridWrapper (AppModel.Instance.Exhibitors);
			CurrentDataWrapper = DefaultDataWrapper;

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
				ItemTemplate = new DataTemplate (typeof(ExhibitorFastCell))
			};

			var container = new PageViewContainer {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new ContentPage {Content = _gridView}
			};
	
			Content = new StackLayout () {
				Children = { 
					searchBarView,
					container
				}};

			AppController.Instance.DownloadExhibitorsData (null);
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
			CurrentDataWrapper = new ExhibitorsGridWrapper (new DynamicListData<Exhibitor> ());
			_gridView.ItemsSource = CurrentDataWrapper;

			AppController.Instance.SearchExhibitors (_previousPattern, CurrentDataWrapper.Exhibitors, null);
		}

		void OnSearchClear ()
		{
			CurrentDataWrapper = DefaultDataWrapper;
			_gridView.ItemsSource = CurrentDataWrapper;
		}
	}
}