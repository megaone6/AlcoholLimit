using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AlcoholLimit.DTOs
{
    /*
     * Data Transfer Object for Drink Items during inputs.
     * */
    public class DrinkItemInputDTO
    {
        [Required]
        [StringLength(50, ErrorMessage ="Name is too long (character limit: 50)")]
        public string Name { get; set; }
        [Required]
        [Range(1, 10000, ErrorMessage = "Invalid size! (range: 1-10000)")]
        public int Size { get; set; } //Size of the drink in milliliter (ml)
        [Required]
        [Range(0, 100, ErrorMessage = "Invalid percetange! (range: 0-100)")]
        public double AlcoholPercent { get; set; }
        [Required]
        [Range(0, 1000000, ErrorMessage = "Invalid cost! (range: 0-1000000)")]
        public int Cost { get; set; } //In HUF
        [Required]
        [Range(0, 1000000, ErrorMessage = "Invalid calories! (range: 0-1000000)")]
        public int Calories { get; set; } //In kCal
    }
}
