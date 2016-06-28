using Xamarin.Forms;
using Conarh_2016.Core;
using System;
using Conarh_2016.Application.UI.Controls;
using Conarh_2016.Application.Domain;
using XLabs.Forms.Controls;

namespace Conarh_2016.Application.UI.Wall
{
	public sealed class WallPostCell: ExtendedViewCell
	{
		private const int LeftBorder = 20;
		private const int ItemLeftBorder = 10;

		private DownloadedImage _postDataImage;

		private Label _likeLabel;
		private WallPost Model;

		private DownloadedImage _creatorUserImage;
		private Label _postDateLabel;
		private Label _postInfoLabel;
		private Label _creatorNameLabel;

		public WallPostCell ()
		{
			int imageSize = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - ItemLeftBorder * 2 - LeftBorder * 2;

			_creatorUserImage = new DownloadedImage(AppResources.DefaultUserImage) 
			{
				HeightRequest = 40,
				WidthRequest = 40
			};

			_creatorNameLabel = new Label { 
				TextColor = AppResources.AgendaCongressoColor, 
				FontSize = 15, 
				XAlign = TextAlignment.Start,
				HeightRequest = 20,
				WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - 60
			};

			_postDateLabel = new Label { 
				TextColor = AppResources.WallPostCreatedDateColor, 
				FontSize = 13, 
				XAlign = TextAlignment.Start,
				HeightRequest = 20,
				WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - 60
			};

			_postInfoLabel = new Label { 
				TextColor = AppResources.AgendaCongressoColor, 
				FontSize = 14, 
				XAlign = TextAlignment.Start,
				WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - 40,
				MinimumHeightRequest = 30
			};

			_postDataImage = new DownloadedImage(AppResources.DefaultPostImage, true) {
				WidthRequest = imageSize,
				HeightRequest = imageSize,
				RequiredSize = 300
			};

			var emptyBox = new BoxView {
				WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width),
				Color = Color.Transparent, 
				HeightRequest = 1
			};

			var likeImage = new Image 
			{
				WidthRequest = 30,
				HeightRequest = 30,
				Source = ImageLoader.Instance.GetImage(AppResources.WallPostLikeImage, true)
			};
			TapGestureRecognizer tapOnLikeImage = new TapGestureRecognizer ();
			tapOnLikeImage.Tapped += OnLikeImageTapped;
			likeImage.GestureRecognizers.Add (tapOnLikeImage);

			_likeLabel = new Label { 
				TextColor = AppResources.WallPostCreatedDateColor, 
				FontSize = 15, 
				XAlign = TextAlignment.Start,
				YAlign = TextAlignment.Center,
				HeightRequest = 30,
				WidthRequest = 60
			};

			TapGestureRecognizer tapOnLikeLabel = new TapGestureRecognizer ();
			tapOnLikeLabel.Tapped += OnLikeLabelTapped;
			_likeLabel.GestureRecognizers.Add (tapOnLikeLabel);

			View = new ContentView {
				Padding = new Thickness(0, 0, 0, 20),
				Content = new StackLayout { 
					Orientation = StackOrientation.Vertical,
					BackgroundColor = AppResources.WallPostBackgroundColor, 
					VerticalOptions = LayoutOptions.StartAndExpand,
					Children = {
						new StackLayout { //top layout
							Orientation = StackOrientation.Horizontal,
							HeightRequest = 40,
							Spacing = 0,
							Children = {
								new BoxView {
									HeightRequest = 40,
									WidthRequest = 10,
									BackgroundColor = AppResources.AgendaCongressoColor
								},
								_creatorUserImage,
								new StackLayout { //data
									Padding = new Thickness (10, 0, 0, 0),
									Spacing = 2,
									Orientation = StackOrientation.Vertical,
									Children = { _creatorNameLabel, _postDateLabel }
								}
							}
						},
						new StackLayout { //post
							Orientation = StackOrientation.Vertical,
							VerticalOptions = LayoutOptions.FillAndExpand,
							HorizontalOptions = LayoutOptions.StartAndExpand,
							Padding = new Thickness (LeftBorder, 5, LeftBorder, 0),
							Children = { _postInfoLabel, _postDataImage, emptyBox }
						},
						new StackLayout { //like
							Orientation = StackOrientation.Horizontal, 
							HorizontalOptions = LayoutOptions.EndAndExpand, 
							Padding = new Thickness (0, 5, 0, 5),
							Children = { likeImage, _likeLabel }
						}
					}
				}
			};
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			if(Model != null)
				Model.IsChanged -= OnModelChanged;

			Model = BindingContext as WallPost;

			if (Model != null) {

				_creatorUserImage.UpdateAtTime = Model.UpdatedAtTime;
				_creatorUserImage.ServerImagePath = Model.CreatorImage;

				_postDataImage.UpdateAtTime = Model.UpdatedAtTime;
				_postDataImage.ServerImagePath = Model.Image;

				_postDateLabel.Text = Model.PostDate;
				_postInfoLabel.Text = Model.Text;

				_creatorNameLabel.Text = Model.CreatorName;
				_likeLabel.Text = Model.PostLikes;

				Model.IsChanged += OnModelChanged;
				OnModelChanged ();
			}
		}

		void OnModelChanged ()
		{
			Device.BeginInvokeOnMainThread (() => {
				if(Model != null)
					_likeLabel.Text = Model.PostLikes;
			});
		}

		void OnLikeImageTapped (object sender, EventArgs e)
		{
			UserController.Instance.LikeWallPost (Model);
		}

		void OnLikeLabelTapped (object sender, EventArgs e)
		{
			if (Model.Likes == 0)
				return;

			AppController.Instance.AppRootPage.CurrentPage.Navigation.PushAsync (
				new WallPostLikersPage (Model));
		}
	}
}