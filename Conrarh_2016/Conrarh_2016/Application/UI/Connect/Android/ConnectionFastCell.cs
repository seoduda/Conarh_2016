using System;
using Conarh_2016.Application.UI.Controls;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Core;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using Conarh_2016.Application.UI.Profile;

namespace Conarh_2016.Application.UI.Connect
{
	public sealed class ConnectionFastCell: FastGridCell
	{
		#region implemented abstract members of FastCell

		protected override void InitializeCell ()
		{
			_userImage = new DownloadedImage(AppResources.DefaultUserImage)  {  WidthRequest = ImageWidth };
			
			int textWidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width)
				- ButtonWidth - 20 - ImageWidth - 10;

			TapGestureRecognizer gesture = new TapGestureRecognizer ();
			gesture.Tapped += TapOnUser;

			_userNameLabel = new Label {
				TextColor = AppResources.AgendaCongressoColor, 
				FontSize = 14, 
				XAlign = TextAlignment.Start, 
				WidthRequest = textWidthRequest,
				HeightRequest = 20,
				GestureRecognizers = {gesture}
			};

			_userEmailLabel = new Label { 
				TextColor = AppResources.AgendaCongressoColor, 
				FontSize = 11, 
				XAlign = TextAlignment.Start, 
				WidthRequest = textWidthRequest,
				HeightRequest = 20
			};
					
			_connectBtn = new Button () {
				WidthRequest = ButtonWidth,
				TextColor = AppResources.AgendaCongressoColor,
				BackgroundColor = AppResources.ConnectRequestNotSentColor,
				BorderRadius = 0,
				FontSize = 13,
				Text = AppResources.ConnectRequestNotSent,
				FontAttributes = FontAttributes.Bold,
				HeightRequest = 30
			};
			_connectBtn.Clicked += OnConnectBtnClicked;



			View = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.Center,
				Children = {
					_userImage,
					new StackLayout { 
						Orientation = StackOrientation.Vertical,
						Children = {_userNameLabel,_userEmailLabel},
						GestureRecognizers = {gesture}
					},
					_connectBtn
				}
			};
		}

		#endregion

		private const int ImageWidth = 40;
		private const int ButtonWidth = 95;

		private ConnectionModel Model;

		private Button _connectBtn;

		private DownloadedImage _userImage;
		private Label _userNameLabel;
		private Label _userEmailLabel;

		protected override void SetupCell (bool isRecycled)
		{
			if(Model != null)
				Model.IsChanged -= OnModelChanged;

			Model = null;

			Model = BindingContext as ConnectionModel;

			if (Model != null) {
				_userImage.UpdateAtTime = Model.UserImageUpdateAtTime;
				_userImage.ServerImagePath = Model.ProfileImagePath;
				_userNameLabel.Text = Model.Name;
				_userEmailLabel.Text = Model.Job;

				OnModelChanged ();
				Model.IsChanged += OnModelChanged;
			}

		}

		public ConnectionFastCell ()
		{
			
		}

		void TapOnUser (object sender, EventArgs e)
		{
			UserController.Instance.UpdateProfileData (Model.UserModel, false, Show);
		}

		private void Show()
		{
			AppController.Instance.AppRootPage.CurrentPage.Navigation.PushAsync (
				new ViewProfilePage (Model.UserModel));
		}

		void OnConnectBtnClicked (object sender, EventArgs e)
		{
			UserController.Instance.TryToConnect (Model);
		}
			

		void OnModelChanged ()
		{
			Device.BeginInvokeOnMainThread (() => {
				if(Model != null)
				{
					if (Model.State == ConnectState.RequestAccepted) 
					{
						_connectBtn.IsVisible = false;
						return;
					}

					_connectBtn.IsVisible = true;
					_connectBtn.BackgroundColor = Model.BtnStateColor;
					_connectBtn.TextColor = Model.State == ConnectState.RequestNotSent ? AppResources.AgendaCongressoColor : Color.White;
					_connectBtn.Text = Model.BtnStateHeader;
				}
			});
		}
	}
}