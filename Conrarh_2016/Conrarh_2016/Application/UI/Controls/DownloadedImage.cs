using Conarh_2016.Application.Domain;
using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Controls
{
	#if __ANDROID__

	public sealed class DownloadedImage:FastImage
	{
		public const string ServerImagePathName = "ServerImagePath";
		public const string UpdateAtTimeName = "UpdateAtTime";

		public event Action ImageLoaded;

		public readonly string DefaultImagePath;
		public readonly bool HideIfEmpty;

		private ImageData _currentData;

		public static readonly BindableProperty ServerImagePathProperty =
		BindableProperty.Create(ServerImagePathName, typeof(string), typeof(DownloadedImage), string.Empty);

		public static readonly BindableProperty UpdateAtTimeProperty =
		BindableProperty.Create(UpdateAtTimeName, typeof(DateTime), typeof(DownloadedImage), DateTime.MinValue);

		public string ServerImagePath
		{
			get { return (string)this.GetValue(ServerImagePathProperty); }
			set { this.SetValue(ServerImagePathProperty, value); }
		}

		public DateTime UpdateAtTime
		{
			get { return (DateTime)this.GetValue(UpdateAtTimeProperty); }
			set { this.SetValue(UpdateAtTimeProperty, value); }
		}

		protected override void OnPropertyChanged (string propertyName = null)
		{
			base.OnPropertyChanged (propertyName);

			if (propertyName.Equals (ServerImagePathName)) 
			{
				Source = ImageLoader.Instance.GetImage(DefaultImagePath, true);
				if (!string.IsNullOrEmpty (ServerImagePath))
					Download ();
				else {
					IsVisible = !HideIfEmpty;
					ImageUrl = DefaultImagePath;
				}
			}
		}

		public DownloadedImage (string defaultImageSource, bool hideIfEmpty = false)
		{
			DefaultImagePath = defaultImageSource;
			HideIfEmpty = hideIfEmpty;
			Source = ImageLoader.Instance.GetImage(DefaultImagePath, true);
		}

		private void Download()
		{
			ImageLoader.Instance.LoadImage (ServerImagePath, UpdateAtTime, UpdateImage);
		}

		public void UpdateDefault ()
		{
			Source = ImageLoader.Instance.GetImage(DefaultImagePath, true);
		}

		public void UpdateImage(ImageData imageData)
		{
			Device.BeginInvokeOnMainThread(() => {
				if(_currentData != imageData)
				{
					_currentData = imageData;

					Source = null;
					ImageUrl = _currentData.ImagePath;

					if(ImageLoaded != null)
						ImageLoaded();
				}
			});
		}
	}

	#else

	public sealed class DownloadedImage:Image
	{
		public const string ServerImagePathName = "ServerImagePath";
		public const string UpdateAtTimeName = "UpdateAtTime";

		public int RequiredSize = 64;
		public event Action ImageLoaded;

		public readonly string DefaultImagePath;
		public readonly bool HideIfEmpty;

		private ImageData _currentData;

		public static readonly BindableProperty ServerImagePathProperty =
			BindableProperty.Create(ServerImagePathName, typeof(string), typeof(DownloadedImage), string.Empty);

		public static readonly BindableProperty UpdateAtTimeProperty =
			BindableProperty.Create(UpdateAtTimeName, typeof(DateTime), typeof(DownloadedImage), DateTime.MinValue);
		
		public string ServerImagePath
		{
			get { return (string)this.GetValue(ServerImagePathProperty); }
			set { this.SetValue(ServerImagePathProperty, value); }
		}

		public DateTime UpdateAtTime
		{
			get { return (DateTime)this.GetValue(UpdateAtTimeProperty); }
			set { this.SetValue(UpdateAtTimeProperty, value); }
		}

		protected override void OnPropertyChanged (string propertyName = null)
		{
			base.OnPropertyChanged (propertyName);

			if (propertyName.Equals (ServerImagePathName)) 
			{
				if (!string.IsNullOrEmpty (ServerImagePath))
					Download ();
				else {
					IsVisible = !HideIfEmpty;
					UpdateDefault ();
				}
			}
		}

		public DownloadedImage (string defaultImageSource, bool hideIfEmpty = false)
		{
			DefaultImagePath = defaultImageSource;
			HideIfEmpty = hideIfEmpty;
			Source = ImageLoader.Instance.GetImage(DefaultImagePath, true);
		}

		private void Download()
		{
			ImageLoader.Instance.LoadImage (ServerImagePath, UpdateAtTime, UpdateImage);
		}

		public void UpdateDefault ()
		{
			Source = ImageLoader.Instance.GetImage(DefaultImagePath, true);
		}

		public void UpdateImage(ImageData imageData)
		{
			Device.BeginInvokeOnMainThread(() => {
				if(_currentData != imageData)
				{
					_currentData = imageData;

					Source = ImageLoader.Instance.GetImage(imageData.ImagePath, false);

					if(ImageLoaded != null)
						ImageLoaded();
				}
			});
		}
	}
	#endif
}

