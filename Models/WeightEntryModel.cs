// Models/WeightEntryModel.cs
namespace HealthBite.Models
{
    public class WeightEntryModel
    {
        // Eğer bu veriyi daha sonra kalıcı bir veritabanına (SQLite gibi)
        // taşımayı düşünürseniz bir Id alanı ekleyebilirsiniz.
        // public int Id { get; set; }

        public int UserId { get; set; } // UserModel'daki Id (int) ile eşleşecek
        public DateTime DateRecorded { get; set; }
        public double WeightInKg { get; set; }
    }
}