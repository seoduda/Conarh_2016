using System;

namespace Conarh_2016.Core.UI
{
	public interface IScreenOptions
	{
		int Width { get; }
		int Height { get; }
		int ConvertPixelsToDp(float pixelValue);
	}
}

