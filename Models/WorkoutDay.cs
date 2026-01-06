using System.Collections.Generic;

namespace HealthBite.Models
{
    // Bir antrenman gününün programını içerir
    public class WorkoutDay
    {
        public string DayName { get; set; } // O günün adı (örn: "Gün 1: Göğüs & Triceps")
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}