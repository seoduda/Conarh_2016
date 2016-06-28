using Xamarin.Forms;
using Conarh_2016.Core;
using Conarh_2016.Application.UI.Shared;
using XLabs.Forms.Controls;
using TwinTechs.Controls;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class ProfileHistoryPage : ShareContentPage
	{
		public UserModel Model;

		private GridView _historyGridView;

		public ProfileHistoryPage (UserModel userModel):base()
		{
			Model = userModel;
			Title = AppResources.Profile;

			var userHeaderView = new UserHeaderView (Model, false);

			int width = AppProvider.Screen.ConvertPixelsToDp (AppProvider.Screen.Width) - 10;
			_historyGridView = new GridView {
				RowSpacing = 5,
				ColumnSpacing = 5,
				ContentPaddingBottom = 0,
				ContentPaddingTop = 0,
				ContentPaddingLeft = 0,
				ContentPaddingRight = 0,
				ItemWidth = width,
				ItemHeight = 60,
				ItemsSource = AppModel.Instance.CurrentContactListWrapper,
				ItemTemplate = new DataTemplate (typeof(ProfileHistoryFastItemCell))
			};

			var container = new PageViewContainer {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new ContentPage {Content = _historyGridView}
			};

			var historylabel = new Label {
				FontSize = 22,
				Text = AppResources.ProfileHistoryHeader,
				TextColor = AppResources.AgendaCongressoColor,
				XAlign = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold,
				YAlign = TextAlignment.Center,
				HeightRequest = 40
			};	

			Content = new StackLayout {
				BackgroundColor = Color.White,
				Children = {
					userHeaderView,
					historylabel,
					container
				}
			};

			UserController.Instance.UpdateProfileData (Model);
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			if (AppProvider.ImageCache != null)
				AppProvider.ImageCache.Clear ();

			if (AppProvider.FastCellCache != null)
				AppProvider.FastCellCache.FlushAllCaches ();
		}
	}

}