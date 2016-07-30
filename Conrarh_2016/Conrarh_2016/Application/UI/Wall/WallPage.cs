using Xamarin.Forms;
using Conarh_2016.Core;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Application.UI.Shared;
using Conrarh_2016.Application.UI.Shared;

namespace Conarh_2016.Application.UI.Wall
{
	public sealed class WallListWrapper: PullRefreshListWrapper
	{
		public override void OnAction ()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			AppController.Instance.DownloadWallData (Done);
		}
	}

	public sealed class WallPage : ShareContentPage
	{
		public const int Border = 20;
		private ListView _wallPostListView;

		public WallPage ()
		{
			Title = AppResources.Wall;
			BackgroundColor = AppResources.WallPageBackgroundColor;

			var wrapper = new WallListWrapper ();

			_wallPostListView = new ListView {
				HasUnevenRows = true,
				ItemTemplate = new DataTemplate (typeof(WallPostCell)),
				SeparatorVisibility = SeparatorVisibility.None,
				BackgroundColor = Color.Transparent,
				RefreshCommand = wrapper.RefreshCommand,
				IsPullToRefreshEnabled = true,
				ItemsSource = AppModel.Instance.WallPostsWrapper,
				BindingContext = wrapper
			};

			_wallPostListView.SetBinding<WallListWrapper> (ListView.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);

			var listContent = new ContentView { 
				Content = _wallPostListView, 
				Padding = new Thickness(Border, Border, Border, 0),
				HeightRequest = AppProvider.Screen.Height
			}; 
					
			Button postButton = new Button {
				BackgroundColor = AppResources.MenuColor,
				WidthRequest = AppProvider.Screen.Width,
				Text = AppResources.PostOnWall,
				TextColor = Color.White,
				BorderRadius = 0
			};
			postButton.Clicked += OnPostClicked;

            var layout =  new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { listContent, postButton }
            };

            BGLayoutView bgLayout = new BGLayoutView(AppResources.InteractBgImage, layout, true, true);
            //Content = new ScrollView {Content = bgLayout };
            Content = new ContentView { Content = bgLayout };


            Content = bgLayout;

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
            for (int i=0; i< AppModel.Instance.WallPostsWrapper.Count; i++)
            {
                AppModel.Instance.WallPostsWrapper[i].CreatedUser = AppModel.Instance.Users.Find(AppModel.Instance.WallPostsWrapper[i].CreatedUserId);
            }
            */
            



            _wallPostListView.BeginRefresh();
		}
			
		private void OnPostClicked (object sender, System.EventArgs e)
		{
			if (AppModel.Instance.CurrentUser == null) 
			{
				AppProvider.PopUpFactory.ShowMessage (AppResources.LoginFirstMessage, AppResources.Warning);
				return;
			}

		    Navigation.PushAsync (new WallCreatePostPage (), false);
		}
	}
}