using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain
{
	public sealed class ServerError
	{
		[JsonProperty(JsonKeys.Error)]
		public string ErrorMsg;

		public static class JsonKeys
		{
			public const string Error = "error";
		}	

		public string ErrorMessage
		{
			get {
				return AppResources.GetLocalizedError (ErrorMsg);
			}
		}
	}
}
