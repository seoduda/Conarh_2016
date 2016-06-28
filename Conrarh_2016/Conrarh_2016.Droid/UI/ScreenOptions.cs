using Conarh_2016.Core.UI;
using Android.Util;
using Android.Content.Res;

namespace Conarh_2016.Android.UI
{
	public sealed class ScreenOptions: IScreenOptions
	{
		DisplayMetrics _metrics;

		public ScreenOptions()
		{
			_metrics = Resources.System.DisplayMetrics;
		}

		public int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int) ((pixelValue)/_metrics.Density);
			return dp;
		}

		public int Width 
		{
			get 
			{
				return _metrics.WidthPixels;
			}
		}
		public int Height 
		{
			get 
			{
				return _metrics.HeightPixels;
			}
		}

	}
}

