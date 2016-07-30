using Conarh_2016.Application.Domain;
using Conarh_2016.Application.Tools;
using System.Collections.Generic;
using Conarh_2016.Application.DataAccess;
using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;

namespace Conarh_2016.Application.BackgroundTasks
{
    public sealed class DownloadWallPostsBackgroundTask : DownloadListKinveyBackgroundTask<WallPost, KinveyRootListData<WallPost>>
    {
        public DownloadWallPostsBackgroundTask(DynamicListData<WallPost> data) : base(data,
            new KinveyDownloadListParameters(KinveyDownloadCountType.All, QueryBuilder.Instance.GetWallPostskinveyQuery()))
        {
        }

        public override List<WallPost> Execute()
        {
            /*

            if (AppModel.Instance.SponsorTypes.Items.Count > 0)
                return new List<SponsorType>();
                */

           // List<SponsorType> result;
            var result = base.Execute();


            //List<SponsorType> result = base.Execute();

            if (result != null)
                AppModel.Instance.WallPosts.UpdateData(result);

            return result;
        }

        protected override void OnSaveData(List<WallPost> data)
        {
            foreach (WallPost wallPost in data)
               
            {
                wallPost.CreatedUser = AppModel.Instance.Users.Find(wallPost.CreatedUserId);
                AppModel.Instance.Users.UpdateData(new List<User> { wallPost.CreatedUser });
                DbClient.Instance.SaveItemData<User>(wallPost.CreatedUser).ConfigureAwait(false);
            }
        }
    }
}

 