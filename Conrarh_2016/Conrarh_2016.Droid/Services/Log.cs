using System;

namespace Conarh_2016.Android.Services
{
	public sealed class Log: Conarh_2016.Core.Services.Log
	{
		protected override void Print (string info)
		{
			Console.WriteLine (info);
		}
	}
}

