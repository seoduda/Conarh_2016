using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Shared
{
	public abstract class DynamicChildContentView: ContentView
	{
		public readonly int Index = 0;

		public DynamicChildContentView(int index)
		{
			Index = index;
		}
	}
}

