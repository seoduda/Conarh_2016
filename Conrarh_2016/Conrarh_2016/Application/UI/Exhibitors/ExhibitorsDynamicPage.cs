using Xamarin.Forms;
using Conarh_2016.Application;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Core;
using Conrarh_2016.Application.UI.Shared;
using Conrarh_2016.Core.DataAccess.Local;
using System.Threading;

namespace Conarh_2016.Application.UI.Exhibitors
{
	public sealed class ExhibitorsListWrapper: PullRefreshListWrapper
	{
		private bool IsSearching = false;
		private ExhibitorsDataWrapper SearchDataWrapper;
		private string Pattern = string.Empty;

		public void Search(ExhibitorsDataWrapper dataWrapper, string pattern)
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

		public override void OnAction ()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			if (IsSearching)
				AppController.Instance.SearchExhibitors (Pattern, SearchDataWrapper.Exhibitors, Done);
			else
				AppController.Instance.DownloadExhibitorsData (Done);
		}

	}

	public sealed class ExhibitorsDynamicPage : ShareContentPage
	{
		private ListView _groupListView;
		private string _previousPattern = string.Empty;

		private ExhibitorsDataWrapper ExhibitorsWrapper;
		private ExhibitorsListWrapper _listWrapper;

		public ExhibitorsDynamicPage ():base()
		{
			Title = AppResources.Exhibitors;
			BackgroundColor = Color.White;
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			if (AppProvider.ImageCache != null)
				AppProvider.ImageCache.Clear ();

			if (AppProvider.FastCellCache != null)
				AppProvider.FastCellCache.FlushAllCaches ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

            /*
			var searchBarView = new SearchBarView ();
			searchBarView.Clear += OnSearchClear;
			searchBarView.Search += OnSearch;
            */

			_listWrapper = new ExhibitorsListWrapper ();

            ExhibitorsWrapper =  AppModel.Instance.ExhibitorsWrapper;
            /*
            if (ExhibitorsWrapper == null)
            {
                AppModel.Instance.SponsorTypes.UpdateData(LocalData.getLocalSponsorList());
                AppModel.Instance.Exhibitors.UpdateData(LocalData.getLocalExhibitorList());
                ExhibitorsWrapper = new ExhibitorsDataWrapper(AppModel.Instance.SponsorTypes, AppModel.Instance.Exhibitors, false);
            }*/
            _groupListView = new ListView {
				HasUnevenRows = false,
				RowHeight = 60,
				IsGroupingEnabled = true,
				GroupHeaderTemplate = new DataTemplate (typeof(ExhibitorGroupItem)),
				SeparatorVisibility = SeparatorVisibility.None,
				ItemTemplate = new DataTemplate (typeof (ExhibitorViewCell)),

				RefreshCommand = _listWrapper.RefreshCommand,
				IsPullToRefreshEnabled = true,
				ItemsSource = ExhibitorsWrapper,
				BindingContext = _listWrapper
			};
			_groupListView.SetBinding<ExhibitorsListWrapper> (ListView.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);

            var layout = new StackLayout
            {
                Padding = new Thickness(20),
                Children = { _groupListView }
                //Children = {searchBarView, _groupListView}
            };

            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, false, true);

            Content =  bgLayout ;

			_groupListView.BeginRefresh ();

            

            _groupListView.BeginRefresh();
        }

		void OnSearch (string pattern)
		{
			if (_previousPattern.Equals (pattern))
				return;

			_previousPattern = pattern;
			ExhibitorsWrapper = new ExhibitorsDataWrapper (AppModel.Instance.SponsorTypes, new DynamicListData<Exhibitor> (), true);
			_groupListView.ItemsSource = ExhibitorsWrapper;

			_listWrapper.Search (ExhibitorsWrapper, pattern);
		}

		void OnSearchClear ()
		{
			ExhibitorsWrapper =  AppModel.Instance.ExhibitorsWrapper;
			_groupListView.ItemsSource = ExhibitorsWrapper;

			_listWrapper.ClearSearch ();
		}
	}
}