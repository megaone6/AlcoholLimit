using AlcoholLimit.Utilities;

namespace AlcoholLimit.Data
{
    public class ProfileDatabase : Database<ProfileItem>
    {
        #region Public methods
        public override async Task<ProfileItem> GetItemAsync(int id)
        {
            await Init();
            return await database.Table<ProfileItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public override async Task<ProfileItem> GetItemAsync(string name)
        {
            await Init();
            return await database.Table<ProfileItem>().Where(i => i.Name == name).FirstOrDefaultAsync();
        }

        public override async Task<int> SaveItemAsync(ProfileItem item)
        {
            await Init();
            if (item.ID != -1)
                return await database.UpdateAsync(item);
            else
                return await database.InsertAsync(item);
        }

        #endregion
    }
}
