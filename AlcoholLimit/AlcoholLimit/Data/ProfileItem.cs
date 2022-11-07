using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoholLimit.Data
{
    public enum sex { WOMAN, MAN };
    public class ProfileItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; } //Size of the drink in milliliter (ml)
        public sex Sex { get; set; }
        public bool nightmode { get; set; }
    }
}
