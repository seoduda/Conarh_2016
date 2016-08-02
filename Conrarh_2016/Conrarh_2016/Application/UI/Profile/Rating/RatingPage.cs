using Xamarin.Forms;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Application.UI.Shared;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class RatingListWrapper: PullRefreshListWrapper
	{
		public readonly RatingListModel Model;
		public RatingListWrapper(RatingListModel model)
		{
			Model = model;
		}
		
		public override void OnAction ()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			AppController.Instance.UpdateRating (Model.Users, Done);
		}
	}

	public sealed class RatingPage : ContentPage
	{
		private readonly ListView _listView;
		private readonly RatingListModel Model;

		public RatingPage ()
		{
			Model = new RatingListModel ();
			Model.ItemsChanged += OnItemsChanged;

			BackgroundColor = Color.Transparent;

			Title = AppResources.ProfileRatingBtnHeader;

			var wrapper = new RatingListWrapper (Model);

			_listView = new ListView {
				HasUnevenRows = false,
				RowHeight = 60,
				ItemTemplate = new DataTemplate (typeof(RatingUserCell)),
				SeparatorVisibility = SeparatorVisibility.None,
				BackgroundColor = Color.Transparent,
				ItemsSource = Model.Items,

				RefreshCommand = wrapper.RefreshCommand,
				IsPullToRefreshEnabled = true,
				BindingContext = wrapper
			};
			_listView.SetBinding<RatingListWrapper> (ListView.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);

            BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, new ContentView { Content = _listView }, true, true);
            //, Padding = new Thickness(10)
            // BGLayoutView bgLayout = new BGLayoutView(AppResources.DefaultBgImage, layout, false, true);

            //Content = layout;
            Content = bgLayout;


            Content = new ContentView { Content = bgLayout};
		}

		void OnItemsChanged ()
		{
			Device.BeginInvokeOnMainThread (() => {
				_listView.ItemsSource = null;
				_listView.ItemsSource =  Model.Items;
			});
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			_listView.BeginRefresh ();
		}
	}

}