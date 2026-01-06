// Models/FoodDetailModel.cs
namespace HealthBite.Models
{
    public class FoodDetailModel
    {
        public string Name { get; set; }
        public string Portion { get; set; } // Örn: "100g", "1 adet", "1 kase"
        public double Calories { get; set; }
        public double Protein { get; set; } // gram
        public double Carbs { get; set; }   // gram
        public double Fat { get; set; }     // gram
    }
}