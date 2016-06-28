using System.Collections.Generic;
using Conarh_2016.Application.Domain;
using System;

namespace Conarh_2016.Application.Wrappers
{
	public sealed class UserModelsWrapper
	{
		public readonly DynamicListData<UserModel> UsersModels;
		public readonly DynamicListData<User> Users;

		public event Action<List<UserModel>> AddUsers;
		public event Action<List<UserModel>> UpdateUsers;


		public UserModelsWrapper(DynamicListData<User> users)
		{
			Users = users;
			UsersModels = new DynamicListData<UserModel> ();

			if (!Users.IsEmpty ())
				OnUsersChanged (Users.Items);

			Users.CollectionChanged += OnUsersChanged;
		}


		public UserModel AddModel(User user, out bool isNew)
		{
			isNew = false;

			UserModel model = UsersModels.Find (user.Id);
			if (model == null) {
				model = new UserModel (user);
				isNew = true;
				UsersModels.Items.Add (model);
			}
			else 
			{
				if (model.User.UpdatedAtTime < user.UpdatedAtTime)
					model.UpdateUser (user);
			}

			return model;
		}

		private void OnUsersChanged (List<User> data)
		{
			List<UserModel> added = new List<UserModel> ();
			List<UserModel> updated = new List<UserModel> ();

			foreach (User user in data) {
				bool isNew;
				var model = AddModel (user, out isNew);
				if (isNew)
					added.Add (model);
				else
					updated.Add (model);
			}

			if (added.Count > 0 && AddUsers != null)
				AddUsers (added);

			if (updated.Count > 0 && UpdateUsers != null)
				UpdateUsers (updated);
		}
	}
}

