using Xamarin.Forms;
using TwinTechs.Controls;
using Xamarin.Forms.Platform.Android;
using MonoDroidToolkit.ImageLoader;
using TwinTechs.Droid.Controls;
using Conarh_2016.Core;

[assembly: ExportRenderer (typeof(FastImage), typeof(FastImageRenderer))]
namespace TwinTechs.Droid.Controls
{
	public class FastImageRenderer : ImageRenderer
	{
		ImageLoader _imageLoader;

		protected override void OnElementChanged (ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged (e);
			if (e.NewElement != null) {
				var fastImage = e.NewElement as FastImage;
				_imageLoader = (AppProvider.ImageCache as ImageLoaderCache).GetImageLoader (this);
				SetImageUrl (fastImage.ImageUrl, fastImage.RequiredSize);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == "ImageUrl") {
				var fastImage = Element as FastImage;
				SetImageUrl (fastImage.ImageUrl, fastImage.RequiredSize);
			}
		}


		public void SetImageUrl (string imageUrl, int size)
		{
			if (Control == null) {
				return;
			}
			if (imageUrl != null) {
				_imageLoader.DisplayImage (imageUrl, Control, -1, size);
			}
		}
	}
}

