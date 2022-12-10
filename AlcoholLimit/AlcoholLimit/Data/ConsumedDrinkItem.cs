using SQLite;

namespace AlcoholLimit.Data
{
    public class ConsumedDrinkItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int DrinkItemID { get; set; } // DrinkItem, which was consumed
        public string Date { get; set; } // Date when it was consumed
    }
}