using HealthBite.Data;
using HealthBite.Models;
using HealthBite.Services;
using System;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace HealthBite.Views
{
    [QueryProperty(nameof(UserJson), "userJson")]
    public class SignupDetailsPage : ContentPage
    {
        public string UserJson
        {
            set => user = JsonSerializer.Deserialize<UserModel>(Uri.UnescapeDataString(value));
        }

        private UserModel user;
        private Entry heightEntry, weightEntry;
        
        // YENİ: Gün, Ay, Yıl için ayrı Entry'ler
        private Entry dayEntry, monthEntry, yearEntry;
        
        private Label errorLabel;

        public SignupDetailsPage()
        {
           // Navigasyon çubuğunun rengini sayfa ile uyumlu hale getirir.
            Shell.SetBackgroundColor(this, Color.FromHex("#29443F"));
            // Geri oku ve başlık gibi öğelerin rengini ayarlar.
            Shell.SetForegroundColor(this, Color.FromHex("#E4C2C1"));

            // Sayfanın üst kısmında başlık yazısının görünmemesi için boş bırakıyoruz.
            // Geri oku bu durumda bile görünmeye devam edecektir.
            Title = "";
            BackgroundColor = Color.FromHex("#29443F");

            var titleLabel = new Label
            {
                Text = "Complete Your Profile",
                FontSize = 26,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("#E4C2C1"),
                HorizontalOptions = LayoutOptions.Center
            };

            heightEntry = CreateStyledEntry("Height (cm)");
            weightEntry = CreateStyledEntry("Weight (kg)");

            // YENİ: Gün, Ay, Yıl giriş alanları oluşturuluyor
            dayEntry = new Entry { Placeholder = "GG", Keyboard = Keyboard.Numeric, MaxLength = 2, HorizontalTextAlignment = TextAlignment.Center };
            monthEntry = new Entry { Placeholder = "AA", Keyboard = Keyboard.Numeric, MaxLength = 2, HorizontalTextAlignment = TextAlignment.Center };
            yearEntry = new Entry { Placeholder = "YYYY", Keyboard = Keyboard.Numeric, MaxLength = 4, HorizontalTextAlignment = TextAlignment.Center };

            // YENİ: Tarih giriş alanlarını yan yana koymak için Grid
            var dateGrid = new Grid
            {
                ColumnSpacing = 10,
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }, // Gün
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }, // Ay
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }  // Yıl
                }
            };
            dateGrid.Add(dayEntry, 0);
            dateGrid.Add(monthEntry, 1);
            dateGrid.Add(yearEntry, 2);

            var completeBtn = new Button
            {
                Text = "Complete Signup",
                BackgroundColor = Color.FromHex("#E4C2C1"),
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 20,
                HeightRequest = 50,
                Shadow = new Shadow { Brush = Brush.Black, Offset = new Point(2, 2), Radius = 4, Opacity = 0.3f }
            };
            completeBtn.Clicked += CompleteClicked;

            errorLabel = new Label
            {
                TextColor = Colors.Red,
                FontAttributes = FontAttributes.Bold,
                IsVisible = false,
                HorizontalOptions = LayoutOptions.Center
            };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = new Thickness(30, 60, 30, 30),
                    Spacing = 25,
                    Children = {
                        titleLabel,
                        CreateLabeledField("Height", heightEntry),
                        CreateLabeledField("Weight", weightEntry),
                        // YENİ: Etiket ve Grid'i içeren yeni doğum günü bölümü
                        CreateLabeledField("Date of Birth", dateGrid),
                        completeBtn,
                        errorLabel
                    }
                }
            };
        }

        private async void CompleteClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(heightEntry.Text) ||
                string.IsNullOrWhiteSpace(weightEntry.Text) ||
                string.IsNullOrWhiteSpace(dayEntry.Text) ||
                string.IsNullOrWhiteSpace(monthEntry.Text) ||
                string.IsNullOrWhiteSpace(yearEntry.Text))
            {
                ShowError("Lütfen tüm alanları doldurun.");
                return;
            }

            if (!double.TryParse(heightEntry.Text, out double height) ||
                !double.TryParse(weightEntry.Text, out double weight) ||
                !int.TryParse(dayEntry.Text, out int day) ||
                !int.TryParse(monthEntry.Text, out int month) ||
                !int.TryParse(yearEntry.Text, out int year))
            {
                ShowError("Sayısal alanlar geçerli sayılar içermelidir.");
                return;
            }

            // *** YENİ: Tarih ve Yaş Hesaplama Mantığı ***
            DateTime birthDate;
            try
            {
                birthDate = new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
                ShowError("Lütfen geçerli bir tarih girin.");
                return;
            }

            if (birthDate > DateTime.Today)
            {
                ShowError("Doğum tarihi bugünden sonra olamaz.");
                return;
            }

            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            user.Height = height;
            user.Weight = weight;
            user.DateOfBirth = birthDate;
            user.Age = age;

            await Database.AddUser(user);
            SessionManager.SetCurrentUser(user);

            await Database.AddWeightEntry(new WeightEntryModel
            {
                UserId = user.Id,
                DateRecorded = DateTime.Today,
                WeightInKg = user.Weight
            });
            
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
        }

        private Entry CreateStyledEntry(string placeholder)
        {
            return new Entry
            {
                Placeholder = placeholder,
                PlaceholderColor = Color.FromArgb("#666666"),
                BackgroundColor = Colors.Transparent,
                TextColor = Colors.Black,
                HeightRequest = 45
            };
        }

        private View CreateLabeledField(string labelText, View view)
        {
            return new StackLayout
            {
                Spacing = 6,
                Children =
                {
                    new Label
                    {
                        Text = labelText,
                        TextColor = Color.FromHex("#E4C2C1"),
                        FontAttributes = FontAttributes.Bold
                    },
                    new Frame
                    {
                        CornerRadius = 12,
                        Padding = new Thickness(12, 8),
                        BackgroundColor = Colors.White,
                        Content = view,
                        HasShadow = true
                    }
                }
            };
        }
        
        private void ShowError(string message)
        {
            errorLabel.Text = message;
            errorLabel.IsVisible = true;
        }
    }
}
