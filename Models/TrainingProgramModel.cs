using System.Collections.Generic;

namespace HealthBite.Models
{
    public class TrainingProgramModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; } // "Evde", "Spor Salonunda"
        public string Category { get; set; }
        public string Description { get; set; }
        public int DurationInWeeks { get; set; }
        public List<WorkoutDay> WeeklySchedule { get; set; } = new List<WorkoutDay>();
    }
}