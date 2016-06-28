using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite.Net.Async;
using Core.DataAccess;

namespace Conarh_2016.Core.DataAccess
{
	public abstract class SQLiteClient
    {
		protected static readonly AsyncLock mutex = new AsyncLock();
		protected SQLiteAsyncConnection connection;

		private void InitConnection(SQLiteAsyncConnection asyncConnection)
        {
            connection = asyncConnection;
        }

		protected abstract void InitializeAsync();
    }
}