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
        public int Weight { get; set; }
        public sex Sex { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public bool nightmode { get; set; }
    }
}
