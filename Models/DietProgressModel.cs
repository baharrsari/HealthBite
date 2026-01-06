using SQLite;
using System;

namespace HealthBite.Models
{
    public class DietProgressModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int DietId { get; set; }
        public DateTime Date { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }

        // Bu metot, bir öğünün tamamlanma durumunu anahtar kelimeye göre döndürür.
        public bool GetMealCompletion(string mealKey)
        {
            return mealKey switch
            {
                "Breakfast" => Breakfast,
                "Lunch" => Lunch,
                "Dinner" => Dinner,
                _ => false, // Geçersiz bir anahtar gelirse varsayılan olarak false
            };
        }
    }
}