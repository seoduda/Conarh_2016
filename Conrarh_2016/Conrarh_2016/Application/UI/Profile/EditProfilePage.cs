using System;
using Xamarin.Forms;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.UI.Shared;
using Conarh_2016.Application.Domain.PostData;
using Conarh_2016.Core;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class EditProfilePage : ContentPage
	{
		public event Action<CreateUserData> SaveProfileChanges;
		public readonly User Model;
		public readonly string ImagePath; 

		public EditProfilePage (User currentUser)
		{
			Title = AppResources.EditProfile;
			Model = currentUser;
			ImagePath = null;
			if (!string.IsNullOrEmpty (Model.ProfileImagePath)) {
				ImageData imageData = AppModel.Instance.Images.Items.Find (temp => temp.ServerPath.Equals (Model.ProfileImagePath));
				ImagePath = imageData != null ? imageData.ImagePath : null;
			}
			
			CreateUserData data = new CreateUserData {
				Name = currentUser.Name,
				Email = currentUser.Email,
				Password = null,
				UserType = currentUser.Id,
				Job = currentUser.Job,
				Phone = currentUser.Phone,
				ProfileImage = ImagePath
			};
			FillUserDataView fillData = new FillUserDataView (data, AppResources.SaveProfileButton, false);
			fillData.Apply += OnSaveUserProfile;

			Content = new ScrollView { Content = fillData, Padding = new Thickness(0, 10, 0, 0) };
		}

		private void OnSaveUserProfile (CreateUserData data)
		{
			if (SaveProfileChanges != null) {
				
				data.Name = Model.Name.Equals (data.Name) ? null : data.Name;
				data.Email = Model.Email.Equals (data.Email) ? null : data.Email;
				data.Password = null;
				data.UserType = null;
				data.Job = Model.Job.Equals (data.Job) ? null : data.Job;
				data.ProfileImage = ImagePath != null && ImagePath.Equals (data.ProfileImage) ? null : data.ProfileImage;
				data.Phone = Model.Phone.Equals (data.Phone) ? null : data.Phone;
				data.ScorePoints = null;

				if (data.Name == null &&
				    data.Email == null &&
				    data.Job == null &&
				    data.ProfileImage == null &&
				    data.Phone == null &&
				    data.ScorePoints == null) {
					AppProvider.PopUpFactory.ShowMessage (AppResources.EditProfileNoChanges, string.Empty);
				} else {
					SaveProfileChanges (data);
				}
			}
		}


	}

}