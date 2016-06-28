using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Core;
using System;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class ProfilePage : ShareContentPage
	{
		public UserModel Model;

		public ProfilePage ():this(AppModel.Instance.CurrentUser)
		{
		}

		public ProfilePage (UserModel userModel):base()
		{
			Model = userModel;
			Title = AppResources.Profile;

			ProfileContentView.Parameters parameters = new ProfileContentView.Parameters ();
			parameters.UserModel = Model;
			parameters.IsBtnEnabled = true;
			parameters.OnRating = OnRatingClicked;
			parameters.OnContactList = OnContactListClicked;
			parameters.OnSaveChanges = OnEditUserProfile;

			Content = new ProfileContentView(parameters);
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

		void OnRatingClicked (object sender, EventArgs e)
		{
         	Navigation.PushAsync (new RatingPage ());
         
		}

        void OnContactListClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ContactListPage());
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