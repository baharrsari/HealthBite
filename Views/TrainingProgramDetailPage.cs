using HealthBite.Data;
using HealthBite.Models;
using HealthBite.Services;
using Microsoft.Maui.Controls;
using System.Linq;
using System;

namespace HealthBite.Views
{
    [QueryProperty(nameof(ProgramId), "programId")]
    public class TrainingProgramDetailPage : ContentPage
    {
        private TrainingProgramModel _program;
        private Label titleLabel, descriptionLabel, detailsLabel;
        private VerticalStackLayout scheduleLayout;

        public string ProgramId { set { LoadProgram(int.Parse(value)); } }

        public TrainingProgramDetailPage()
        {
            BackgroundColor = Color.FromHex("#F8F9FA");

            titleLabel = new Label { FontSize = 26, FontAttributes = FontAttributes.Bold, TextColor = Color.FromRgb(25, 54, 48), Margin = new Thickness(0, 0, 0, 10) };
            descriptionLabel = new Label { FontSize = 16, TextColor = Colors.DarkSlateGray };
            detailsLabel = new Label { FontSize = 14, FontAttributes = FontAttributes.Italic, TextColor = Colors.Gray };
            scheduleLayout = new VerticalStackLayout { Spacing = 10, Margin = new Thickness(0, 15, 0, 0) };

            var selectButton = new Button
            {
                Text = "Bu Programı Seç",
                BackgroundColor = Color.FromRgb(228, 178, 179),
                TextColor = Color.FromRgb(25, 54, 48),
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(20, 30, 20, 20),
                HeightRequest = 50,
                CornerRadius = 25
            };
            selectButton.Clicked += OnSelectProgramClicked;

            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 15,
                    Children = { titleLabel, descriptionLabel, detailsLabel, new BoxView { HeightRequest = 1, Color = Colors.LightGray, Margin = new Thickness(0, 10) }, scheduleLayout, selectButton }
                }
            };
        }

        private void LoadProgram(int programId)
        {
            _program = Data.TrainingData.GetPredefinedTrainingPrograms().FirstOrDefault(p => p.Id == programId);
            if (_program != null)
            {
                Title = _program.Title;
                titleLabel.Text = _program.Title;
                descriptionLabel.Text = _program.Description;
                detailsLabel.Text = $"{_program.Location} • {_program.Category} • {_program.DurationInWeeks} Hafta";

                scheduleLayout.Children.Clear();
                scheduleLayout.Children.Add(new Label { Text = "Haftalık Plan:", FontAttributes = FontAttributes.Bold, FontSize = 18, TextColor = Color.FromRgb(25, 54, 48) });
                foreach (var day in _program.WeeklySchedule)
                {
                    var dayFrame = new Frame
                    {
                        Padding = 15,
                        Margin = new Thickness(0, 5),
                        CornerRadius = 8,
                        BackgroundColor = Colors.White,
                        BorderColor = Colors.LightSlateGray
                    };

                    var dayLayout = new VerticalStackLayout { Spacing = 8 };
                    dayLayout.Children.Add(new Label { Text = day.DayName, FontAttributes = FontAttributes.Bold, FontSize = 16 });

                    if (day.Exercises != null && day.Exercises.Any())
                    {
                        dayLayout.Children.Add(new BoxView { HeightRequest = 1, Color = Colors.LightGray });
                        foreach (var exercise in day.Exercises)
                        {
                            var exerciseLabel = new Label
                            {
                                FormattedText = new FormattedString
                                {
                                    Spans =
                                    {
                                        new Span { Text = $"{exercise.Name}: ", FontAttributes = FontAttributes.Bold },
                                        new Span { Text = $"{exercise.Sets} set x {exercise.Reps} tekrar" }
                                    }
                                }
                            };
                            dayLayout.Children.Add(exerciseLabel);
                        }
                    }
                    dayFrame.Content = dayLayout;
                    scheduleLayout.Children.Add(dayFrame);
                }
            }
        }

        private async void OnSelectProgramClicked(object sender, EventArgs e)
{
    if (_program != null)
    {
        bool confirm = await DisplayAlert("Program Seçimi", $"'{_program.Title}' programını mevcut antrenmanınız olarak ayarlamak istediğinize emin misiniz?", "Evet, Ayarla", "İptal");
        if (confirm)
        {
            var user = SessionManager.GetCurrentUser();
            if (user != null)
            {
                // Seçimi hem veritabanındaki kullanıcıya hem de session'a kaydet
                user.SelectedTrainingId = _program.Id;
                await Database.UpdateUser(user); // Kullanıcıyı veritabanında güncelle
                SessionManager.SetCurrentUser(user); // Session'daki kullanıcıyı güncelle
                
                SessionManager.SetCurrentTrainingProgram(_program);
                Preferences.Set($"TrainingProgramStartDate_{user.Id}", DateTime.Today);

                await DisplayAlert("Başarılı!", "Yeni antrenman programınız ayarlandı.", "Harika!");
                await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
            }
        }
    }
}
    }
}