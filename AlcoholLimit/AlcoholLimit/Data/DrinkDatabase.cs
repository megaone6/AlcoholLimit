using AlcoholLimit.Utilities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoholLimit.Data
{
    public class DrinkDatabase
    {
        SQLiteAsyncConnection Database;

        #region Contructors

        public DrinkDatabase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<DrinkItem>();
        }

        #endregion

        #region Public methods

        public async Task<List<DrinkItem>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<DrinkItem>().ToListAsync();
        }

        public async Task<List<DrinkItem>> GetItemsStandardDrinkAsync()
        {
            await Init();
            return await Database.Table<DrinkItem>().Where(t => t.StandardDrink > 1).ToListAsync();
        }

        public async Task<DrinkItem> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<DrinkItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<DrinkItem> GetItemAsync(string name)
        {
            await Init();
            return await Database.Table<DrinkItem>().Where(i => i.Name == name).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(DrinkItem item)
        {
            await Init();
            if (item.ID != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(DrinkItem item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }

        public async Task<int> DeleteAllItemAsync()
        {
            await Init();
            return await Database.DeleteAllAsync<DrinkItem>();
        }

        #endregion
    }
}
