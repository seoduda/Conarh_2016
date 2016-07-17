using Conarh_2016.Application.BackgroundTasks.GetData.Kinvey;
using Conarh_2016.Application.Domain;

namespace Conarh_2016.Application.BackgroundTasks
{
    //public sealed class DownloadUsersBackgroundTask : DownloadListBackgroundTask<User, RootListData<User>>
    public sealed class DownloadUsersBackgroundTask : DownloadListKinveyBackgroundTask<User, KinveyRootListData<User>>
	{

		public DownloadUsersBackgroundTask(DynamicListData<User> dataModel, KinveyDownloadListParameters parameters): base(dataModel, parameters)
		{
		}
	}
}