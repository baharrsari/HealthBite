using SQLite;
using System;

namespace HealthBite.Models
{
    public class TrainingProgressModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
    }
}