using Xamarin.Forms;
using Conarh_2016.Core;
using Conarh_2016.Application.UI.Controls;
using Conarh_2016.Application.Domain;
using XLabs.Forms.Controls;
using System;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class RatingUserCell: ExtendedViewCell
	{
		private const int ImageWidth = 40;
		private const int ButtonWidth = 70;

		private const int ImageStartXPos = 40;

		private Label _userIndexLabel;
		private DownloadedImage _userImage;
		private Label _userNameLabel;
		private Button _pointsBox;

		public RatingUserCell ()
		{
			_userIndexLabel = new Label { 
				TextColor = AppResources.ProfileContactListBtnColor, 
				FontSize = 16,
				XAlign = TextAlignment.Start, 
				WidthRequest =  30,
				YAlign = TextAlignment.Center
			};

			_userImage = new DownloadedImage(AppResources.DefaultUserImage) 
			{  
				WidthRequest = ImageWidth
			};

			int textWidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - 
				ImageWidth - 40 - ImageStartXPos - ButtonWidth;

			_userNameLabel = new Label { 
				TextColor = AppResources.AgendaCongressoColor, 
				FontSize = 12, 
				XAlign = TextAlignment.Start, 
				WidthRequest = textWidthRequest,
				YAlign = TextAlignment.Center
			};

			_pointsBox = new Button {
				WidthRequest =  ButtonWidth,
				TextColor = Color.Black,
				BackgroundColor = AppResources.ProfileRankingPointsColor,
				BorderRadius = 0,
				FontSize = 12,
				HeightRequest = 15
			};

			TapGestureRecognizer gesture = new TapGestureRecognizer ();
			gesture.Tapped += TapOnUser;

			View = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Children = {
					_userIndexLabel,
					_userImage,
					_userNameLabel,
					_pointsBox
				},
				Padding = new Thickness(0, 2),
				GestureRecognizers = {gesture}
			};
		}

		void TapOnUser (object sender, EventArgs e)
		{
			var model = BindingContext as RatingUserModel;
			AppController.Instance.AppRootPage.CurrentPage.Navigation.PushAsync (
				new ViewProfilePage (model.UserModel));
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			var model = BindingContext as RatingUserModel;
			if (model != null) {
				_userImage.UpdateAtTime = model.UserModel.UpdatedAtTime;
				_userImage.ServerImagePath = model.ProfileImagePath;
				_userIndexLabel.Text = model.IndexStr;
				_userNameLabel.Text = model.Name;
				_pointsBox.Text = model.Points;
			}
		}
	}
}
