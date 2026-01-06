using HealthBite.Data;
using HealthBite.Models;
using HealthBite.Services;
using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBite.Views
{
    public class WorkoutOfTheDayPage : ContentPage
    {
        private WorkoutDay _workoutDay;
        private Button completeButton;
        private bool _isCompleted = false;

        // Ana Temaya Uygun Renkler
        private readonly Color PrimaryColor = Color.FromRgb(25, 54, 48);
        private readonly Color AccentColorPink = Color.FromRgb(228, 178, 179);
        private readonly Color PageBackgroundColor = Color.FromRgb(246, 247, 249);
        private readonly Color CardBackgroundColor = Colors.White;
        private readonly Color ShadowColor = Color.FromHex("#E0E0E0");
        private readonly Color CompletedColor = Color.FromHex("#4CAF50");

        public WorkoutOfTheDayPage(WorkoutDay workoutDay)
        {
            _workoutDay = workoutDay;
            Title = workoutDay.DayName;
            BackgroundColor = PageBackgroundColor;

            // Egzersiz listesi için şablon (DataTemplate)
            var exerciseTemplate = new DataTemplate(() =>
            {
                var nameLabel = new Label { FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = PrimaryColor };
                nameLabel.SetBinding(Label.TextProperty, nameof(Exercise.Name));

                var setsLabel = new Label { FontSize = 16 };
                setsLabel.SetBinding(Label.TextProperty, nameof(Exercise.Sets), stringFormat: "Set: {0}");
                
                var repsLabel = new Label { FontSize = 16 };
                repsLabel.SetBinding(Label.TextProperty, nameof(Exercise.Reps), stringFormat: "Tekrar: {0}");

                var restLabel = new Label { FontSize = 16 };
                restLabel.SetBinding(Label.TextProperty, nameof(Exercise.RestPeriod), stringFormat: "Dinlenme: {0}");

                var exerciseDetailsLayout = new VerticalStackLayout { Spacing = 8, Children = { setsLabel, repsLabel, restLabel } };

                return new Frame
                {
                    Padding = 15, Margin = new Thickness(0, 0, 0, 15), CornerRadius = 10,
                    BackgroundColor = CardBackgroundColor, BorderColor = ShadowColor, HasShadow = true,
                    Content = new VerticalStackLayout
                    {
                        Spacing = 10,
                        Children = { nameLabel, new BoxView { HeightRequest = 1, Color = ShadowColor }, exerciseDetailsLayout }
                    }
                };
            });

            // Egzersizleri listeleyecek CollectionView
            var collectionView = new CollectionView
            {
                ItemsSource = _workoutDay.Exercises,
                ItemTemplate = exerciseTemplate,
                SelectionMode = SelectionMode.None,
                Margin = new Thickness(20, 0)
            };

            // Sayfa başlığı ve alt başlık
            var headerLayout = new VerticalStackLayout
            {
                Padding = 20,
                Children =
                {
                    new Label
                    {
                        Text = _workoutDay.DayName,
                        FontSize = 26,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = PrimaryColor,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    new Label
                    {
                        Text = "Bugünkü hedeflerini tamamla!",
                        FontSize = 16,
                        TextColor = Color.FromHex("#6C757D"),
                        HorizontalOptions = LayoutOptions.Center,
                        Margin = new Thickness(0, 0, 0, 10)
                    }
                }
            };
            
            // Tamamlama Butonu
            completeButton = new Button { FontAttributes = FontAttributes.Bold, TextColor = PrimaryColor, Margin = 20, HeightRequest = 50, CornerRadius = 25 };
            completeButton.Clicked += OnCompleteButtonClicked;

            // Ana Sayfa Yerleşimi
            Content = new Grid
            {
                RowDefinitions = { new RowDefinition(GridLength.Star), new RowDefinition(GridLength.Auto) },
                Children =
                {
                    new ScrollView { Content = new VerticalStackLayout { Children = { headerLayout, collectionView } } },
                    completeButton
                }
            };
            Grid.SetRow(completeButton, 1);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCompletionStatus();
        }

        private async Task LoadCompletionStatus()
        {
            var user = SessionManager.GetCurrentUser();
            if (user == null) return;
            var progress = await Database.GetTrainingProgressForDay(user.Id, DateTime.Today);
            _isCompleted = progress?.IsCompleted ?? false;
            UpdateButtonAppearance();
        }

        private void UpdateButtonAppearance()
        {
            if (_isCompleted)
            {
                completeButton.Text = "Antrenman Tamamlandı ✔";
                completeButton.BackgroundColor = CompletedColor;
                completeButton.TextColor = Colors.White;
            }
            else
            {
                completeButton.Text = "Antrenmanı Tamamla Olarak İşaretle";
                completeButton.BackgroundColor = AccentColorPink;
                completeButton.TextColor = PrimaryColor;
            }
        }

        private async void OnCompleteButtonClicked(object sender, EventArgs e)
        {
            var user = SessionManager.GetCurrentUser();
            if (user == null) return;
            
            _isCompleted = !_isCompleted;
            await Database.MarkTrainingDay(user.Id, DateTime.Today, _isCompleted);
            UpdateButtonAppearance();
            
            if(_isCompleted)
            {
                await DisplayAlert("Tebrikler!", "Bugünkü antrenmanını tamamladın. Harika iş!", "Tamam");
            }

            // Dashboard'a mesaj göndererek grafiğin yenilenmesini sağlıyoruz
            MessagingCenter.Send(this, "UpdateTrainingProgress");
            
            // Bir önceki sayfaya güvenle geri dönüyoruz
            await Navigation.PopAsync();
        }
    }
}