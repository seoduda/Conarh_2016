﻿using Xamarin.Forms;
using Conarh_2016.Application;
using Conarh_2016.Application.UI.Controls;
using Conarh_2016.Core;
using System.IO;
using Conarh_2016.Application.UI.Shared;

namespace Conarh_2016.Application.UI.Wall
{
	public sealed class WallCreatePostPage : ContentPage
	{
		private readonly EditorControl _postTextField;
		private const int EditorHeight = 100;
		private const int Border = 20;

		private Image _postImage;
		private const int PostImageWidth = 450;

		private readonly string _fakeImagePath;

		public WallCreatePostPage ()
		{
			_fakeImagePath = Path.Combine(AppProvider.IOManager.DocumentPath, "fakeImage.jpeg");

			Title = AppResources.WallCreatePost;

			BackgroundColor = AppResources.WallPageBackgroundColor;

			var layout = new StackLayout  { Orientation = StackOrientation.Vertical, 
				BackgroundColor = AppResources.WallPostBackgroundColor, 
				Padding = new Thickness(Border),
				Spacing = 5};

			var topLayout = new StackLayout  { Orientation = StackOrientation.Horizontal };
			var userImage = new DownloadedImage(AppResources.DefaultUserImage) 
			{
				WidthRequest = 40,
				HeightRequest = 40
			};
			userImage.UpdateAtTime = AppModel.Instance.CurrentUser.User.UpdatedAtTime;
			userImage.ServerImagePath = AppModel.Instance.CurrentUser.User.ProfileImagePath;

			topLayout.Children.Add (userImage);

			var userNameLabel = new Label { TextColor = AppResources.AgendaCongressoColor, FontSize = 15, XAlign = TextAlignment.Start};
			userNameLabel.Text = AppModel.Instance.CurrentUser.User.Name;
		
			topLayout.Children.Add (userNameLabel);

			layout.Children.Add (topLayout);

			_postImage = new Image { WidthRequest = PostImageWidth,IsVisible = false, Aspect = Aspect.AspectFit};
			layout.Children.Add (_postImage);

			float btnWidth = (AppProvider.Screen.Width - Border) / 2;
			var btnUpload = new Button { 
				BackgroundColor = AppResources.AgendaPageBackgroundColor,
				WidthRequest = btnWidth,
				HeightRequest = 40,
				Text = AppResources.WallPostButtonUpload,
				TextColor = Color.White,
				BorderRadius = 0
			};
			btnUpload.Clicked += OnUploadClicked;;
			layout.Children.Add (btnUpload);

			_postTextField = new EditorControl(string.Empty) {
				HeightRequest = EditorHeight,
				WidthRequest = AppProvider.Screen.Width - Border * 2,
				BackgroundColor = Color.White
			};

			layout.Children.Add (_postTextField);

			var btnPost = new Button { 
				BackgroundColor = AppResources.AgendaPageBackgroundColor,
				WidthRequest = btnWidth,
				Text = AppResources.WallPostButtonPost,
				TextColor = Color.White,
				BorderRadius = 0
			};
			btnPost.Clicked += OnPostClicked;;
			layout.Children.Add (btnPost);

			Content = new ScrollView() {Content = layout};
		}

		void OnUploadClicked (object sender, System.EventArgs e)
		{
			AppController.Instance.AddImage (_fakeImagePath, OnAddImage, 350);
		}

		private void OnAddImage()
		{
			Device.BeginInvokeOnMainThread (() => {
				_postImage.Source = ImageLoader.Instance.GetImage(_fakeImagePath, false);
				_postImage.IsVisible = true;
			});
		}
			
		void OnPostClicked (object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty (_postTextField.Text)) {
				AppProvider.PopUpFactory.ShowMessage (AppResources.WallPostAddSomeTextMessage, AppResources.Warning);
				return;
			}
			UserController.Instance.CreateWallPost (_postTextField.Text, _postImage.IsVisible ? _fakeImagePath : string.Empty);
		}
	}
}