
using Conarh_2016.Core.UI;
using UIKit;

namespace Conarh_2016.iOS.UI
{
	public sealed class ScreenOptions: IScreenOptions
	{
		public int ConvertPixelsToDp (float pixelValue)
		{
			return (int)pixelValue;
		}

		#region IScreenOptions implementation
		public int Width 
		{
			get 
			{
				return (int)UIScreen.MainScreen.Bounds.Width;
			}
		}
		public int Height 
		{
			get 
			{
				return (int)UIScreen.MainScreen.Bounds.Height;
			}
		}
		#endregion
		
	}
}

