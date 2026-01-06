using SQLite;
using System; // DateTime için eklendi

namespace HealthBite.Models
{
    public class UserModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        [Unique]
        public string IDNumber { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        // --- DEĞİŞİKLİKLER ---
        public double Height { get; set; }
        public double Weight { get; set; }
        public int Age { get; set; } // Bu alan, doğum gününden hesaplanarak doldurulacak
        
        // YENİ: Doğum tarihi alanı eklendi
        public DateTime DateOfBirth { get; set; } 
        
        // KALDIRILDI: Kan grubu alanı kaldırıldı
        public string BloodType { get; set; } 
        
        public int? SelectedDietId { get; set; }
        public int? SelectedTrainingId { get; set; }

        public double CalculateBmi()
        {
            if (Height <= 0 || Weight <= 0)
                return 0;

            double heightInMeters = Height / 100.0; // cm'yi metreye çevir
            return Weight / (heightInMeters * heightInMeters);
        }
    }
}
