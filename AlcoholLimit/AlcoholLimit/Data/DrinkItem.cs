using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoholLimit.Data
{
    public class DrinkItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Size { get; set; } //Size of the drink in milliliter (ml)
        public double AlcoholPercent { get; set; }
        public int Cost { get; set; } //In HUF
        public int Calories { get; set; } //In kCal
        public double PureAlcGram => Size * (AlcoholPercent / 100) * 0.78945;
    }
}
