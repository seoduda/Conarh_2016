using Newtonsoft.Json;

namespace Conarh_2016.Application.Domain
{
	public sealed class BadgeAction:UpdatedUniqueItem
	{
		[JsonProperty(JsonKeys.User)]
		public string User
		{
			set;
			get;
		}

		[JsonProperty(JsonKeys.BadgeAction)]
		public string Badge
		{
			set;
			get;
		}

		public new static class JsonKeys
		{
			public const string User = "user";
			public const string BadgeAction = "badge";
		}

		public override string ToString ()
		{
			return string.Format ("[BadgeAction] [ Id: {0} User: {1} BadgeAction: {2} ]", 
				Id, User, Badge);
		}

		public BadgeAction()
		{
		}

		public BadgeAction(string userId, string badgeId)
		{
			User = userId;
			Badge = badgeId;
		}

	}
}