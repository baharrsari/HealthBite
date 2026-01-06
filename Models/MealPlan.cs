// Models/MealPlan.cs
using System.Collections.Generic;
using System.Linq; // .Any() için

namespace HealthBite.Models
{
    public class MealPlan
    {
        public List<FoodDetailModel> PrimaryFoodItems { get; set; }
        public List<FoodDetailModel> AlternativeFoodItems { get; set; }
        public bool HasAlternative => AlternativeFoodItems != null && AlternativeFoodItems.Any();

        public MealPlan()
        {
            PrimaryFoodItems = new List<FoodDetailModel>();
            // AlternativeFoodItems null bırakılabilir veya boş başlatılabilir.
            // Kullanım kolaylığı için boş başlatalım:
            AlternativeFoodItems = new List<FoodDetailModel>();
        }
    }
}