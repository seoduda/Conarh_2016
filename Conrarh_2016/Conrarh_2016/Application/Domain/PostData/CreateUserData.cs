using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain.PostData
{
	public sealed class CreateUserData
	{
		[JsonProperty(User.JsonKeys.Name, NullValueHandling = NullValueHandling.Ignore)]
		public string Name;

		[JsonProperty(User.JsonKeys.UserName, NullValueHandling = NullValueHandling.Ignore)]
		public string Email;

		[JsonProperty(User.JsonKeys.Password, NullValueHandling = NullValueHandling.Ignore)]
		public string Password;

		[JsonProperty(User.JsonKeys.UserType, NullValueHandling = NullValueHandling.Ignore)]
		public string UserType = "5575ce28657f6f66f9ee570f";

		[JsonProperty(User.JsonKeys.Job, NullValueHandling = NullValueHandling.Ignore)]
		public string Job;

		[JsonProperty(User.JsonKeys.Phone, NullValueHandling = NullValueHandling.Ignore)]
		public string Phone;

		[JsonProperty(User.JsonKeys.ProfileImagePath, NullValueHandling = NullValueHandling.Ignore)]
		public string ProfileImage;

		[JsonProperty(User.JsonKeys.Points, NullValueHandling = NullValueHandling.Ignore)]
		public int? ScorePoints;

		public bool IsEmpty()
		{
			return string.IsNullOrEmpty (Name) &&
			string.IsNullOrEmpty (Email) &&
			string.IsNullOrEmpty (Job) &&
			string.IsNullOrEmpty (Phone);
		}
	}
}