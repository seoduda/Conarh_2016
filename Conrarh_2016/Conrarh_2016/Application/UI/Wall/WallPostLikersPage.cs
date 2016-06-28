using Xamarin.Forms;
using Conarh_2016.Application;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Application.Wrappers;

namespace Conarh_2016.Application.UI.Wall
{
	public sealed class WallPostLikersWrapper: PullRefreshListWrapper
	{
		public readonly WallPost Post;
		public readonly WallPostLikesWrapper DataWrapper;

		public WallPostLikersWrapper(WallPostLikesWrapper dataWrapper, WallPost post)
		{
			Post = post;
			DataWrapper = dataWrapper;
		}

		public override void OnAction ()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			AppController.Instance.DownloadWallPostLikes (DataWrapper.Likes, Post, Done);
		}
	}

	public sealed class WallPostLikersPage : ContentPage
	{
		private readonly WallPostLikesWrapper DataWrapper;
		private readonly ListView _likersListView;

		public WallPostLikersPage (WallPost postData)
		{
			Title = AppResources.WallLikes;
			BackgroundColor = AppResources.WallPageBackgroundColor;

			var likes = new DynamicListData<WallPostLike>();
			likes.UpdateData (AppModel.Instance.GetWallPostLikes (postData));
			DataWrapper = new WallPostLikesWrapper (likes);

			var wrapper = new WallPostLikersWrapper (DataWrapper, postData);

			_likersListView = new ListView {
				HasUnevenRows = false,
				RowHeight = 60,
				ItemTemplate = new DataTemplate (typeof(UserCell)),
				SeparatorVisibility = SeparatorVisibility.None,
				BackgroundColor = Color.Transparent,
				ItemsSource = DataWrapper,

				RefreshCommand = wrapper.RefreshCommand,
				IsPullToRefreshEnabled = true,
				BindingContext = wrapper
			};

			_likersListView.SetBinding<WallPostLikersWrapper> (ListView.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);

			Content = new ContentView { 
				Content = _likersListView, 
				Padding = new Thickness(10, 10, 10, 0)
			}; 
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_likersListView.BeginRefresh ();
		}
	}
}