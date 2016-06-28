using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain.PostData
{
	public sealed class AddValueData:UniqueItem
	{
		[JsonProperty(JsonKeys.Value)]
		public string Value;

		public new static class JsonKeys
		{
			public const string Value = "value";
		}

		public AddValueData()
		{
		}

		public AddValueData( string addedValue)
		{
			Value = addedValue;
		}
	}
}