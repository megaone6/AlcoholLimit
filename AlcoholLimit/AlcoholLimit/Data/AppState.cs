using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoholLimit.Data
{
    public class AppState
    {
        public ProfileItem profile;

        public bool isDrinking = false;
        public bool hideComponents { 
            get
            {
                return !isDrinking;
            }
        }
        public List<DrinkItem> consumedDrinks = new List<DrinkItem>();
        public DateTime startTime = DateTime.Now;

        public AppState()
        {
            ProfileItem p = new ProfileItem();
            p.ID = -1;
            profile = p;
        }

        public event Action OnChange;

        public void SetProfile()
        {
            ProfileItem p = new ProfileItem();
            p.ID = -1;
            this.profile = p;
            NotifyStateChanged();
        }
        public void SetProfile(ProfileItem p)
        {
            this.profile = p;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
