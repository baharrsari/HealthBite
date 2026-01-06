using SQLite; // Bu using ifadesini eklemeyi unutmayın

namespace HealthBite.Models
{
    public class WaterIntakeModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // <<-- BU SATIRI EKLEYİN
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public double AmountMl { get; set; } // İçilen su miktarı (mililitre)
    }
}