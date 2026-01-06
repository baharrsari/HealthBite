using HealthBite.Data;
using HealthBite.Models;
using HealthBite.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthBite.Views
{
    public class DietProgramsPage : ContentPage
    {
        // Tema Renkleri
        private readonly Color DarkPrimaryColor = Color.FromRgb(25, 54, 48);
        private readonly Color AccentColorPink = Color.FromRgb(228, 178, 179);
        private readonly Color LightBackgroundColor = Color.FromRgb(246, 247, 249);
        private readonly Color TextColorOnDark = Colors.White;
        private readonly Color CardBackgroundColor = Colors.White;
        private readonly Color SubtleGrayText = Color.FromRgb(120, 120, 120);

        private CollectionView allDietsCollectionView;
        private CollectionView personalizedDietsCollectionView;
        private List<DietModel> allDiets = new List<DietModel>();
        private Picker goalPicker;
        private Label personalizedDietsSectionTitleLabel;
        private Label allDietsSectionTitleLabel;
        private StackLayout personalizedDietsSection;
        private StackLayout allDietsSection;

        public DietProgramsPage()
        {
            // Navigasyon çubuğunun rengini sayfa ile uyumlu hale getirir.
            Shell.SetBackgroundColor(this, Color.FromRgb(246, 247, 249));
            // Geri oku ve başlık gibi öğelerin rengini ayarlar.
            Shell.SetForegroundColor(this, Color.FromHex("#343A40"));

            Title = "";
            BackgroundColor = LightBackgroundColor;

            var pageTitleLabel = new Label
            {
                Text = "Diyet Programlarını Keşfet",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                TextColor = DarkPrimaryColor,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 30, 0, 20)
            };

            var goalPickerFrame = new Frame
            {
                CornerRadius = 25,
                Padding = new Thickness(15, 0),
                Margin = new Thickness(20, 0, 20, 20),
                BackgroundColor = DarkPrimaryColor,
                HasShadow = true,
                Content = (goalPicker = new Picker
                {
                    Title = "Hedefine Göre Filtrele",
                    TitleColor = AccentColorPink,
                    ItemsSource = new List<string> { "Tümü", "Kilo Verme", "Kilo Alma", "Kilo Koruma" },
                    SelectedIndex = 0,
                    TextColor = TextColorOnDark,
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    HeightRequest = 50,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    BackgroundColor = Colors.Transparent
                })
            };
            goalPicker.SelectedIndexChanged += GoalPicker_SelectedIndexChanged;

            personalizedDietsSectionTitleLabel = new Label
            {
                Text = "🌟 Senin İçin Seçtiklerimiz",
                FontSize = 22,
                FontAttributes = FontAttributes.Bold,
                TextColor = DarkPrimaryColor,
                HorizontalOptions = LayoutOptions.Start, // Hizalama güncellendi
                Margin = new Thickness(0, 20, 0, 15),
            };
            personalizedDietsCollectionView = CreateDietCollectionView();
            personalizedDietsSection = new StackLayout { Children = { personalizedDietsSectionTitleLabel, personalizedDietsCollectionView }, IsVisible = false };

            allDietsSectionTitleLabel = new Label
            {
                Text = "Tüm Diyet Programları",
                FontSize = 22,
                FontAttributes = FontAttributes.Bold,
                TextColor = DarkPrimaryColor,
                HorizontalOptions = LayoutOptions.Start, // Hizalama güncellendi
                Margin = new Thickness(0, 20, 0, 15)
            };
            allDietsCollectionView = CreateDietCollectionView();
            allDietsSection = new StackLayout { Children = { allDietsSectionTitleLabel, allDietsCollectionView } };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    // Sayfa geneli için yatay padding eklendi
                    Padding = new Thickness(20, 0, 20, 20),
                    Children = { pageTitleLabel, goalPickerFrame, personalizedDietsSection, allDietsSection }
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            allDiets = Database.GetAllDiets() ?? new List<DietModel>();

            // Sayfa her açıldığında Picker'ı "Tümü" olarak ayarla ve listeyi güncelle
            ResetFilterAndRefreshList();
        }

        private void GoalPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Sadece kullanıcı manuel olarak seçim yaptığında listeyi güncelle
            UpdateDietList();
        }

        private void ResetFilterAndRefreshList()
        {
            // Olayın iki kez tetiklenmesini önlemek için event handler'ı geçici olarak kaldır
            goalPicker.SelectedIndexChanged -= GoalPicker_SelectedIndexChanged;

            // Picker'ı "Tümü" olarak ayarla
            goalPicker.SelectedItem = "Tümü";

            // Event handler'ı tekrar ekle
            goalPicker.SelectedIndexChanged += GoalPicker_SelectedIndexChanged;

            // Listeyi yeni duruma göre güncelle
            UpdateDietList();
        }

        private void UpdateDietList()
        {
            var user = SessionManager.GetCurrentUser();
            string recommendedGoal = "";

            if (user != null && user.Height > 0 && user.Weight > 0)
            {
                double bmi = user.CalculateBmi();
                string bmiCategory = GetBmiCategory(bmi);
                if (bmiCategory == "Zayıf") recommendedGoal = "Kilo Alma";
                else if (bmiCategory == "Normal Kilolu") recommendedGoal = "Kilo Koruma";
                else if (bmiCategory.Contains("Obez") || bmiCategory == "Fazla Kilolu") recommendedGoal = "Kilo Verme";
            }

            var selectedFilter = goalPicker.SelectedItem?.ToString()?.Trim();

            var personalizedDiets = new List<DietModel>();
            if (!string.IsNullOrEmpty(recommendedGoal))
            {
                personalizedDiets = allDiets
                    .Where(d => d.Goal?.Trim().Equals(recommendedGoal, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();
            }

            if (selectedFilter == "Tümü")
            {
                if (personalizedDiets.Any())
                {
                    personalizedDietsSection.IsVisible = true;
                    personalizedDietsCollectionView.ItemsSource = personalizedDiets;
                    allDietsSectionTitleLabel.Text = "Diğer Diyetler";
                }
                else
                {
                    personalizedDietsSection.IsVisible = false;
                    allDietsSectionTitleLabel.Text = "Tüm Diyet Programları";
                }

                allDietsSection.IsVisible = true;
                allDietsCollectionView.ItemsSource = allDiets.Except(personalizedDiets).ToList();
            }
            else
            {
                personalizedDietsSection.IsVisible = false;
                allDietsSection.IsVisible = true;
                allDietsSectionTitleLabel.Text = $"{selectedFilter} Diyetleri";
                allDietsCollectionView.ItemsSource = allDiets
                    .Where(d => d.Goal?.Trim().Equals(selectedFilter, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();
            }
        }

        private CollectionView CreateDietCollectionView()
{
    var cv = new CollectionView
    {
        SelectionMode = SelectionMode.None,
        ItemTemplate = new DataTemplate(() =>
        {
            var titleLabel = new Label { FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = DarkPrimaryColor, LineBreakMode = LineBreakMode.TailTruncation, Margin = new Thickness(0, 0, 0, 2) };
            titleLabel.SetBinding(Label.TextProperty, "Title");

            var goalLabel = new Label { FontSize = 14, TextColor = AccentColorPink, FontAttributes = FontAttributes.Bold };
            goalLabel.SetBinding(Label.TextProperty, new Binding("Goal", stringFormat: "Amaç: {0}"));

            var descriptionLabel = new Label { FontSize = 14, TextColor = SubtleGrayText, MaxLines = 2, LineBreakMode = LineBreakMode.TailTruncation, Margin = new Thickness(0, 0, 0, 5) };
            descriptionLabel.SetBinding(Label.TextProperty, "Description");

            var caloriesLabel = new Label { FontSize = 12, TextColor = Colors.DarkSlateGray, FontAttributes = FontAttributes.Italic };
            caloriesLabel.SetBinding(Label.TextProperty, new Binding("Calories", stringFormat: "Günlük Yaklaşık {0:F0} kcal"));

            // Kart içeriği VerticalStackLayout ile düzenlendi
            var contentStack = new VerticalStackLayout { Padding = 15, Spacing = 5, Children = { titleLabel, goalLabel, descriptionLabel, caloriesLabel } };

            // Kart tasarımı 'Frame' olarak değiştirildi
            var dietCard = new Frame
            {
                CornerRadius = 12,
                Padding = 0,
                Margin = new Thickness(0),
                HasShadow = false,
                BorderColor = AccentColorPink,
                BackgroundColor = CardBackgroundColor,
                Content = contentStack
            };

            // *** HATA DÜZELTMESİ BURADA ***
            // 'Frame.BindingContextProperty' yerine 'BindableObject.BindingContextProperty' kullanıldı.
            dietCard.SetBinding(BindableObject.BindingContextProperty, ".");

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) =>
            {
                try
                {
                    if (s is Frame tappedFrame && tappedFrame.BindingContext is DietModel diet)
                    {
                        var mealDetails = new StringBuilder();
                        mealDetails.AppendLine("\n--- Örnek Öğünler ---");
                        if (diet.Meals != null && diet.Meals.Any())
                        {
                            foreach (var mealEntry in diet.Meals.OrderBy(m => GetMealOrder(m.Key)))
                            {
                                mealDetails.AppendLine($"\n🍽️ **{TranslateMealKey(mealEntry.Key)}:**");
                                if (mealEntry.Value?.PrimaryFoodItems?.Any() == true)
                                {
                                    foreach (var foodDetail in mealEntry.Value.PrimaryFoodItems)
                                    {
                                        mealDetails.AppendLine($"  • {foodDetail?.Name ?? "N/A"} ({foodDetail?.Portion ?? "N/A"})");
                                    }
                                } else mealDetails.AppendLine("  - Bu öğün için yiyecek belirtilmemiş.");
                            }
                        } else mealDetails.AppendLine("\nBu diyet için öğün bilgisi mevcut değil.");

                        string alertMessage = $"🎯 **Amaç:** {diet.Goal}\n🔥 **Günlük Kalori:** {diet.Calories:F0} kcal\n\n📝 **Açıklama:** {diet.Description}\n{mealDetails}";
                        bool confirm = await Shell.Current.DisplayAlert(diet.Title, alertMessage, "Bu Diyeti Seç", "İptal");

                        if (confirm)
                        {
                            var user = SessionManager.GetCurrentUser();
                            if (user != null)
                            {
                                user.SelectedDietId = diet.Id;
                                await Database.UpdateUser(user);
                                SessionManager.SetCurrentUser(user);
                                SessionManager.SetCurrentDiet(diet);

                                await Shell.Current.DisplayAlert("Seçildi!", $"{diet.Title} adlı diyet programınız başarıyla seçildi.", "Harika!");
                                await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Hata", "Diyeti seçmek için kullanıcı oturumu bulunamadı.", "Tamam");
                                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                            }
                        }
                    }
                }
                catch (Exception ex) { await Shell.Current.DisplayAlert("Beklenmedik Hata", $"Bir hata oluştu: {ex.Message}", "Tamam"); }
            };
            dietCard.GestureRecognizers.Add(tapGesture);
            return dietCard;
        }),
        // Yerleşim 'LinearItemsLayout' olarak değiştirildi
        ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 15 }
    };
    return cv;
}
        private string TranslateMealKey(string mealKey) => mealKey switch { "Breakfast" => "Kahvaltı", "Lunch" => "Öğle Yemeği", "Dinner" => "Akşam Yemeği", "Snack1" => "Ara Öğün 1", "Snack2" => "Ara Öğün 2", _ => mealKey };

        private int GetMealOrder(string mealKey) => mealKey switch { "Breakfast" => 1, "Snack1" => 2, "Lunch" => 3, "Snack2" => 4, "Dinner" => 5, _ => 99 };

        private string GetBmiCategory(double bmi)
        {
            if (bmi <= 0) return "Bilinmiyor";
            if (bmi < 18.5) return "Zayıf";
            if (bmi < 24.9) return "Normal Kilolu";
            if (bmi < 29.9) return "Fazla Kilolu";
            if (bmi < 34.9) return "Obez (Sınıf I)";
            if (bmi < 39.9) return "Obez (Sınıf II)";
            return "Aşırı Obez (Sınıf III)";
        }
    }
}