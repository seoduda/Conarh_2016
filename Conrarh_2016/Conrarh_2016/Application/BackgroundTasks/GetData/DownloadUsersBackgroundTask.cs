using Conarh_2016.Application.Domain;

namespace Conarh_2016.Application.BackgroundTasks
{
	public sealed class DownloadUsersBackgroundTask : DownloadListBackgroundTask<User, RootListData<User>>
	{
		public DownloadUsersBackgroundTask(DynamicListData<User> dataModel, DownloadListParameters parameters): base(dataModel, parameters)
		{
		}
	}
}