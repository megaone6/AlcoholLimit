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

        public async Task<Dictionary<string, List<ConsumedDrinkItem>>> GetItemsByDateAsync()
        {
            await Init();
            var cDrinkItems = await database.Table<ConsumedDrinkItem>().OrderByDescending<string>(x => x.Date).ToListAsync();
            Dictionary<string, List<ConsumedDrinkItem>> retDict = new();
            string prevKey = "";
            foreach(var cDrinkItem in cDrinkItems) {
                if(prevKey != cDrinkItem.Date)
                {
                    retDict.Add(cDrinkItem.Date, new List<ConsumedDrinkItem> {cDrinkItem});
                    prevKey = cDrinkItem.Date;
                }
                else
                {
                    retDict[cDrinkItem.Date].Add(cDrinkItem);
                }
            }

            return retDict;
        }

        #endregion
    }
}
