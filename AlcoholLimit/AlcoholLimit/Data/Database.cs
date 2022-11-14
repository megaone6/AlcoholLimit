using AlcoholLimit.Utilities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoholLimit.Data
{
    public abstract class Database<T> where T : new()
    {
        protected SQLiteAsyncConnection database;

        protected async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await database.CreateTableAsync<T>();
        }

        #region Public methods

        public async Task<List<T>> GetItemsAsync()
        {
            await Init();
            return await database.Table<T>().ToListAsync();
        }

        public abstract Task<T> GetItemAsync(int id);

        public abstract Task<T> GetItemAsync(string name);

        public abstract Task<int> SaveItemAsync(T item);

        public async Task<int> DeleteItemAsync(T item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }

        public async Task<int> DeleteAllItemAsync()
        {
            await Init();
            return await database.DeleteAllAsync<T>();
        }

        #endregion
    }
}
