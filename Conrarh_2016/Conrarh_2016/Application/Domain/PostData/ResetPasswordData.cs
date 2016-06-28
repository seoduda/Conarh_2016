using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain.PostData
{
	public sealed class ResetPasswordData
	{
		[JsonProperty(User.JsonKeys.UserName)]
		public string Username;

		[JsonProperty("length")]
		public int Length = 4;

		public ResetPasswordData()
		{
		}

		public ResetPasswordData( string email)
		{
			Username = email;
		}
	}
}

//{
//	"username": "guilherme_mds@hotmail.com",
//	"length": 4
//}


