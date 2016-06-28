using Conarh_2016.Application.Domain;
using Conarh_2016.Application.UI.Controls;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using Conarh_2016.Application.Wrappers;
using System;
using Conarh_2016.Application.UI.Profile;

namespace Conarh_2016.Application.UI.Shared
{
	public sealed class UserCell: ExtendedViewCell
	{
		private DownloadedImage _userImage;
		private Label _userNameLabel;
		private Label _userJobLabel;

		public UserCell ()
		{
			var layout = new StackLayout  { 
				Orientation = StackOrientation.Horizontal, 
				BackgroundColor = AppResources.WallPostBackgroundColor
			};

			_userImage = new DownloadedImage(AppResources.DefaultUserImage) 
			{
				HeightRequest = 50,
				WidthRequest = 50,
				Aspect = Aspect.AspectFill
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

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

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