using AlcoholLimit.Utilities;

namespace AlcoholLimit.Data
{
    public class DrinkDatabase : Database<DrinkItem>
    {
        #region Public methods

        public async Task<List<DrinkItem>> GetItemsStandardDrinkAsync()
        {
            await Init();
            return await database.Table<DrinkItem>().Where(i => !i.IsDeleted && i.PureAlcGram > 1).ToListAsync();
        }

        public override async Task<List<DrinkItem>> GetItemsAsync()
        {
            await Init();
            return await database.Table<DrinkItem>().Where(i => !i.IsDeleted).ToListAsync();
        }
        
        public async Task<List<DrinkItem>> GetItemsWithDeletedAsync()
        {
            await Init();
            return await database.Table<DrinkItem>().ToListAsync();
        }

        public override async Task<DrinkItem> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<DrinkItem>().Where(i => !i.IsDeleted && i.ID == id).FirstOrDefaultAsync();
        }

        public override async Task<DrinkItem> GetItemAsync(string name)
        {
            await Init();
            return await database.Table<DrinkItem>().Where(i => !i.IsDeleted && i.Name == name).FirstOrDefaultAsync();
        }

        public async Task<List<DrinkItem>> GetItemsFromIdSetAsync(HashSet<int> idSet, bool includeDeleted)
        {
            await Init();
            return await database.Table<DrinkItem>().Where(i => (includeDeleted || !includeDeleted && !i.IsDeleted) && idSet.Contains(i.ID)).ToListAsync();
        }

        public override async Task<int> SaveItemAsync(DrinkItem item)
        {
            await Init();
            if (item.ID != 0)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }
         
        public override async Task<int> DeleteItemAsync(DrinkItem item)
        {
            await Init();
            item.IsDeleted = true;
            return await database.UpdateAsync(item);
        }

        public virtual async Task<int> DeleteAllItemAsync()
        {
            await Init();
            List<DrinkItem> dIs = await database.Table<DrinkItem>().ToListAsync();
            foreach (DrinkItem item in dIs)
            {
                item.IsDeleted = true;
            }
            var result = await database.UpdateAllAsync(dIs);
            AddDefaultItems();
            return result;
        }

        public async Task<List<DrinkItem>> GetDefaultItemsAsync()
        {
            List<DrinkItem> items = new List<DrinkItem>();
            items.Add(
                new DrinkItem
                {
                    Name = "Sörike",
                    Size = 500,
                    AlcoholPercent = 4,
                    Cost = 1200,
                    Calories = 200
                });
            return items;
        }

        #endregion

        protected override void AddDefaultItems()
        {
            DrinkItem item = new DrinkItem
            {
                Name = "Sörike",
                Size = 500,
                AlcoholPercent = 4,
                Cost = 1200,
                Calories = 200
            };

            _ = SaveItemAsync(item);
        }
    }
}
