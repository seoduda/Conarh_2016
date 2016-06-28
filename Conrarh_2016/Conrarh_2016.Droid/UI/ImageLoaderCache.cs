using System;
using MonoDroidToolkit.ImageLoader;
using Conarh_2016.Core.Services;

namespace TwinTechs.Droid.Controls
{
	/**
	 * caches available image loaders.
	 * TODO this needs to be written
	 */
	public class ImageLoaderCache:IImageCache
	{
		#region IImageCache implementation

		public void Clear ()
		{
			if (_onlyLoader != null) {
				_onlyLoader.ClearCache ();
			}
		}

		#endregion

		//TODO change to a proper dictionary
		private ImageLoader _onlyLoader;

		public ImageLoader GetImageLoader (FastImageRenderer imageRenderer)
		{
			//TODO
			if (_onlyLoader == null) {
				_onlyLoader = new ImageLoader (Android.App.Application.Context);
			}
			return _onlyLoader;
		}
	}
}

