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
        public int AlcoholPercent { get; set; }
        public int Cost { get; set; }
        public int Calories { get; set; }
        public int StandardDrink => Size * AlcoholPercent / 100; //TODO: Calculate StandardDrink properly
    }
}
