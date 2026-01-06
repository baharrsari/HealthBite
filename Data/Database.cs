using HealthBite.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBite.Data
{
    public static class Database
    {
        private static SQLiteAsyncConnection db;
        private static List<DietModel> diets = new();

        public static void Init()
        {
            if (db != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "HealthBite.db3");

            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<UserModel>().Wait();
            db.CreateTableAsync<DietProgressModel>().Wait();
            db.CreateTableAsync<WaterIntakeModel>().Wait();
            db.CreateTableAsync<WeightEntryModel>().Wait();
            db.CreateTableAsync<TrainingProgressModel>().Wait(); // Antrenman ilerleme tablosu
            
            diets = DietData.GetPredefinedDiets();
        }

        #region User Operations
        public static async Task<int> AddUser(UserModel user) { Init(); return await db.InsertAsync(user); }
        public static async Task<bool> IDExists(string id) { Init(); var count = await db.Table<UserModel>().Where(u => u.IDNumber == id).CountAsync(); return count > 0; }
        public static async Task<UserModel> GetUserById(string id) { Init(); return await db.Table<UserModel>().Where(u => u.IDNumber == id).FirstOrDefaultAsync(); }
        public static async Task<int> UpdateUser(UserModel user) { Init(); return await db.UpdateAsync(user); }
        #endregion

        #region Diet Operations
        public static List<DietModel> GetAllDiets() { Init(); return diets; }
        #endregion

        #region WeightEntry Operations
        public static async Task AddWeightEntry(WeightEntryModel entry)
        {
            Init();
            var startDate = entry.DateRecorded.Date;
            var endDate = startDate.AddDays(1);
            var existingEntry = await db.Table<WeightEntryModel>().Where(w => w.UserId == entry.UserId && w.DateRecorded >= startDate && w.DateRecorded < endDate).FirstOrDefaultAsync();

            if (existingEntry != null)
            {
                existingEntry.WeightInKg = entry.WeightInKg;
                await db.UpdateAsync(existingEntry);
            }
            else
            {
                await db.InsertAsync(entry);
            }

            var user = await db.Table<UserModel>().Where(u => u.Id == entry.UserId).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Weight = entry.WeightInKg;
                await UpdateUser(user);
            }
        }

        public static async Task<List<WeightEntryModel>> GetWeightEntriesForUser(int userId)
        {
            Init();
            return await db.Table<WeightEntryModel>().Where(w => w.UserId == userId).OrderByDescending(w => w.DateRecorded).ToListAsync();
        }
        #endregion

        #region DietProgress Operations
        public static async Task<DietProgressModel> GetOrCreateMealProgress(int userId, int dietId, DateTime date)
        {
            Init();
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);
            var record = await db.Table<DietProgressModel>().Where(p => p.UserId == userId && p.DietId == dietId && p.Date >= startDate && p.Date < endDate).FirstOrDefaultAsync();

            if (record == null)
            {
                record = new DietProgressModel
                {
                    UserId = userId,
                    DietId = dietId,
                    Date = date.Date,
                    Breakfast = false,
                    Lunch = false,
                    Dinner = false
                };
                await db.InsertAsync(record);
            }
            return record;
        }

        public static async Task UpdateMealProgress(DietProgressModel progress)
        {
            Init();
            await db.UpdateAsync(progress);
        }

        public static async Task<int> GetDailyMealProgress(int userId, int dietId, DateTime date)
        {
            Init();
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);
            var record = await db.Table<DietProgressModel>().Where(p => p.UserId == userId && p.DietId == dietId && p.Date >= startDate && p.Date < endDate).FirstOrDefaultAsync();

            if (record == null) return 0;

            int count = 0;
            if (record.Breakfast) count++;
            if (record.Lunch) count++;
            if (record.Dinner) count++;
            return (int)Math.Round(count * 100.0 / 3.0);
        }
        #endregion
        
        #region WaterIntake Operations
        public static async Task AddWaterIntake(int userId, DateTime date, double amountMl)
        {
            Init();
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);
            var existingRecord = await db.Table<WaterIntakeModel>().Where(w => w.UserId == userId && w.Date >= startDate && w.Date < endDate).FirstOrDefaultAsync();

            if (existingRecord != null)
            {
                existingRecord.AmountMl += amountMl;
                await db.UpdateAsync(existingRecord);
            }
            else
            {
                await db.InsertAsync(new WaterIntakeModel { UserId = userId, Date = date.Date, AmountMl = amountMl });
            }
        }

        public static async Task<double> GetDailyWaterIntake(int userId, DateTime date)
        {
            Init();
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            var records = await db.Table<WaterIntakeModel>()
                                  .Where(w => w.UserId == userId && w.Date >= startDate && w.Date < endDate)
                                  .ToListAsync();
            return records.Sum(w => w.AmountMl);
        }
        #endregion
        
        #region TrainingProgress Operations
        public static async Task MarkTrainingDay(int userId, DateTime date, bool completed)
        {
            Init();
            var record = await db.Table<TrainingProgressModel>().Where(p => p.UserId == userId && p.Date == date.Date).FirstOrDefaultAsync();
            if (record != null)
            {
                record.IsCompleted = completed;
                await db.UpdateAsync(record);
            }
            else
            {
                await db.InsertAsync(new TrainingProgressModel { UserId = userId, Date = date.Date, IsCompleted = completed });
            }
        }

        public static async Task<TrainingProgressModel> GetTrainingProgressForDay(int userId, DateTime date)
        {
            Init();
            return await db.Table<TrainingProgressModel>().Where(p => p.UserId == userId && p.Date == date.Date).FirstOrDefaultAsync();
        }

        public static async Task<int> GetTotalCompletedWorkouts(int userId, DateTime startDate)
        {
            Init();
            return await db.Table<TrainingProgressModel>().Where(p => p.UserId == userId && p.Date >= startDate.Date && p.IsCompleted).CountAsync();
        }
        #endregion
    }
}