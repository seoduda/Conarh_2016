using System;
using Conarh_2016.Application.UI.Controls;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Core;
using Xamarin.Forms;
using Conarh_2016.Application.UI.Profile;
using XLabs.Forms.Controls;

namespace Conarh_2016.Application.UI.Connect
{
	public sealed class ConnectionCell: ExtendedViewCell
	{
		private const int ImageWidth = 40;
		private const int ButtonWidth = 95;

		private ConnectionModel Model;

		private readonly Button _connectBtn;

		private DownloadedImage _userImage;
		private Label _userNameLabel;
		private Label _userEmailLabel;

		public ConnectionCell ()
		{
			var userChooseLayout = new AbsoluteLayout {};
			_userImage = new DownloadedImage(AppResources.DefaultUserImage) 
			{  
				WidthRequest = ImageWidth
			};
			userChooseLayout.Children.Add (_userImage);

			var namesLayout = new StackLayout  { Orientation = StackOrientation.Vertical};

			int textWidthRequest = AppProvider.Screen.Width - ButtonWidth - 20 - ImageWidth - 10;

			_userNameLabel = new Label { TextColor = AppResources.MenuColor, FontSize = 14, XAlign = TextAlignment.Start, WidthRequest = textWidthRequest};
			namesLayout.Children.Add (_userNameLabel);

			_userEmailLabel = new Label { TextColor = AppResources.MenuColor, FontSize = 11, XAlign = TextAlignment.Start, WidthRequest = textWidthRequest};
			namesLayout.Children.Add (_userEmailLabel);

			userChooseLayout.Children.Add (namesLayout, new Point(ImageWidth * 1.2f, 0));

            /* TODO validar se precisa do TapOnUser Connection cell
			TapGestureRecognizer tapOnUser = new TapGestureRecognizer ();
			tapOnUser.Tapped += TapOnUser;
			userChooseLayout.GestureRecognizers.Add (tapOnUser);
            */

            _connectBtn = new Button () {
				WidthRequest = ButtonWidth,
				TextColor = Color.White,
				BackgroundColor = AppResources.AgendaExpoColor,
				BorderRadius = 0,
				FontSize = 12,
				Text = AppResources.ConnectRequestNotSent,
				FontAttributes = FontAttributes.Bold
			};
			_connectBtn.Clicked += OnConnectBtnClicked;

			Grid grid = new Grid
			{
				HorizontalOptions = LayoutOptions.EndAndExpand,
				RowDefinitions = 
				{
					new RowDefinition { Height = GridLength.Auto },
				},
				ColumnDefinitions = 
				{
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(ButtonWidth, GridUnitType.Absolute) }
				},
				Padding = new Thickness(10, 0, 10, 10),
				BackgroundColor = Color.Transparent
			};

			grid.Children.Add (userChooseLayout, 0, 0);
			grid.Children.Add (_connectBtn, 1, 0);


            OnModelChanged();

            this.View = grid;
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

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			if(Model != null)
				Model.IsChanged -= OnModelChanged;

			Model = null;

			Model = BindingContext as ConnectionModel;

			if (Model != null) {
				_userImage.UpdateAtTime = Model.UserImageUpdateAtTime;
				_userImage.ServerImagePath = Model.ProfileImagePath;
				_userNameLabel.Text = Model.Name;
				_userEmailLabel.Text = Model.Job;

				Model.IsChanged += OnModelChanged;

				OnModelChanged ();
			}
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
					_connectBtn.TextColor = Model.State == ConnectState.RequestNotSent ? AppResources.MenuColor : Color.White;
					_connectBtn.Text = Model.BtnStateHeader;
				}
			});
		}
	}
}