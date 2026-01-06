using HealthBite.Models;
using HealthBite.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBite.Services
{
    public static class SessionManager
    {
        private static UserModel _currentUser;
        private static DietModel _currentDiet;
        private static TrainingProgramModel _currentTrainingProgram;

        private static List<DietModel> _diets = new List<DietModel>();

        public static void SetCurrentUser(UserModel user) => _currentUser = user;

        public static UserModel GetCurrentUser() => _currentUser;

        public static void UpdateCurrentUser(UserModel updatedUser) => _currentUser = updatedUser;

        public static bool IsLoggedIn => _currentUser != null;

        public static void SetAllDiets(List<DietModel> diets) => _diets = diets;

        public static List<DietModel> GetAllDiets() => _diets ?? new List<DietModel>();

        public static void SetCurrentDiet(DietModel diet)
        {
            if (diet != null)
            {
                _currentDiet = diet;
                Preferences.Set("CurrentDietTitle", diet.Title);
            }
        }

        public static DietModel GetCurrentDiet()
        {
            if (_currentDiet != null)
                return _currentDiet;

            string savedTitle = Preferences.Get("CurrentDietTitle", null);
            if (string.IsNullOrWhiteSpace(savedTitle))
                return null;
            
            _currentDiet = GetAllDiets().FirstOrDefault(d => d.Title == savedTitle);
            return _currentDiet;
        }

        public static void SetCurrentTrainingProgram(TrainingProgramModel program)
        {
            if (program != null)
            {
                _currentTrainingProgram = program;
                Preferences.Set("CurrentTrainingProgramId", program.Id);
            }
        }

        public static TrainingProgramModel GetCurrentTrainingProgram()
        {
            if (_currentTrainingProgram != null)
                return _currentTrainingProgram;

            int savedId = Preferences.Get("CurrentTrainingProgramId", -1);
            if (savedId == -1)
                return null;

            _currentTrainingProgram = TrainingData.GetPredefinedTrainingPrograms().FirstOrDefault(p => p.Id == savedId);
            return _currentTrainingProgram;
        }

        public static async Task<int> GetMealProgressPercentage()
        {
            var user = GetCurrentUser();
            var diet = GetCurrentDiet();
            if (user == null || diet == null) return 0;

            return await Database.GetDailyMealProgress(user.Id, diet.Id, System.DateTime.Today);
        }

        public static void ClearSession()
        {
            Preferences.Remove("CurrentDietTitle");
            Preferences.Remove("CurrentTrainingProgramId");
            Preferences.Remove("TrainingProgramStartDate"); // <-- EKLENDİ
            _currentUser = null;
            _currentDiet = null;
            _currentTrainingProgram = null;
        }
    }
}