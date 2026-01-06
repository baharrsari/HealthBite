using HealthBite.Data;
using HealthBite.Models;
using HealthBite.Services;
using HealthBite.Drawable;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBite.Views
{
    public class DashboardPage : ContentPage
    {
        // UI Elemanları
        Label welcomeLabel, bmiLabel, bmiCategoryLabel, waterPercentageLabel;
        Label dietProgressLabel, trainingProgressLabel;
        Label activeDietLabel, activeTrainingLabel;
        Label dietMessageLabel, trainingMessageLabel;
        Label waterProgressLabel;

        ProgressBar waterProgressBar;
        CircularProgressDrawable dietCircularProgress, trainingCircularProgress;
        GraphicsView dietCircleView, trainingCircleView;
        Frame dietProgressFrame, trainingProgressFrame, waterTileFrame;
        
        Grid dietProgressGrid, trainingProgressGrid;

        // Tema Renkleri
        private readonly Color PrimaryColor = Color.FromRgb(25, 54, 48);
        private readonly Color AccentColorPink = Color.FromRgb(228, 178, 179);
        private readonly Color AccentColorYellow = Color.FromHex("#FFC107");
        private readonly Color PageBackgroundColor = Color.FromRgb(246, 247, 249);
        private readonly Color TextColorDark = Color.FromHex("#343A40");
        private readonly Color ShadowColor = Color.FromHex("#E0E0E0");

        public DashboardPage()
        {
            // Navigasyon çubuğunun rengini sayfa ile uyumlu hale getirir.
            Shell.SetBackgroundColor(this, Color.FromRgb(246, 247, 249));
            // Geri oku ve başlık gibi öğelerin rengini ayarlar.
            Shell.SetForegroundColor(this, Color.FromHex("#343A40"));

            Title = "";
            BackgroundColor = PageBackgroundColor;

            var mainLayout = new VerticalStackLayout { Padding = 20, Spacing = 20 };

            welcomeLabel = new Label { FontSize = 24, FontAttributes = FontAttributes.Bold, TextColor = PrimaryColor };
            mainLayout.Children.Add(welcomeLabel);

            var kpiGrid = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
                RowDefinitions = { new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Auto) },
                ColumnSpacing = 15,
                RowSpacing = 15
            };

            var (bmiTile, dietProgressTile) = CreateLeftColumnTiles();
            var (waterTile, trainingProgressTile) = CreateRightColumnTiles();
            waterTileFrame = waterTile;

            AddTapGesture(dietProgressFrame, $"//{nameof(DailyMealCheckPage)}");
            AddTapGesture(waterTileFrame, nameof(WaterIntakeDetailPage));

            var trainingTapGesture = new TapGestureRecognizer();
            trainingTapGesture.Tapped += OnTrainingProgressTapped;
            trainingProgressFrame.GestureRecognizers.Add(trainingTapGesture);

            kpiGrid.Add(bmiTile, 0, 0);
            kpiGrid.Add(dietProgressTile, 0, 1);
            kpiGrid.Add(waterTileFrame, 1, 0);
            kpiGrid.Add(trainingProgressTile, 1, 1);
            mainLayout.Children.Add(kpiGrid);

            var actionsLayout = new VerticalStackLayout { Spacing = 15, Margin = new Thickness(0, 10, 0, 0) };
            actionsLayout.Children.Add(CreateActionView("Aktif Diyet", "Diyet seçilmedi", "Diyetleri Görüntüle", AccentColorPink, $"//{nameof(DietProgramsPage)}", out activeDietLabel));
            actionsLayout.Children.Add(CreateActionView("Aktif Antrenman", "Antrenman seçilmedi", "Antrenmanları Görüntüle", AccentColorYellow, $"//{nameof(TrainingProgramsPage)}", out activeTrainingLabel));
            mainLayout.Children.Add(actionsLayout);

            Content = new ScrollView { Content = mainLayout };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadAllData();
        }

        private void LoadAllData()
        {
            var user = SessionManager.GetCurrentUser();
            if (user == null)
            {
                SetLoggedOutState();
                return;
            }

            welcomeLabel.Text = $"Merhaba, {user.Name}!";
            UpdateBmi(user);

            Task.Run(async () =>
            {
                await UpdateWaterIntakeDisplay(user);
                await UpdateDietProgress(user);
                await UpdateTrainingProgress(user);
            });
        }

        private void SetLoggedOutState()
        {
            welcomeLabel.Text = "HealthBite'a Hoş Geldiniz";
            bmiLabel.Text = "-";
            bmiCategoryLabel.Text = "Lütfen giriş yapın";
            if(waterPercentageLabel != null) waterPercentageLabel.Text = "0%";
            if(waterProgressBar != null) waterProgressBar.Progress = 0;
            if(waterProgressLabel != null) waterProgressLabel.Text = "0/2000 ml tamamlandı";
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateDietProgress(null); 
                UpdateTrainingProgress(null);
                if(activeDietLabel != null) activeDietLabel.Text = "Giriş yapmalısınız";
                if(activeTrainingLabel != null) activeTrainingLabel.Text = "Giriş yapmalısınız";
            });
        }

        private void UpdateBmi(UserModel user)
        {
            double bmi = user.CalculateBmi();
            bmiLabel.Text = bmi.ToString("F2");
            bmiCategoryLabel.Text = GetBmiCategory(bmi);
        }

        private async Task UpdateWaterIntakeDisplay(UserModel user)
        {
            if (user == null) return;
            double totalWater = await Database.GetDailyWaterIntake(user.Id, DateTime.Today);
            double goal = Preferences.Get("water_goal_ml", 2000);
            double progress = (goal > 0) ? Math.Clamp(totalWater / goal, 0.0, 1.0) : 0.0;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                waterProgressBar.Progress = progress;
                waterPercentageLabel.Text = progress.ToString("P0");
                waterProgressLabel.Text = $"{totalWater:F0}/{goal:F0} ml tamamlandı";
            });
        }
        
        private async Task UpdateDietProgress(UserModel user)
        {
            var dbUser = user != null ? await Database.GetUserById(user.IDNumber) : null;

            if (dbUser != null && dbUser.SelectedDietId.HasValue)
            {
                var diet = Database.GetAllDiets().FirstOrDefault(d => d.Id == dbUser.SelectedDietId.Value);
                if (diet != null)
                {
                    SessionManager.SetCurrentDiet(diet);
                    var percent = await Database.GetDailyMealProgress(dbUser.Id, diet.Id, DateTime.Today);
                    
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        dietProgressGrid.IsVisible = true;
                        dietMessageLabel.IsVisible = false;
                        activeDietLabel.Text = diet.Title;
                        dietCircularProgress.Progress = percent / 100f;
                        dietProgressLabel.Text = $"{percent}%";
                        dietCircleView?.Invalidate();
                    });
                    return;
                }
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                dietProgressGrid.IsVisible = false;
                dietMessageLabel.IsVisible = true;
                dietMessageLabel.Text = "Henüz bir diyet seçmediniz.";
                activeDietLabel.Text = "Diyet seçilmedi";
            });
        }

        private async Task UpdateTrainingProgress(UserModel user)
        {
            var dbUser = user != null ? await Database.GetUserById(user.IDNumber) : null;

            if (dbUser != null && dbUser.SelectedTrainingId.HasValue)
            {
                var trainingProgram = TrainingData.GetPredefinedTrainingPrograms().FirstOrDefault(p => p.Id == dbUser.SelectedTrainingId.Value);
                if (trainingProgram != null)
                {
                    SessionManager.SetCurrentTrainingProgram(trainingProgram);
                    
                    MainThread.BeginInvokeOnMainThread(() => activeTrainingLabel.Text = trainingProgram.Title);
                    
                    var startDate = Preferences.Get($"TrainingProgramStartDate_{dbUser.Id}", DateTime.MinValue);
                    if (startDate == DateTime.MinValue)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            trainingProgressGrid.IsVisible = false;
                            trainingMessageLabel.IsVisible = true;
                            trainingMessageLabel.Text = "Programı başlatmak için dokunun.";
                        });
                        return;
                    }
                    
                    int daysIntoProgram = (DateTime.Today - startDate.Date).Days;
                    int dayOfWeekIndex = daysIntoProgram % 7;
                    WorkoutDay todayWorkout = (dayOfWeekIndex < trainingProgram.WeeklySchedule.Count) ? trainingProgram.WeeklySchedule[dayOfWeekIndex] : null;
                    bool isRestDay = todayWorkout == null || !todayWorkout.Exercises.Any();

                    if (isRestDay)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            trainingProgressGrid.IsVisible = false;
                            trainingMessageLabel.IsVisible = true;
                            trainingMessageLabel.Text = "Bugün dinlenme günü.";
                        });
                    }
                    else
                    {
                        var progress = await Database.GetTrainingProgressForDay(dbUser.Id, DateTime.Today);
                        bool isCompletedToday = progress?.IsCompleted ?? false;
                        float progressValue = isCompletedToday ? 1.0f : 0f;
                        string progressText = isCompletedToday ? "100%" : "0%";

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            trainingMessageLabel.IsVisible = false;
                            trainingProgressGrid.IsVisible = true;
                            trainingCircularProgress.Progress = progressValue;
                            trainingProgressLabel.Text = progressText;
                            trainingCircleView?.Invalidate();
                        });
                    }
                    return;
                }
            }
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                trainingProgressGrid.IsVisible = false;
                trainingMessageLabel.IsVisible = true;
                trainingMessageLabel.Text = "Henüz bir antrenman seçmediniz.";
                activeTrainingLabel.Text = "Antrenman seçilmedi";
            });
        }

        private (Frame, Frame) CreateLeftColumnTiles()
        {
            bmiLabel = new Label { FontSize = 32, FontAttributes = FontAttributes.Bold, TextColor = PrimaryColor, HorizontalOptions = LayoutOptions.Center };
            bmiCategoryLabel = new Label { FontSize = 16, TextColor = TextColorDark, HorizontalOptions = LayoutOptions.Center };
            var bmiLayout = new VerticalStackLayout { Spacing = 5, Children = { new Label { Text = "Vücut Kitle İndeksi", FontAttributes = FontAttributes.Bold, TextColor = TextColorDark }, new BoxView { HeightRequest = 1, Color = ShadowColor }, bmiLabel, bmiCategoryLabel } };
            var bmiTile = new KpiTileFrame { Content = bmiLayout, HeightRequest = 180 };

            dietCircularProgress = new CircularProgressDrawable { PrimaryColor = AccentColorPink, SecondaryColor = ShadowColor };
            // --- DEĞİŞİKLİK BURADA ---
            dietCircleView = new GraphicsView { Drawable = dietCircularProgress, HeightRequest = 110, WidthRequest = 110, HorizontalOptions = LayoutOptions.Center };
            
            dietProgressLabel = new Label { 
                FontSize = 18, 
                FontAttributes = FontAttributes.Bold, 
                HorizontalTextAlignment = TextAlignment.Center, 
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            
            dietProgressGrid = new Grid { IsVisible = false, Children = { dietCircleView, dietProgressLabel } };
            
            dietMessageLabel = new Label { FontSize = 16, TextColor = TextColorDark, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, Padding = new Thickness(10) };
            
            var dietContentGrid = new Grid { Children = { dietProgressGrid, dietMessageLabel } };
            var dietProgressLayout = new VerticalStackLayout { Spacing = 5, Children = { new Label { Text = "Günlük Öğün İlerlemesi", FontAttributes = FontAttributes.Bold, TextColor = TextColorDark }, new BoxView { HeightRequest = 1, Color = ShadowColor }, dietContentGrid } };
            dietProgressFrame = new KpiTileFrame { Content = dietProgressLayout, HeightRequest = 180 };

            return (bmiTile, dietProgressFrame);
        }

        private (Frame, Frame) CreateRightColumnTiles()
        {
            waterProgressBar = new ProgressBar { ProgressColor = Color.FromHex("#2196F3") };
            waterPercentageLabel = new Label { FontSize = 18, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.End };
            
            waterProgressLabel = new Label { FontSize = 12, TextColor = TextColorDark, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };

            var waterDetailsGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                }
            };
            waterDetailsGrid.Add(waterProgressLabel, 0, 0);
            waterDetailsGrid.Add(waterPercentageLabel, 1, 0);

            var waterLayout = new VerticalStackLayout { Spacing = 10, Children = { new Label { Text = "Günlük Su Tüketimi", FontAttributes = FontAttributes.Bold, TextColor = TextColorDark }, new BoxView { HeightRequest = 1, Color = ShadowColor }, waterProgressBar, waterDetailsGrid } };
            var waterTile = new KpiTileFrame { Content = waterLayout, HeightRequest = 180 };

            trainingCircularProgress = new CircularProgressDrawable { PrimaryColor = AccentColorYellow, SecondaryColor = ShadowColor };
            // --- DEĞİŞİKLİK BURADA ---
            trainingCircleView = new GraphicsView { Drawable = trainingCircularProgress, HeightRequest = 110, WidthRequest = 110, HorizontalOptions = LayoutOptions.Center };
            
            trainingProgressLabel = new Label { 
                FontSize = 18, 
                FontAttributes = FontAttributes.Bold, 
                HorizontalTextAlignment = TextAlignment.Center, 
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            
            trainingProgressGrid = new Grid { IsVisible = false, Children = { trainingCircleView, trainingProgressLabel } };
            
            trainingMessageLabel = new Label { FontSize = 16, TextColor = TextColorDark, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, Padding = new Thickness(10) };
            
            var trainingContentGrid = new Grid { Children = { trainingProgressGrid, trainingMessageLabel } };
            var trainingProgressLayout = new VerticalStackLayout { Spacing = 5, Children = { new Label { Text = "Günlük Antrenman Durumu", FontAttributes = FontAttributes.Bold, TextColor = TextColorDark }, new BoxView { HeightRequest = 1, Color = ShadowColor }, trainingContentGrid } };
            trainingProgressFrame = new KpiTileFrame { Content = trainingProgressLayout, HeightRequest = 180 };

            return (waterTile, trainingProgressFrame);
        }

        private void AddTapGesture(Frame frame, string route)
        {
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                if (SessionManager.IsLoggedIn) {
                    if (route == $"//{nameof(DailyMealCheckPage)}" && SessionManager.GetCurrentUser()?.SelectedDietId == null)
                    {
                        await Shell.Current.GoToAsync($"//{nameof(DietProgramsPage)}");
                    }
                    else
                    {
                       await Shell.Current.GoToAsync(route);
                    }
                }
                else { await DisplayAlert("Giriş Gerekli", "Bu özelliği kullanmak için lütfen giriş yapın.", "Tamam"); }
            };
            frame.GestureRecognizers.Add(tapGesture);
        }

        private async void OnTrainingProgressTapped(object sender, TappedEventArgs e)
        {
            if (!SessionManager.IsLoggedIn) { await DisplayAlert("Giriş Gerekli", "Bu özelliği kullanmak için lütfen giriş yapın.", "Tamam"); return; }
            
            var user = SessionManager.GetCurrentUser();
            if (user == null || !user.SelectedTrainingId.HasValue) 
            {
                 await Shell.Current.GoToAsync($"//{nameof(TrainingProgramsPage)}");
                 return;
            }

            var trainingProgram = SessionManager.GetCurrentTrainingProgram();
            var startDate = Preferences.Get($"TrainingProgramStartDate_{user.Id}", DateTime.MinValue);
            if (startDate == DateTime.MinValue)
            {
                bool start = await DisplayAlert("Programı Başlat", "Bu antrenman programı henüz başlamamış. Şimdi başlatmak ister misiniz?", "Evet, Başlat", "İptal");
                if(start)
                {
                    Preferences.Set($"TrainingProgramStartDate_{user.Id}", DateTime.Today);
                    LoadAllData();
                }
                return;
            }

            int daysIntoProgram = (DateTime.Today - startDate.Date).Days;
            int dayOfWeekIndex = daysIntoProgram % 7;
            WorkoutDay todayWorkout = (dayOfWeekIndex < trainingProgram.WeeklySchedule.Count) ? trainingProgram.WeeklySchedule[dayOfWeekIndex] : null;

            if (todayWorkout != null && todayWorkout.Exercises != null && todayWorkout.Exercises.Any())
            {
                await Navigation.PushAsync(new WorkoutOfTheDayPage(todayWorkout));
            }
            else
            {
                await DisplayAlert("Dinlenme Günü", "Bugün antrenman planınızda dinlenme günü olarak görünüyor. İyi dinlenmeler!", "Harika!");
            }
        }

        private Frame CreateActionView(string title, string defaultText, string buttonText, Color buttonColor, string route, out Label infoLabel)
        {
            infoLabel = new Label { Text = defaultText, TextColor = TextColorDark, FontSize = 16, FontAttributes = FontAttributes.Italic };
            var button = new Button { Text = buttonText, BackgroundColor = buttonColor, TextColor = Color.FromRgb(25, 54, 48), FontAttributes = FontAttributes.Bold, CornerRadius = 15 };
            button.Clicked += async (s, e) => await Shell.Current.GoToAsync(route);
            return new KpiTileFrame { Content = new VerticalStackLayout { Spacing = 8, Children = { new Label { Text = title, FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = TextColorDark }, infoLabel, button } } };
        }

        private string GetBmiCategory(double bmi) { if (bmi <= 0) return "Bilinmiyor"; if (bmi < 18.5) return "Zayıf"; if (bmi < 24.9) return "Normal Kilolu"; if (bmi < 29.9) return "Fazla Kilolu"; if (bmi < 34.9) return "Obez (Sınıf I)"; if (bmi < 39.9) return "Obez (Sınıf II)"; return "Aşırı Obez (Sınıf III)"; }
    }
}