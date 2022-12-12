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
            return GenerateDefaultItems();
        }

        #endregion

        protected override void AddDefaultItems()
        {
            foreach (var drinkItem in GenerateDefaultItems())
            {
                _ = SaveItemAsync(drinkItem);
            }
        }

        private List<DrinkItem> GenerateDefaultItems()
        {
            return new List<DrinkItem>() { 
                new DrinkItem
                {
                    Name = "Red Wine",
                    Size = 100,
                    AlcoholPercent = 13,
                    Cost = 1500,
                    Calories = 85
                },
                new DrinkItem
                {
                    Name = "White Wine",
                    Size = 100,
                    AlcoholPercent = 11.5,
                    Cost = 1000,
                    Calories = 80
                },
                new DrinkItem
                {
                    Name = "Lager Beer",
                    Size = 500,
                    AlcoholPercent = 4.5,
                    Cost = 500,
                    Calories = 170
                },
                new DrinkItem
                {
                    Name = "Lager Beer",
                    Size = 330,
                    AlcoholPercent = 4.5,
                    Cost = 420,
                    Calories = 120
                },
                new DrinkItem
                {
                    Name = "IPA Beer",
                    Size = 500,
                    AlcoholPercent = 6,
                    Cost = 800,
                    Calories = 235
                },
                new DrinkItem
                {
                    Name = "IPA Beer",
                    Size = 330,
                    AlcoholPercent = 6,
                    Cost = 630,
                    Calories = 150
                },
                new DrinkItem
                {
                    Name = "Vodka",
                    Size = 50,
                    AlcoholPercent = 40,
                    Cost = 1000,
                    Calories = 105
                },
                new DrinkItem
                {
                    Name = "Rum",
                    Size = 50,
                    AlcoholPercent = 40,
                    Cost = 1200,
                    Calories = 120
                },
                new DrinkItem
                {
                    Name = "Whisky",
                    Size = 50,
                    AlcoholPercent = 40,
                    Cost = 1300,
                    Calories = 125
                }
            };
        }
    }
}
