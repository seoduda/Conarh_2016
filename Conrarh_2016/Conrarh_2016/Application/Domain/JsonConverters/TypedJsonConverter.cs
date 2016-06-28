using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Conarh_2016.Application.Domain.JsonConverters
{
	public class TypedJsonConverter<T>:JsonConverter where T:class, new()
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var template = new T();
			var obj = JToken.ReadFrom(reader);

			serializer.Populate(obj.CreateReader(), template);

			return template;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(T);
		}
	}
}