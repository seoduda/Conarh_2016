using System.Collections.Generic;
using Conarh_2016.Application.Domain;

namespace Conarh_2016.Application.Wrappers
{
	public sealed class WallPostsDataWrapper:DynamicObservableData<WallPost>
	{
		public readonly DynamicListData<WallPost> WallPosts;

		public WallPostsDataWrapper( DynamicListData<WallPost> posts):base(true)
		{
			WallPosts = posts;

			if (!WallPosts.IsEmpty ())
				OnWallPostsChanged (WallPosts.Items);

			WallPosts.CollectionChanged += OnWallPostsChanged;
		}

		private void OnWallPostsChanged (List<WallPost> posts)
		{
			UpdateData (posts);
		}
	}
}

