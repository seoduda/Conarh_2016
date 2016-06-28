using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain.PostData
{
	public sealed class LoginUserData:UniqueItem
	{
		[JsonProperty(User.JsonKeys.UserName)]
		public string Email;

		[JsonProperty(User.JsonKeys.Password)]
		public string Password;
	}
}
