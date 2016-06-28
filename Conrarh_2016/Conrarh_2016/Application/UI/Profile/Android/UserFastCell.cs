using Conarh_2016.Application.Domain;
using Conarh_2016.Application.UI.Controls;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Application.UI.Profile;
using System;

namespace Conarh_2016.Application.UI.Shared
{
	public sealed class UserFastCell: FastGridCell
	{
		private DownloadedImage _userImage;
		private Label _userNameLabel;
		private Label _userJobLabel;

		#region implemented abstract members of FastGridCell

		protected override void InitializeCell ()
		{
			var layout = new StackLayout  { 
				Orientation = StackOrientation.Horizontal, 
				BackgroundColor = AppResources.WallPostBackgroundColor
			};

			_userImage = new DownloadedImage(AppResources.DefaultUserImage) 
			{
				HeightRequest = 50
			};

			layout.Children.Add (_userImage);

			var topLabelsLayout = new StackLayout { 
				Orientation = StackOrientation.Vertical
			};

			_userNameLabel = new Label { 
				TextColor = AppResources.AgendaCongressoColor, 
				FontSize = 15, 
				XAlign = TextAlignment.Start
			};
			topLabelsLayout.Children.Add (_userNameLabel);

			_userJobLabel = new Label { 
				TextColor = AppResources.AgendaCongressoColor, 
				FontSize = 13, 
				XAlign = TextAlignment.Start
			};
			topLabelsLayout.Children.Add(_userJobLabel);

			layout.Children.Add (topLabelsLayout);

			TapGestureRecognizer gesture = new TapGestureRecognizer ();
			gesture.Tapped += TapOnUser;

			View = new ContentView {
				Content = layout, 
				Padding = new Thickness(0, 0, 0, 10), 
				GestureRecognizers = {gesture} 
			};
		}

		protected override void SetupCell (bool isRecycled)
		{
			if (BindingContext is UserModel) {
				var model = BindingContext as UserModel;
				_userImage.UpdateAtTime = model.UserImageUpdateAtTime;
				_userImage.ServerImagePath = model.ProfileImagePath;
				_userJobLabel.Text = model.Job;
				_userNameLabel.Text = model.Name;
			} else if (BindingContext is ConnectionModel) {
				var model = BindingContext as ConnectionModel;
				_userImage.UpdateAtTime = model.UserImageUpdateAtTime;
				_userImage.ServerImagePath = model.ProfileImagePath;
				_userJobLabel.Text = model.Job;
				_userNameLabel.Text = model.Name;
			}
		}

		#endregion

		void TapOnUser (object sender, EventArgs e)
		{
			var model = BindingContext as UserModel;
			if (model == null)
				model = (BindingContext as ConnectionModel).UserModel;
			
			AppController.Instance.AppRootPage.CurrentPage.Navigation.PushAsync (
				new ViewProfilePage (model));
		}

	}
}