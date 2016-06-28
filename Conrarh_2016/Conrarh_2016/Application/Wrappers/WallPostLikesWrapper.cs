using System.Collections.Generic;
using Conarh_2016.Application.Domain;
using Xamarin.Forms;
using System.Collections.Specialized;

namespace Conarh_2016.Application.Wrappers
{
	public sealed class WallPostLikesWrapper:DynamicObservableData<UserModel>
	{
		public readonly DynamicListData<WallPostLike> Likes;

		public Dictionary<string, UserModel> Users;

		public WallPostLikesWrapper( DynamicListData<WallPostLike> likes):base(false)
		{
			Users = new Dictionary<string, UserModel> ();
			Likes = likes;

			if (!Likes.IsEmpty ())
				OnLikesCollectionChanged (Likes.Items);

			Likes.CollectionChanged += OnLikesCollectionChanged;
		}

		private void OnLikesCollectionChanged (List<WallPostLike> likes)
		{
			foreach (WallPostLike like in likes) {
				AddUserLike (like);
			}
		}

		void AddUserLike (WallPostLike like)
		{
			if (like.User == null)
				return;
			
			if (Users.ContainsKey (like.User.Id))
				return;

			UserModel model = AppModel.Instance.UsersModelsWrapper.UsersModels.Find (like.User.Id);
			bool isNew;

			if (model == null)
				model = AppModel.Instance.UsersModelsWrapper.AddModel (like.User, out isNew);

			Users.Add (like.User.Id, model);
			Device.BeginInvokeOnMainThread (() => {
				InsertItem(0, model); 
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Add, model));
			});
		}
	}
}

