using HealthBite.Models;
using System.Collections.Generic;

namespace HealthBite.Data
{
    public static class TrainingData
    {
        public static List<TrainingProgramModel> GetPredefinedTrainingPrograms()
        {
            return new List<TrainingProgramModel>
            {
                // --- PROGRAM 1: SPOR SALONU - BAŞLANGIÇ ---
                new TrainingProgramModel
                {
                    Id = 1,
                    Title = "Yeni Başlayanlar İçin Tüm Vücut",
                    Location = "Spor Salonunda",
                    Category = "Başlangıç",
                    Description = "Spora yeni başlayanlar için temel kuvvet ve dayanıklılık kazandırmayı amaçlayan 4 haftalık program.",
                    DurationInWeeks = 4,
                    WeeklySchedule = new List<WorkoutDay>
                    {
                        new WorkoutDay { DayName = "1. Gün: Tüm Vücut A", Exercises = new List<Exercise>
                        {
                            new Exercise { Name = "Squat", Sets = "3", Reps = "10-12", RestPeriod = "90 sn" },
                            new Exercise { Name = "Push-up (Dizde veya Normal)", Sets = "3", Reps = "Maksimum", RestPeriod = "60 sn" },
                            new Exercise { Name = "Dumbbell Row", Sets = "3", Reps = "10-12 (her kol)", RestPeriod = "60 sn" },
                            new Exercise { Name = "Plank", Sets = "3", Reps = "30-60 sn", RestPeriod = "45 sn" },
                        }},
                        new WorkoutDay { DayName = "2. Gün: Dinlenme veya Aktif Toparlanma" },
                        new WorkoutDay { DayName = "3. Gün: Tüm Vücut B", Exercises = new List<Exercise>
                        {
                            new Exercise { Name = "Lunge", Sets = "3", Reps = "10-12 (her bacak)", RestPeriod = "90 sn" },
                            new Exercise { Name = "Overhead Press (Dumbbell)", Sets = "3", Reps = "10-12", RestPeriod = "60 sn" },
                            new Exercise { Name = "Lat Pulldown (Makinede)", Sets = "3", Reps = "10-12", RestPeriod = "60 sn" },
                            new Exercise { Name = "Leg Raises", Sets = "3", Reps = "15-20", RestPeriod = "45 sn" },
                        }},
                        new WorkoutDay { DayName = "4. Gün: Dinlenme" },
                        new WorkoutDay { DayName = "5. Gün: Tüm Vücut A (Tekrar)" },
                        new WorkoutDay { DayName = "6. Gün: Dinlenme" },
                        new WorkoutDay { DayName = "7. Gün: Dinlenme" },
                    }
                },
                // --- PROGRAM 2: SPOR SALONU - GÜÇ ---
                new TrainingProgramModel
                {
                    Id = 2,
                    Title = "Orta Seviye Güç Programı",
                    Location = "Spor Salonunda",
                    Category = "Güç",
                    Description = "Kas kütlesini ve gücü artırmaya yönelik, itme/çekme/bacak günlerine bölünmüş 4 haftalık program.",
                    DurationInWeeks = 4,
                    WeeklySchedule = new List<WorkoutDay>
                    {
                        new WorkoutDay { DayName = "1. Gün: İtme (Göğüs, Omuz, Triceps)", Exercises = new List<Exercise>
                        {
                            new Exercise { Name = "Bench Press", Sets = "4", Reps = "6-8", RestPeriod = "120 sn" },
                            new Exercise { Name = "Incline Dumbbell Press", Sets = "3", Reps = "8-10", RestPeriod = "90 sn" },
                            new Exercise { Name = "Lateral Raises", Sets = "3", Reps = "12-15", RestPeriod = "60 sn" },
                            new Exercise { Name = "Triceps Pushdown", Sets = "3", Reps = "10-12", RestPeriod = "60 sn" },
                        }},
                        new WorkoutDay { DayName = "2. Gün: Çekme (Sırt, Biceps)", Exercises = new List<Exercise>
                        {
                            new Exercise { Name = "Pull-up (Yardımlı veya Normal)", Sets = "4", Reps = "Maksimum", RestPeriod = "120 sn" },
                            new Exercise { Name = "Barbell Row", Sets = "3", Reps = "8-10", RestPeriod = "90 sn" },
                            new Exercise { Name = "Face Pulls", Sets = "3", Reps = "15-20", RestPeriod = "60 sn" },
                            new Exercise { Name = "Barbell Curl", Sets = "3", Reps = "10-12", RestPeriod = "60 sn" },
                        }},
                        new WorkoutDay { DayName = "3. Gün: Dinlenme" },
                        new WorkoutDay { DayName = "4. Gün: Bacak", Exercises = new List<Exercise>
                        {
                            new Exercise { Name = "Barbell Squat", Sets = "4", Reps = "6-8", RestPeriod = "120 sn" },
                            new Exercise { Name = "Romanian Deadlift", Sets = "3", Reps = "8-10", RestPeriod = "90 sn" },
                            new Exercise { Name = "Leg Press", Sets = "3", Reps = "10-12", RestPeriod = "90 sn" },
                            new Exercise { Name = "Calf Raises", Sets = "4", Reps = "15-20", RestPeriod = "45 sn" },
                        }},
                        new WorkoutDay { DayName = "5. Gün: Dinlenme" },
                        new WorkoutDay { DayName = "6. Gün: Opsiyonel (Tüm Vücut Hafif)" },
                        new WorkoutDay { DayName = "7. Gün: Dinlenme" },
                    }
                },
                // --- PROGRAM 3: EVDE - BAŞLANGIÇ ---
                new TrainingProgramModel
                {
                    Id = 3,
                    Title = "Evde Ekipmansız Vücut Ağırlığı",
                    Location = "Evde",
                    Category = "Başlangıç",
                    Description = "Hiçbir ekipman gerektirmeyen, evde rahatlıkla uygulanabilecek temel bir başlangıç programı.",
                    DurationInWeeks = 4,
                    WeeklySchedule = new List<WorkoutDay>
                    {
                        new WorkoutDay { DayName = "1. Gün: Alt Vücut & Karın", Exercises = new List<Exercise> {
                            new Exercise { Name = "Bodyweight Squat", Sets = "3", Reps = "15-20", RestPeriod = "60 sn" },
                            new Exercise { Name = "Glute Bridge", Sets = "3", Reps = "20", RestPeriod = "45 sn" },
                            new Exercise { Name = "Plank", Sets = "3", Reps = "Maksimum Süre", RestPeriod = "60 sn" },
                        }},
                        new WorkoutDay { DayName = "2. Gün: Dinlenme" },
                        new WorkoutDay { DayName = "3. Gün: Üst Vücut & Kardiyo", Exercises = new List<Exercise> {
                            new Exercise { Name = "Push-up (Dizde)", Sets = "3", Reps = "Maksimum", RestPeriod = "60 sn" },
                            new Exercise { Name = "Jumping Jacks", Sets = "3", Reps = "60 saniye", RestPeriod = "30 sn" },
                            new Exercise { Name = "Mountain Climbers", Sets = "3", Reps = "45 saniye", RestPeriod = "45 sn" },
                        }},
                        new WorkoutDay { DayName = "4. Gün: Dinlenme" },
                        new WorkoutDay { DayName = "5. Gün: Tüm Vücut (Karma)", Exercises = new List<Exercise> {
                            new Exercise { Name = "Lunge", Sets = "3", Reps = "12 (her bacak)", RestPeriod = "60 sn" },
                            new Exercise { Name = "Pike Push-up", Sets = "3", Reps = "Maksimum", RestPeriod = "60 sn" },
                            new Exercise { Name = "High Knees", Sets = "3", Reps = "45 saniye", RestPeriod = "45 sn" },
                        }},
                        new WorkoutDay { DayName = "6. Gün: Dinlenme" },
                        new WorkoutDay { DayName = "7. Gün: Dinlenme" },
                    }
                }
            };
        }
    }
}