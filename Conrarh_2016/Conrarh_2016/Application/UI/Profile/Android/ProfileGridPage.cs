using System;
using Xamarin.Forms;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Core;
using Conarh_2016.Application.UI.Shared;
using XLabs.Forms.Controls;
using TwinTechs.Controls;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class ProfileGridPage : ShareContentPage
	{
		public UserModel Model;

		public ProfileGridPage ():this(AppModel.Instance.CurrentUser)
		{
		}

		private ContentView GetButton(Color btnColor, string name, EventHandler onClicked, Thickness padding)
		{
			var btn = new Button { Text = name, FontAttributes = FontAttributes.Bold,
				FontSize = 17, BackgroundColor = btnColor, TextColor = Color.White, BorderRadius = 0};

			if(onClicked != null)
				btn.Clicked += onClicked;

			return new ContentView {Content = btn, Padding = padding,  WidthRequest = AppProvider.Screen.Width / 2};
		}

		public ProfileGridPage (UserModel userModel):base()
		{
			Model = userModel;
			Title = AppResources.Profile;

			var userHeaderView = new UserHeaderView (Model, true);
			userHeaderView.EditUserProfile += OnEditUserProfile;

			var btnLayout = new StackLayout { 
				Padding = new Thickness (0),
				Orientation = StackOrientation.Horizontal,
				Children = {
					GetButton (AppResources.ProfileRatingBtnColor, AppResources.ProfileRatingBtnHeader, 
						OnRatingClicked, new Thickness(20, 0, 10, 0)),
					GetButton (AppResources.ProfileContactListBtnColor, AppResources.ProfileContactListBtnHeader,
						OnContactListClicked, new Thickness(10, 0, 20, 0))
				}
			};


			var historyButton = new Button {
				FontSize = 20,
				BackgroundColor = AppResources.ProfileHistoryItemLightColor,
				TextColor = AppResources.AgendaCongressoColor,
				FontAttributes = FontAttributes.Bold,
				HeightRequest = 60,
				Text = AppResources.ProfileHistoryClickHeader,
				WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width)
			};	
			historyButton.Clicked += OnHistoryClicked;

			Content = new ScrollView {
				BackgroundColor = AppResources.UserBackgroundColor,
				Content = new StackLayout {
					Orientation = StackOrientation.Vertical,
					VerticalOptions = LayoutOptions.StartAndExpand,
					Children = {
						userHeaderView,
						btnLayout,
						new BadgeGridView (Model),
						historyButton
					}
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

		void OnHistoryClicked (object sender, EventArgs e)
		{
			Navigation.PushAsync (new ProfileHistoryPage (Model));
		}

		void OnRatingClicked (object sender, EventArgs e)
		{
			Navigation.PushAsync (new RatingGridPage ());
		}

		void OnContactListClicked (object sender, EventArgs e)
		{
			Navigation.PushAsync (new ContactListPage ());
		}

		private void OnEditUserProfile ()
		{
			if (!Model.IsEditable())
				return;

			EditProfilePage page = new EditProfilePage (Model.User);
			page.SaveProfileChanges += OnSaveProfileChanges;
			Navigation.PushAsync (page);
		}

		void OnSaveProfileChanges (CreateUserData data)
		{
			UserController.Instance.SaveProfileChanges (data);
		}
	}

}