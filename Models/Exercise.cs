namespace HealthBite.Models
{
    // Tek bir egzersizi tanımlar
    public class Exercise
    {
        public string Name { get; set; }        // Egzersiz adı (örn: "Push-up")
        public string Sets { get; set; }        // Set sayısı (örn: "3")
        public string Reps { get; set; }        // Tekrar sayısı (örn: "10-12")
        public string RestPeriod { get; set; }  // Dinlenme süresi (örn: "60 saniye")
    }
}