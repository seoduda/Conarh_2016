using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;


namespace Conarh_2016.Application.DataAccess
{
	public sealed class DbClient : SQLiteClient
    {
        private const string SqliteFilename = "TaskDB.db3";

        public void Initialize(ISQLitePlatform sqlPlatform)
        {
            var connectionWithLock = new SQLiteConnectionWithLock(sqlPlatform,
                new SQLiteConnectionString(Path.Combine(AppProvider.IOManager.DocumentPath, SqliteFilename), true));
            var connection = new SQLiteAsyncConnection(() => connectionWithLock);
            DbClient = SQLiteClient.Create(connection);

            AppProvider.Log.WriteLine(LogChannel.DataBase, "DBClient: {0}", Path.Combine(AppProvider.IOManager.DocumentPath, SqliteFilename));
        }

		protected override async void InitializeAsync()
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				await connection.CreateTablesAsync<UserTask, User, Assets, SmartTaskAttachment, PersistentBackgroundWorker.DbPersistedTaskDescriptor>().ConfigureAwait(false);
			}
		}

		public async Task<List<UserTask>> GetTasks(User user)
		{
			List<UserTask> tasks;
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				tasks = await connection.Table<UserTask>().Where(x => x.UserId == user.PrimaryID).ToListAsync().ConfigureAwait(false);
			}

			return tasks;
		}

		public async Task<UserTask> GetUserTaskById(User user, string taskId)
		{
			UserTask task;
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				task = await connection.Table<UserTask>().Where(x => x.UserId == user.PrimaryID && x.Id == taskId).FirstOrDefaultAsync();
			}

			return task;
		}

		public async Task Save(UserTask task, User user)
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				UserTask existingTask = await connection.Table<UserTask>()
					.Where(x => x.Id == task.Id)
					.FirstOrDefaultAsync();

				task.UserId = user.PrimaryID;

				if (existingTask == null)
				{
					await connection.InsertAsync(task).ConfigureAwait(false);
				}
				else
				{
					task.PrimaryId = existingTask.PrimaryId;
					await connection.UpdateAsync(task).ConfigureAwait(false);
				}
			}
		}

		public async Task<int> UpdateTaskWithWorkflowStatus(List<string> faultedWorkflow, User user)
		{
			int updated;
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				List<UserTask> tasksFromDb = await connection.Table<UserTask>().Where(task => task.UserId == user.PrimaryID &&
					task.WorkflowStatus != "Faulted" && faultedWorkflow.Contains(task.InstanceId) ).ToListAsync();

				updated = tasksFromDb.Count;
				foreach (UserTask faultedTask in tasksFromDb)
				{
					faultedTask.WorkflowStatus = "Faulted";
					await connection.UpdateAsync(faultedTask).ConfigureAwait(false);
				}
			}

			return updated;
		}

		public async Task CleanupTasksAsync(IEnumerable<UserTask> rawTasks, User user)
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				List<UserTask> tasksFromDb = await connection.Table<UserTask>().Where(task => task.UserId == user.PrimaryID).ToListAsync();

				foreach (UserTask rawTask in rawTasks)
				{
					UserTask userTask = rawTask;
					UserTask found = tasksFromDb.Find(task => task.TaskId == userTask.TaskId);
					await connection.UpdateAsync(userTask).ConfigureAwait(false);

					tasksFromDb.Remove(found);
				}

				foreach (UserTask taskToDelete in tasksFromDb)
				{
					await connection.DeleteAsync(taskToDelete).ConfigureAwait(false);
				}
			}
		}

		public async Task SaveAll(IEnumerable<UserTask> rawTasks, User user)
		{
			foreach (UserTask task in rawTasks)
			{
				await Save(task, user);
			}
		}

		public async Task SaveAssets(Assets assets)
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				Assets exitingAssets = await connection.Table<Assets>()
					.Where(x => x.Server.Equals(assets.Server))
					.FirstOrDefaultAsync();

				if (exitingAssets == null)
					await connection.InsertAsync(assets).ConfigureAwait(false);
				else
					await connection.UpdateAsync(assets).ConfigureAwait(false);
			}
		}

		public async Task SaveUser(User user)
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				User exitingUser = await connection.Table<User>()
					.Where(x => x.Name == user.Name && x.Server == user.Server)
					.FirstOrDefaultAsync();

				if (user.SettingsID == null)
				{
				}

				if (exitingUser == null)
				{
					await connection.InsertAsync(user).ConfigureAwait(false);
				}
				else
				{
					await connection.UpdateAsync(user).ConfigureAwait(false);
				}
			}
		}

		public async Task<Assets> GetAssets(string server)
		{
			return await connection.Table<Assets>().Where(x => x.Server.Equals(server)).FirstOrDefaultAsync();
		}

		public async Task<User> GetUser(string username, string server)
		{
			return await connection.Table<User>().Where(x => x.Name == username && x.Server == server).FirstOrDefaultAsync();
		}

		public static SQLiteClient Create(SQLiteAsyncConnection connection)
		{
			var client = new SQLiteClient(connection);
			client.InitializeAsync();
			return client;
		}



		public async Task<SmartTaskAttachment> GetAttachment(string attachmentKey, string server)
		{
			SmartTaskAttachment attachment;
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				attachment = await connection.Table<SmartTaskAttachment>().Where(
					x => x.RecordId.Equals(attachmentKey) && x.Server.Equals(server)).FirstOrDefaultAsync();
			}

			return attachment;
		}

		public async Task SaveAttachment(SmartTaskAttachment attachment)
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				SmartTaskAttachment existingTask = await connection.Table<SmartTaskAttachment>()
					.Where(x => x.PrimaryId == attachment.PrimaryId && x.Server == attachment.Server)
					.FirstOrDefaultAsync();

				if (existingTask == null)
				{
					await connection.InsertAsync(attachment).ConfigureAwait(false);
				}
				else
				{
					attachment.PrimaryId = existingTask.PrimaryId;
					await connection.UpdateAsync(attachment).ConfigureAwait(false);
				}
			}
		}

		#region BackgroundTasks Data Access

		internal async Task<IEnumerable<PersistentBackgroundWorker.DbPersistedTaskDescriptor>> GetBackgroundTasksAsync(string queueName, int count, int skip = 0)
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
				return await connection.Table<PersistentBackgroundWorker.DbPersistedTaskDescriptor>().Where(d => d.QueueName == queueName).OrderBy(d => d.LastUpdated).
					Skip(skip).Take(count).ToListAsync();
		}

		internal async Task<IEnumerable<PersistentBackgroundWorker.DbPersistedTaskDescriptor>> GetBackgroundTasksAsync(string queueName)
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
				return await connection.Table<PersistentBackgroundWorker.DbPersistedTaskDescriptor>().Where(d => d.QueueName == queueName).OrderBy(d => d.LastUpdated).ToListAsync();
		}

		internal async Task<bool> SaveBackgroundTaskAsync(PersistentBackgroundWorker.DbPersistedTaskDescriptor descriptor)
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				if (await connection.Table<PersistentBackgroundWorker.DbPersistedTaskDescriptor>().Where(btd => btd.Id == descriptor.Id).FirstOrDefaultAsync() == null)
				{
					string uniqueId = descriptor.Task.UniqueId;
					var taskDescriptor = await connection.Table<PersistentBackgroundWorker.DbPersistedTaskDescriptor>().Where(btd => btd.UniqueId == uniqueId).FirstOrDefaultAsync().ConfigureAwait(false);
					if (string.IsNullOrEmpty(uniqueId) ||
						taskDescriptor == null)
					{
						await connection.InsertAsync(descriptor).ConfigureAwait(false);
						return true;
					}
				}
				else
				{
					await connection.UpdateAsync(descriptor).ConfigureAwait(false);
				}
			}
			return false;
		}

		internal async Task RemoveBackgroundTaskAsync(int backgroundTaskId)
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
				await connection.DeleteAsync<PersistentBackgroundWorker.DbPersistedTaskDescriptor>(backgroundTaskId);
		}

		#endregion
    }
}