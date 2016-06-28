using Newtonsoft.Json;
using System.Collections.Generic;

namespace Conarh_2016.Application.Domain
{
	public class RootListData
	{
		[JsonProperty(JsonKeys.CountPerPage)]
		public int CountPerPage;

		[JsonProperty(JsonKeys.TotalPages)]
		public int TotalPages;

		[JsonProperty(JsonKeys.CurrentPage)]
		public int CurrentPage;

		public static class JsonKeys
		{
			public const string CountPerPage = "per_page";
			public const string TotalPages = "total";
			public const string Data = "data";
			public const string CurrentPage = "current_page";
		}
	}

	public class RootListData<T> :RootListData where T:UniqueItem
	{
		[JsonProperty(RootListData.JsonKeys.Data)]
		public List<T> Data { get; set; }

		public override string ToString ()
		{
			return string.Format ("[RootListData: CurrentPage={0} TotalPages={1} Data={2}]", CurrentPage, TotalPages, Data.Count);
		}
	}
}
