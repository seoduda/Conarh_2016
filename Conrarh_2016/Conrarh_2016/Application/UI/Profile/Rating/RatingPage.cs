using Xamarin.Forms;
using Conarh_2016.Application.Wrappers;
using System.Collections.Generic;

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

			BackgroundColor = Color.White;

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

			Content = new ContentView { Content = _listView, Padding = new Thickness(10)};
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