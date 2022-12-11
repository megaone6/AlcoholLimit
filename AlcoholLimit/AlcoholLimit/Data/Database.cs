using AlcoholLimit.Utilities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SQLite.SQLite3;

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
            if(CreateTableResult.Created == (await database.CreateTableAsync<T>()))
            {
                AddDefaultItems();
            };
        }

        protected virtual void AddDefaultItems() { }

        #region Public methods

        public virtual async Task<List<T>> GetItemsAsync()
        {
            await Init();
            return await database.Table<T>().ToListAsync();
        }

        public abstract Task<T> GetItemAsync(int id);

        public abstract Task<T> GetItemAsync(string name);

        public abstract Task<int> SaveItemAsync(T item);

        public virtual async Task<int> DeleteItemAsync(T item)
        {
            await Init();
            return await database.DeleteAsync(item);
        }

        public virtual async Task<int> DeleteAllItemAsync()
        {
            await Init();
            var result = await database.DeleteAllAsync<T>();
            AddDefaultItems();
            return result;
        }

        #endregion
    }
}
