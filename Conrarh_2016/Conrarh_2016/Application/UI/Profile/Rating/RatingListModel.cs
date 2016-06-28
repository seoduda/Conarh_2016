using Conarh_2016.Application.Domain;
using System.Collections.Generic;
using System;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class RatingUserModel
	{
		public readonly UserModel UserModel;
		public readonly int Index;

		public string IndexStr
		{
			get 
			{
				return string.Format ("#{0}", Index + 1);
			}
		}

		public string Points
		{
			get 
			{
				return string.Format ("{0}pts", UserModel.User.ScorePoints);
			}
		}

		public RatingUserModel(UserModel user, int index)
		{
			UserModel = user;
			Index = index;
		}

		public string Name
		{
			get 
			{
				return UserModel.User.Name;
			}
		}

		public string Job
		{
			get 
			{
				return UserModel.User.Job;
			}
		}

		public string ProfileImagePath
		{
			get 
			{
				return UserModel.User.ProfileImagePath;
			}
		}
	}

	public sealed class RatingListModel
	{
		public readonly List<RatingUserModel> Items;
		public readonly DynamicListData<User> Users;

		public event Action ItemsChanged;

		public RatingListModel()
		{
			Users = new DynamicListData<User> ();
			Items = new List<RatingUserModel> ();

			Users.CollectionChanged += UpdateUsers;
		}

		private RatingUserModel FindModel(string userId)
		{
			return Items.Find (temp => temp.UserModel.User.Id.Equals (userId));
		}

		private RatingUserModel GetModel(User user, int index)
		{
			UserModel model = AppModel.Instance.UsersModelsWrapper.UsersModels.Find (user.Id);
			RatingUserModel result = FindModel (user.Id);

			if (result == null) 
			{
				result = new RatingUserModel (model, index);
				Items.Add (result);
			}

			return result;
		}

		public void UpdateUsers(List<User> users)
		{
			Items.Clear ();

			int collectionCount = Items.Count;

			int index = 0;
			foreach (User user in users) {
				GetModel (user, index);
				index += 1;
			}

			RaiseCollectionChanged ();
		}

		private void RaiseCollectionChanged()
		{
			if (ItemsChanged != null)
				ItemsChanged ();
		}
	}
}