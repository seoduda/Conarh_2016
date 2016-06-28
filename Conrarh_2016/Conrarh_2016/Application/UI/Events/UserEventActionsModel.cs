using Conarh_2016.Application.Domain;
using System.Collections.Generic;
using Conarh_2016.Core;

namespace Conarh_2016.Application.UI.Events
{
	public sealed class UserEventActionsModel
	{
		public readonly EventData Data;

		public List<UserVoteData> Model;

		public List<LikedItem> Items;

		public UserEventActionsModel(EventData data, List<UserVoteData> votes)
		{
			Model = votes;
			Data = data;

			Items = new List<LikedItem> ();

			foreach (UserVoteData vote in Model)
				Items.Add (vote.UserVoteType == UserVoteType.Speaker ? 
					GetSpeakerItem (vote) : GetEventItem (vote));
		}

		public void Update (List<UserVoteData> result)
		{
			foreach (UserVoteData voteData in result) 
			{
				UserVoteData current = Model.Find (temp => temp.Subject.Equals (voteData.Subject));

				if (current != null && current.UpdatedAtTime < voteData.UpdatedAtTime) {
					current.UpdateWithItem (voteData);

					LikedItem item = Items.Find (temp => temp.VoteData.Subject.Equals (current.Subject));
					item.Update ();
				}
			}
		}

		private LikedItem GetEventItem(UserVoteData voteData)
		{
			return new LikedItem (voteData) {
				UI_ImageHeight = 40,
				UI_NameFontSize = 20,
				UI_TextYPosition = 10,
				UI_LikeXPosition = AppProvider.Screen.ConvertPixelsToDp( AppProvider.Screen.Width ) - 100,
				UI_DislikeXPosition = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - 50
			};
		}

		private LikedItem GetSpeakerItem(UserVoteData voteData)
		{
			return new LikedItem (voteData) {
				UI_ImageHeight = 25,
				UI_NameFontSize = 14,
				UI_TextYPosition = 4,
				UI_LikeXPosition = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - 70,
				UI_DislikeXPosition = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - 40
			};
		}
	}
}

