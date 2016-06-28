using Conarh_2016.Application.Domain;
using System;
using System.Collections.Generic;

namespace Conarh_2016.Application
{
	public sealed class UserModel:UpdatedUniqueItem
	{
		public bool IsEditable ()
		{
			return this == AppModel.Instance.CurrentUser;
		}

		public event Action IsChanged;

		public User User { private set; get; }

		public DynamicListData<ConnectRequest> Connections;
		public DynamicListData<BadgeAction> BadgeActions;
		public DynamicListData<FavouriteEventData> FavouriteActions;
		public DynamicListData<UserVoteData> VoteData;
		public DynamicListData<ConnectRequest> AcceptedConnections;

		public PushNotificationData PushNotificationData { private set; get;}

		public override int CompareTo (UpdatedUniqueItem other)
		{
			return UpdatedAtTime.CompareTo (other.UpdatedAtTime);
		}

		public UserModel (User user)
		{
			Id = user.Id;
			User = user;
			UpdatedAtTime = user.UpdatedAtTime;

			Connections = new DynamicListData<ConnectRequest> ();
			BadgeActions = new DynamicListData<BadgeAction> ();
			FavouriteActions = new DynamicListData<FavouriteEventData> ();
			VoteData = new DynamicListData<UserVoteData> ();

			AcceptedConnections = new DynamicListData<ConnectRequest> ();
			Connections.CollectionChanged += OnConnectionsChanged;
		}

		public void UpdatePushData(PushNotificationData pushData)
		{
			PushNotificationData = pushData;
		}

		void OnConnectionsChanged (List<ConnectRequest> connections)
		{
			AcceptedConnections.UpdateData (connections.FindAll (temp => temp.Accepted));
		}
			
		public void UpdateUser(User user)
		{
			User = user;
			UpdatedAtTime = user.UpdatedAtTime;

			if (IsChanged != null)
				IsChanged ();
		}

		public override void UpdateWithItem (UniqueItem item)
		{
			UserModel model = item as UserModel;
			if (model != null) 
				UpdateUser (model.User);
		}

		public bool GetBadgeState (BadgeType badge)
		{
			AppBadgeType appBadgeType = AppResources.BadgeTypes[badge.Id];

			List<BadgeAction> actions;

			switch (appBadgeType) 
			{
				case AppBadgeType.ConnectTo10Users:
				case AppBadgeType.ConnectTo3Users:
				case AppBadgeType.ConnectTo50Users:
					string badgeId = AppResources.GetBadgeIdByType (AppBadgeType.ConnectTo50Users);
					actions = BadgeActions.Items.FindAll (temp => temp.Badge.Equals (badgeId));
					break;
				default:
					actions = BadgeActions.Items.FindAll (temp => temp.Badge.Equals (badge.Id));
					break;
			}

			return AppResources.IsBadgeEnabled (badge.Id, actions.Count);
		}


		public string Name
		{
			get 
			{
				return User.Name;
			}
		}

		public string ProfileImagePath
		{
			get 
			{
				return User.ProfileImagePath;
			}
		}

		public DateTime UserImageUpdateAtTime
		{
			get 
			{
				return User.UpdatedAtTime;
			}
		}

		public string Job
		{
			get {
				return User.Job;
			}
		}
	}
}
