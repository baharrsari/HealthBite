using HealthBite.Data;
using HealthBite.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace HealthBite.Views
{
    public class WaterIntakeDetailPage : ContentPage
    {
        private Label waterAmountLabel, waterPercentageLabel, waterMessageLabel;
        private ProgressBar waterProgressBar;
        private Entry waterEntry;

        private double DailyWaterGoalMl => Preferences.Get("water_goal_ml", 2000);
        private readonly Color PrimaryColor = Color.FromRgb(25, 54, 48);
        private readonly Color WaterProgressBarColor = Color.FromHex("#2196F3");
        private readonly Color TextColorDark = Color.FromHex("#343A40");
        private readonly Color TextColorLight = Color.FromHex("#6C757D");

        public WaterIntakeDetailPage()
        {

            // Navigasyon çubuğunun rengini sayfa ile uyumlu hale getirir.
            Shell.SetBackgroundColor(this, Color.FromRgb(246, 247, 249));
            // Geri oku ve başlık gibi öğelerin rengini ayarlar.
            Shell.SetForegroundColor(this, Color.FromHex("#343A40"));

            Title = "";
            BackgroundColor = Color.FromHex("#F8F9FA");

            waterAmountLabel = new Label { FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = TextColorDark, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 0, 10) };
            waterPercentageLabel = new Label { FontSize = 24, FontAttributes = FontAttributes.Bold, TextColor = WaterProgressBarColor, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 0, 15) };
            waterProgressBar = new ProgressBar { ProgressColor = WaterProgressBarColor, BackgroundColor = Color.FromHex("#E0E0E0"), HeightRequest = 10, Margin = new Thickness(0, 0, 0, 10) };
            waterMessageLabel = new Label { Text = "", FontSize = 14, TextColor = TextColorLight, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(0, 5, 0, 15) };
            waterEntry = new Entry { Placeholder = "Miktar (ml)", Keyboard = Keyboard.Numeric, BackgroundColor = Color.FromHex("#F0F2F5"), TextColor = TextColorDark, PlaceholderColor = TextColorLight, Margin = new Thickness(0, 0, 0, 10), HorizontalOptions = LayoutOptions.FillAndExpand };
            
            var addWaterButton = new Button { Text = "Su Ekle", BackgroundColor = PrimaryColor, TextColor = Colors.White, FontAttributes = FontAttributes.Bold, CornerRadius = 10, Padding = new Thickness(15, 12), Margin = new Thickness(0, 0, 0, 0) };
            addWaterButton.Clicked += async (s, e) => { var currentUser = SessionManager.GetCurrentUser(); if (currentUser == null) { await DisplayAlert("Hata", "Lütfen önce giriş yapın.", "Tamam"); return; } if (double.TryParse(waterEntry.Text, out double amount) && amount > 0) { await Database.AddWaterIntake(currentUser.Id, DateTime.Today, amount); waterEntry.Text = string.Empty; await UpdateWaterIntakeDisplay(); } else { await DisplayAlert("Hata", "Lütfen geçerli bir miktar girin.", "Tamam"); } };
            
            var waterInputLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 10, Children = { waterEntry, addWaterButton } };
            var addSmallGlassButton = new Button { Text = "200 ml Ekle", BackgroundColor = WaterProgressBarColor, TextColor = Colors.White, FontAttributes = FontAttributes.Bold, CornerRadius = 10, Padding = new Thickness(15, 12), HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = 14 }; addSmallGlassButton.Clicked += async (s, e) => await AddWaterShortcut(200);
            var addLargeGlassButton = new Button { Text = "350 ml Ekle", BackgroundColor = WaterProgressBarColor, TextColor = Colors.White, FontAttributes = FontAttributes.Bold, CornerRadius = 10, Padding = new Thickness(15, 12), HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = 14 }; addLargeGlassButton.Clicked += async (s, e) => await AddWaterShortcut(350);
            var shortcutButtonsLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 10, Margin = new Thickness(0, 15, 0, 0), Children = { addSmallGlassButton, addLargeGlassButton } };

            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 15,
                    Children = { waterAmountLabel, waterPercentageLabel, waterProgressBar, waterMessageLabel, waterInputLayout, shortcutButtonsLayout }
                }
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await UpdateWaterIntakeDisplay();
        }

        private async Task UpdateWaterIntakeDisplay()
        {
            var user = SessionManager.GetCurrentUser();
            if (user != null)
            {
                double totalWater = await Database.GetDailyWaterIntake(user.Id, DateTime.Today);
                waterAmountLabel.Text = $"Bugün içilen su: {totalWater:F0} ml / {DailyWaterGoalMl:F0} ml";
                double progress = Math.Clamp(totalWater / DailyWaterGoalMl, 0.0, 1.0);
                waterProgressBar.Progress = progress;
                waterPercentageLabel.Text = $"{progress:P0}";
                if (totalWater >= DailyWaterGoalMl)
                {
                    waterPercentageLabel.TextColor = PrimaryColor;
                    waterMessageLabel.Text = "Harikasın! Hedefine ulaştın! ✨";
                    waterMessageLabel.TextColor = PrimaryColor;
                }
                else
                {
                    waterPercentageLabel.TextColor = WaterProgressBarColor;
                    int remainingMl = (int)(DailyWaterGoalMl - totalWater);
                    string[] messages = { $"Hedefine ulaşmak için {remainingMl} ml su daha!", $"Devam et! {remainingMl} ml kaldı!", $"Vücudun sana teşekkür edecek! {remainingMl} ml daha." };
                    waterMessageLabel.Text = messages[new Random().Next(messages.Length)];
                    waterMessageLabel.TextColor = TextColorLight;
                }
            }
        }

        private async Task AddWaterShortcut(double amount)
        {
            var user = SessionManager.GetCurrentUser();
            if (user == null) { await DisplayAlert("Hata", "Lütfen önce giriş yapın.", "Tamam"); return; }
            await Database.AddWaterIntake(user.Id, DateTime.Today, amount);
            await UpdateWaterIntakeDisplay();
        }
    }
}