using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using Newtonsoft.Json;
using Conarh_2016.Core.DataAccess;
using Conarh_2016.Core;
using Conarh_2016.Application.Domain;
using Conarh_2016.Core.Services;

namespace Conarh_2016.Application.DataAccess
{
	public sealed class DbClient : SQLiteClient
    { 
		private static DbClient instance;
		public static DbClient Instance
		{
			get
			{
				if (instance == null)
					instance = new DbClient ();

				return instance;
			}
		}
		
        private const string SqliteFilename = "db.db3";

        public void Initialize(ISQLitePlatform sqlPlatform)
        {
            var connectionWithLock = new SQLiteConnectionWithLock(sqlPlatform,
                new SQLiteConnectionString(Path.Combine(AppProvider.IOManager.DocumentPath, SqliteFilename), true));
            
			connection = new SQLiteAsyncConnection(() => connectionWithLock);
			InitializeAsync ();

            AppProvider.Log.WriteLine(LogChannel.DataBase, "DBClient: {0}", Path.Combine(AppProvider.IOManager.DocumentPath, SqliteFilename));
        }

		protected override async void InitializeAsync()
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				await connection.CreateTablesAsync<User, EventData, SponsorType, BadgeType, Exhibitor>().ConfigureAwait(false);
				await connection.CreateTablesAsync<WallPost, ConnectRequest, ImageData, BadgeAction, WallPostLike>().ConfigureAwait(false);
				await connection.CreateTablesAsync<FavouriteEventData, UserVoteData, Speaker, PushNotificationData, AppInformation>().ConfigureAwait(false);
			}
		}

		public async Task SaveData<T>(T data) where T: UniqueItem
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				T existingItem = await connection.Table<T>()
					.Where(x => x.Id == data.Id)
					.FirstOrDefaultAsync();
	
				if (existingItem == null)
					await connection.InsertAsync (data).ConfigureAwait (false);
				else
					await connection.UpdateAsync (data).ConfigureAwait (false);
			}
		}
			
		public async Task SaveItemData<T>(T data) where T: UpdatedUniqueItem
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				T existingItem = await connection.Table<T>()
					.Where(x => x.Id == data.Id)
					.FirstOrDefaultAsync();

				if (data is Exhibitor) {
					var exhibitor = data as Exhibitor;
					exhibitor.SponsorTypeId = exhibitor.SponsorType.Id;
				}
				else if (data is WallPost) {
					var wallPost = data as WallPost;
					wallPost.CreatedUserId = wallPost.CreatedUser.Id;
				}
				else if (data is ConnectRequest) {
					var connectRequest = data as ConnectRequest;
                    /* TODO validar
					connectRequest.RequesterId = connectRequest.Requester.Id;
					connectRequest.ResponderId = connectRequest.Responder.Id;
                    */
				}
				else if (data is EventData) {
					var eventData = data as EventData;
					List<string> ids = new List<string> ();
                    foreach (Speaker speaker in eventData.Speechers)
                    {
                        /* Todo arrumar gambi da ID DB_clien.cs */
                        //speaker.Id = speaker.Xid;
                        ids.Add(speaker.Id);
                    }
					eventData.SpeechersList = JsonConvert.SerializeObject (ids);
				}
				else if (data is WallPostLike) {
					var wallPostLike = data as WallPostLike;
					wallPostLike.UserId = wallPostLike.User.Id;
				} 

				if (existingItem == null)
					await connection.InsertAsync (data).ConfigureAwait (false);
				else
				{
					if(existingItem.UpdatedAtTime < data.UpdatedAtTime)
						await connection.UpdateAsync (data).ConfigureAwait (false);
				}
			}
		}

		public async Task<PushNotificationData> GetPushData(string userId, string token)
		{
			PushNotificationData data;
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				data = await connection.Table<PushNotificationData>().Where(temp => temp.UserId.Equals(userId) && temp.Token.Equals(token)).FirstOrDefaultAsync().ConfigureAwait(false);
			}

			return data;
		}

		public async Task SaveData<T>(List<T> dataList) where T: UpdatedUniqueItem
		{
			foreach (T data in dataList)
				await SaveItemData (data);
		}

		public async Task<T> GetData<T>(string id) where T: UniqueItem
		{
			T data;
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				data = await connection.Table<T>().Where(temp => temp.Id.Equals(id)).FirstOrDefaultAsync().ConfigureAwait(false);
			}

			return data;
		}

		public async Task<List<T>> GetData<T>() where T: UpdatedUniqueItem
		{
			List<T> data;
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				data = await connection.Table<T>().ToListAsync().ConfigureAwait(false);
			}

			return data;
		}

		public async Task<List<ConnectRequest>> GetUserConnectRequest(string userId)
		{
			List<ConnectRequest> data;
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				data = await connection.Table<ConnectRequest>().Where(temp => temp.RequesterId.Equals(userId) || temp.ResponderId.Equals(userId)).ToListAsync().ConfigureAwait(false);
			}

			return data;
		}
			
		public async Task<List<BadgeAction>> GetUserBadgeActions(string userId)
		{
			List<BadgeAction> data;
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				data = await connection.Table<BadgeAction>().Where(temp => temp.User.Equals(userId)).ToListAsync().ConfigureAwait(false);
			}

			return data;
		}

		public async Task<List<FavouriteEventData>> GetUserFavouriteActions (string userId)
		{
			List<FavouriteEventData> data;
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				data = await connection.Table<FavouriteEventData>().Where(temp => temp.User.Equals(userId)).ToListAsync().ConfigureAwait(false);
			}

			return data;
		}

		public async Task<List<UserVoteData>> GetUserEventVotes (string userId)
		{
			List<UserVoteData> data;
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				data = await connection.Table<UserVoteData>().Where(temp => temp.User.Equals(userId)).ToListAsync().ConfigureAwait(false);
			}

			return data;
		}

		public async Task DeleteItemData<T> (T data) where T:UniqueItem
		{
			using (await mutex.LockAsync().ConfigureAwait(false))
			{
				await connection.DeleteAsync(data).ConfigureAwait(false);
			}
		}

    }
}