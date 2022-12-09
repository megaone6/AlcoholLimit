using AlcoholLimit.Utilities;

namespace AlcoholLimit.Data
{
    public class ConsumedDrinkDatabase : Database<ConsumedDrinkItem>
    {
        #region Public methods

        public override async Task<ConsumedDrinkItem> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<ConsumedDrinkItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public override async Task<ConsumedDrinkItem> GetItemAsync(string name)
        {
            return await database.Table<ConsumedDrinkItem>().Where(i => i.ID == 0).FirstOrDefaultAsync();
        }

        public override async Task<int> SaveItemAsync(ConsumedDrinkItem item)
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
