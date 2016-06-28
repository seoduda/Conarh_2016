using Conarh_2016.Application.Domain;
using System;
using Conarh_2016.Application;
using Xamarin.Forms;

namespace Conarh_2016.Application.Wrappers
{
	public enum ConnectState
	{
		RequestNotSent = 0,
		RequestSent = 1,
		RequestAccepted = 2,
		RequestedToAccept = 3
	}

	public interface IIndex
	{
		int Index {
			set;
		}
	}

	public sealed class ConnectionModel: UpdatedUniqueItem, IIndex
	{
		#region IIndex implementation

		private int index = -1;
		public event Action<int> IndexChanged;
		public int Index
		{
			get 
			{
				return index;
			}
			set 
			{
				if (index != value) {
					index = value;
					if (IndexChanged != null)
						IndexChanged (index);
				}
			}
		}

		#endregion

		public ConnectRequest Request {private set; get;}
		public event Action IsChanged;

		public readonly UserModel UserModel;
		public ConnectState State { private set; get;}

		public ConnectionModel(UserModel user)
		{
			UserModel = user;
			State = ConnectState.RequestNotSent;
		}

		public string Name
		{
			get 
			{
				return UserModel.User.Name;
			}
		}

		public string ProfileImagePath
		{
			get 
			{
				return UserModel.User.ProfileImagePath;
			}
		}

		public DateTime UserImageUpdateAtTime
		{
			get 
			{
				return UserModel.User.UpdatedAtTime;
			}
		}

		public string Job
		{
			get {
				return UserModel.User.Job;
			}
		}

		public override int CompareTo (UpdatedUniqueItem other)
		{
			var otherModel = other as ConnectionModel;
			return otherModel.GetUpdatedTime().CompareTo (GetUpdatedTime());
		}

		private DateTime GetUpdatedTime()
		{
			return Request != null ? Request.UpdatedAtTime : UserModel.User.UpdatedAtTime;
		}

		private void RequestIsChanged(ConnectRequest request)
		{
			if (Request.Accepted)
				State = ConnectState.RequestAccepted;
			else 
			{
				if (UserModel.User.Id.Equals (Request.Requester.Id))
					State = ConnectState.RequestedToAccept;
				else if (UserModel.User.Id.Equals (Request.Responder.Id))
					State = ConnectState.RequestSent;
				else
					State = ConnectState.RequestNotSent;
			}

			RaiseIsChanged ();
		}

		public void ApplyConnectRequest(ConnectRequest request)
		{
			if (Request != null)
				Request.IsChanged -= RequestIsChanged;

			Request = request;

			if (Request != null)
				Request.IsChanged += RequestIsChanged;

			RequestIsChanged (Request);
		}

		private void RaiseIsChanged()
		{
			if (IsChanged != null)
				IsChanged ();
		}

		public string BtnStateHeader
		{
			get 
			{
				if(State == ConnectState.RequestNotSent)
					return AppResources.ConnectRequestNotSent;

				if(State == ConnectState.RequestSent)
					return AppResources.ConnectRequestSent;

				if(State == ConnectState.RequestedToAccept)
					return AppResources.ConnectAcceptRequest;

				return string.Empty;
			}
		}

		public Color BtnStateColor
		{
			get 
			{
				if(State == ConnectState.RequestNotSent)
					return AppResources.ConnectRequestNotSentColor;

				if(State == ConnectState.RequestSent)
					return AppResources.ConnectRequestSentColor;

				if(State == ConnectState.RequestedToAccept)
					return AppResources.ConnectAcceptRequestColor;

				return Color.Blue;
			}
		}

		public const string HeaderLabelPropertyName = "HeaderLabel";
		public const string DateLabelPropertyName = "DateLabel";

		private User ConnectedUser
		{
			get
			{
				return Request.Responder.Id.Equals (AppModel.Instance.CurrentUser.User.Id) ? Request.Requester : Request.Responder;
			}
		}

		public FormattedString HeaderLabel
		{
			get 
			{
				var fs = new FormattedString ();
				fs.Spans.Add (new Span { Text = string.Format (AppResources.ProfileHistoryItemHeaderFormat, AppResources.GetName(ConnectedUser.Name)), ForegroundColor = Color.White, FontSize = 14});
				fs.Spans.Add (new Span { Text= string.Format (" +{0}", Request.PointsEarned), ForegroundColor = Color.White, FontSize = 22, FontAttributes = FontAttributes.Bold });
				fs.Spans.Add (new Span { Text=" pts", ForegroundColor = Color.White, FontSize = 14 });
				return fs;
			}
		}

		public string DateLabel
		{
			get {
				return string.Format("{0}h:{1}m {2}/{3}/{4}", 
					Request.UpdatedAtTime.Hour, 
					Request.UpdatedAtTime.Minute, 
					Request.UpdatedAtTime.Day, 
					Request.UpdatedAtTime.Month, 
					Request.UpdatedAtTime.Year);
			}
		}

	}
}

