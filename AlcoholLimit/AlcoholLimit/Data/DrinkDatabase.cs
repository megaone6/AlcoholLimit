using AlcoholLimit.Utilities;

namespace AlcoholLimit.Data
{
    public class DrinkDatabase : Database<DrinkItem>
    {
        #region Public methods

        public async Task<List<DrinkItem>> GetItemsStandardDrinkAsync()
        {
            await Init();
            return await database.Table<DrinkItem>().Where(t => t.StandardDrink > 1).ToListAsync();
        }

        public override async Task<DrinkItem> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<DrinkItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public override async Task<DrinkItem> GetItemAsync(string name)
        {
            await Init();
            return await database.Table<DrinkItem>().Where(i => i.Name == name).FirstOrDefaultAsync();
        }

        public override async Task<int> SaveItemAsync(DrinkItem item)
        {
            await Init();
            if (item.ID != 0)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        #endregion
    }
}
