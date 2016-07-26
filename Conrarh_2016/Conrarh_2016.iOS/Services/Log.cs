using System;
using Foundation;

namespace Conarh_2016.iOS.Services
{
	public sealed class Log: Conarh_2016.Core.Services.Log
	{
		public NSObject _object = new NSObject();

		protected override void Print (string info)
		{
			_object.InvokeOnMainThread( () => Console.WriteLine (info) );
		}
	}
}

