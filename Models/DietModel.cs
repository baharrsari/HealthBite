// Models/DietModel.cs
using System.Collections.Generic; // Dictionary ve List için

namespace HealthBite.Models
{
    public class DietModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        // public List<string> Foods { get; set; } // Bu genel liste kaldırıldı veya farklı bir amaçla kullanılabilir.
        public string Goal { get; set; }
        public double Calories { get; set; } // Diyetin ana öğünlere göre toplam günlük kalori değeri
        public Dictionary<string, MealPlan> Meals { get; set; } // Anahtar: "Breakfast", "Lunch", "Dinner"
    }
}