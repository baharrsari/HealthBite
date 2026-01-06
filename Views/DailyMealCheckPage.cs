using HealthBite.Data;
using HealthBite.Models;
using HealthBite.Services;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBite.Views;

public class DailyMealCheckPage : ContentPage
{
    // Renk Tanımlamaları
    private readonly Color DarkPrimaryColor = Color.FromRgb(25, 54, 48);
    private readonly Color AccentColorPink = Color.FromRgb(228, 178, 179);
    private readonly Color LightBackgroundColor = Color.FromRgb(246, 247, 249);
    private readonly Color TextColorOnDark = Colors.White;
    private readonly Color SubtleGrayBorder = Color.FromRgb(220, 220, 220);
    private readonly Color CardBackgroundColor = Colors.White;

    // UI Elementleri
    private Button breakfastCompleteBtn, lunchCompleteBtn, dinnerCompleteBtn;
    private Label totalCaloriesLabel;
    private VerticalStackLayout breakfastFoodsLayout, lunchFoodsLayout, dinnerFoodsLayout;
    private Button breakfastAltButton, lunchAltButton, dinnerAltButton;

    // Sayfa İçeriği Alanları
    private ScrollView mealContentLayout; // Öğünlerin gösterileceği ana alan
    private StackLayout noDietContentLayout; // Diyet seçilmediğinde gösterilecek alan

    // Durum ve Veri Değişkenleri
    private Dictionary<string, bool> _isShowingAlternative = new();
    private DietProgressModel _currentProgress;
    private readonly List<string> motivationalMessages = new() { "Harika gidiyorsun, devam et! 💪", "Bir adım daha hedefine yaklaştın! ✨", "Sağlıklı seçimlerin için tebrikler! 🥗", "Bu enerjiyle her şeyi başarırsın! 🚀", "Kendine yaptığın bu iyilik paha biçilemez! ❤️", "Unutma, her sağlıklı öğün bir zaferdir! 🏆" };
    private readonly Random random = new();

    public DailyMealCheckPage()
    {
        // Navigasyon çubuğunun rengini sayfa ile uyumlu hale getirir.
            Shell.SetBackgroundColor(this, Color.FromRgb(246, 247, 249));
            // Geri oku ve başlık gibi öğelerin rengini ayarlar.
            Shell.SetForegroundColor(this, Color.FromHex("#343A40"));

        BackgroundColor = LightBackgroundColor;

        // --- 1. Diyet Seçilmediğinde Gösterilecek Arayüz ---
        var noDietIcon = new Label { Text = "🥗", FontSize = 48, HorizontalOptions = LayoutOptions.Center };
        var noDietLabel = new Label { Text = "Henüz bir diyet programı seçmediniz.", FontSize = 18, TextColor = DarkPrimaryColor, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center };
        var goToDietsButton = new Button 
        { 
            Text = "Diyet Programlarını Görüntüle", 
            BackgroundColor = AccentColorPink, 
            TextColor = DarkPrimaryColor, 
            FontAttributes = FontAttributes.Bold, 
            CornerRadius = 25, 
            HeightRequest = 50,
            Padding = new Thickness(20, 0),
            Margin = new Thickness(0, 20, 0, 0)
        };
        goToDietsButton.Clicked += async (s, e) => await Shell.Current.GoToAsync($"//{nameof(DietProgramsPage)}");
        
        noDietContentLayout = new StackLayout
        {
            Spacing = 15,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Fill,
            Padding = 30,
            IsVisible = false, // Başlangıçta gizli
            Children = { noDietIcon, noDietLabel, goToDietsButton }
        };

        // --- 2. Öğünler Gösterildiğinde Kullanılacak Arayüz ---
        var pageTitleLabel = new Label { Text = "Bugünkü Öğünlerin", FontSize = 24, FontAttributes = FontAttributes.Bold, TextColor = DarkPrimaryColor, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 20, 0, 20) };
        breakfastFoodsLayout = new VerticalStackLayout { Spacing = 5 };
        lunchFoodsLayout = new VerticalStackLayout { Spacing = 5 };
        dinnerFoodsLayout = new VerticalStackLayout { Spacing = 5 };
        breakfastAltButton = CreateAlternativeButton("Breakfast");
        lunchAltButton = CreateAlternativeButton("Lunch");
        dinnerAltButton = CreateAlternativeButton("Dinner");
        breakfastCompleteBtn = CreateMealCompleteButton("Breakfast");
        lunchCompleteBtn = CreateMealCompleteButton("Lunch");
        dinnerCompleteBtn = CreateMealCompleteButton("Dinner");
        totalCaloriesLabel = new Label { Text = "Günlük Hedef: 0 kcal", FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = DarkPrimaryColor, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 20, 0, 10) };
       
        mealContentLayout = new ScrollView 
        {
            Content = new StackLayout 
            {
                Padding = 20, Spacing = 15,
                Children = 
                {
                    pageTitleLabel,
                    CreateMealSectionView("Kahvaltı", "☀️", breakfastFoodsLayout, breakfastAltButton, breakfastCompleteBtn),
                    CreateSeparator(),
                    CreateMealSectionView("Öğle Yemeği", "🍽️", lunchFoodsLayout, lunchAltButton, lunchCompleteBtn),
                    CreateSeparator(),
                    CreateMealSectionView("Akşam Yemeği", "🌙", dinnerFoodsLayout, dinnerAltButton, dinnerCompleteBtn),
                    CreateSeparator(),
                    totalCaloriesLabel
                }
            }
        };
        
        // --- 3. Ana İçeriği Ayarla ---
        Content = new Grid
        {
            Children = { mealContentLayout, noDietContentLayout }
        };

        _isShowingAlternative["Breakfast"] = false;
        _isShowingAlternative["Lunch"] = false;
        _isShowingAlternative["Dinner"] = false;
    }

    protected override async void OnAppearing() 
    { 
        base.OnAppearing(); 
        await LoadDailyMeals(); 
    }

    private async Task LoadDailyMeals()
    {
        var user = SessionManager.GetCurrentUser();
        if (user == null)
        {
            // Kullanıcı girişi yapılmamışsa bu durum kritik, login sayfasına yönlendirme kalabilir.
            await DisplayAlert("Hata", "Öğünleri görüntülemek için lütfen giriş yapın.", "Tamam");
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            return;
        }

        var diet = SessionManager.GetCurrentDiet();
        if (diet == null || diet.Meals == null)
        {
            // Diyet seçilmemişse arayüzü güncelle
            Title = "Diyet Seçimi";
            mealContentLayout.IsVisible = false;
            noDietContentLayout.IsVisible = true;
            return;
        }
        
        // Diyet seçilmişse normal akışa devam et
        Title = "";
        mealContentLayout.IsVisible = true;
        noDietContentLayout.IsVisible = false;
        
        _currentProgress = await Database.GetOrCreateMealProgress(user.Id, diet.Id, DateTime.Today);

        if (breakfastCompleteBtn != null) UpdateButtonAppearance(breakfastCompleteBtn, _currentProgress.Breakfast);
        if (lunchCompleteBtn != null) UpdateButtonAppearance(lunchCompleteBtn, _currentProgress.Lunch);
        if (dinnerCompleteBtn != null) UpdateButtonAppearance(dinnerCompleteBtn, _currentProgress.Dinner);
        
        totalCaloriesLabel.Text = $"Günlük Hedef: {diet.Calories:F0} kcal";

        if (diet.Meals.TryGetValue("Breakfast", out var breakfastPlan)) PopulateMealLayout(breakfastFoodsLayout, breakfastPlan, "Breakfast", breakfastAltButton); else { breakfastFoodsLayout.Children.Clear(); breakfastFoodsLayout.Children.Add(new Label { Text = "Kahvaltı bilgisi yok." }); breakfastAltButton.IsVisible = false; }
        if (diet.Meals.TryGetValue("Lunch", out var lunchPlan)) PopulateMealLayout(lunchFoodsLayout, lunchPlan, "Lunch", lunchAltButton); else { lunchFoodsLayout.Children.Clear(); lunchFoodsLayout.Children.Add(new Label { Text = "Öğle yemeği bilgisi yok."}); lunchAltButton.IsVisible = false; }
        if (diet.Meals.TryGetValue("Dinner", out var dinnerPlan)) PopulateMealLayout(dinnerFoodsLayout, dinnerPlan, "Dinner", dinnerAltButton); else { dinnerFoodsLayout.Children.Clear(); dinnerFoodsLayout.Children.Add(new Label { Text = "Akşam yemeği bilgisi yok."}); dinnerAltButton.IsVisible = false; }
    }

    private async void OnMealCompleteButtonClicked(object sender, EventArgs e)
    {
        if (sender is not Button clickedButton || clickedButton.CommandParameter is not string mealKey) return;
        if (_currentProgress == null) { await DisplayAlert("Hata", "Öğün ilerleme verisi yüklenemedi. Lütfen sayfayı yenileyin.", "Tamam"); return; }
        
        bool previousState; // Önceki durumu saklayalım
        switch (mealKey) 
        { 
            case "Breakfast": previousState = _currentProgress.Breakfast; _currentProgress.Breakfast = !previousState; break; 
            case "Lunch": previousState = _currentProgress.Lunch; _currentProgress.Lunch = !previousState; break; 
            case "Dinner": previousState = _currentProgress.Dinner; _currentProgress.Dinner = !previousState; break; 
            default: return; 
        }

        // Butonun görünümünü yeni duruma göre güncelle
        // _currentProgress.GetMealCompletion(mealKey) yerine doğrudan _currentProgress.Breakfast, .Lunch, .Dinner kullanabiliriz
        // çünkü yukarıdaki switch bloğunda zaten güncellendi.
        switch (mealKey)
        {
            case "Breakfast": UpdateButtonAppearance(clickedButton, _currentProgress.Breakfast); break;
            case "Lunch": UpdateButtonAppearance(clickedButton, _currentProgress.Lunch); break;
            case "Dinner": UpdateButtonAppearance(clickedButton, _currentProgress.Dinner); break;
        }

        await Database.UpdateMealProgress(_currentProgress); // Öğün ilerlemesini otomatik kaydet

        // Sadece tamamlanmamış durumdan tamamlanmış duruma geçerken mesaj göster
        bool currentState = false;
        switch (mealKey)
        {
            case "Breakfast": currentState = _currentProgress.Breakfast; break;
            case "Lunch": currentState = _currentProgress.Lunch; break;
            case "Dinner": currentState = _currentProgress.Dinner; break;
        }

        if (!previousState && currentState) 
        {
            string randomMessage = motivationalMessages[random.Next(motivationalMessages.Count)];
            await DisplayAlert("Tebrikler!", randomMessage, "Süper!");
        }
    }

    private void UpdateButtonAppearance(Button button, bool isCompleted) { if (isCompleted) { button.Text = "Tamamlandı ✔"; button.BackgroundColor = DarkPrimaryColor; button.TextColor = TextColorOnDark; button.BorderColor = Colors.Transparent; button.BorderWidth = 0; } else { button.Text = "Tamamla"; button.BackgroundColor = CardBackgroundColor; button.TextColor = DarkPrimaryColor; button.BorderColor = DarkPrimaryColor; button.BorderWidth = 1.5; } }
    private Button CreateMealCompleteButton(string mealKey) { var button = new Button { HeightRequest = 40, CornerRadius = 20, FontAttributes = FontAttributes.Bold, FontSize = 14, Padding = new Thickness(15,0), HorizontalOptions = LayoutOptions.End, CommandParameter = mealKey }; button.Clicked += OnMealCompleteButtonClicked; return button; }
    private Button CreateAlternativeButton(string mealKey) { var button = new Button { Text = "Alternatifi Göster", FontSize = 12, BackgroundColor = AccentColorPink, TextColor = DarkPrimaryColor, Padding = new Thickness(10,5), HeightRequest = 38, CornerRadius = 19, Margin = new Thickness(0, 8, 0, 8), HorizontalOptions = LayoutOptions.End, CommandParameter = mealKey }; button.Clicked += async (s, e) => { if (s is Button b && b.CommandParameter is string mk) { _isShowingAlternative[mk] = !_isShowingAlternative[mk]; await LoadDailyMeals(); } }; return button; }
    private void PopulateMealLayout(VerticalStackLayout foodsLayout, MealPlan mealPlan, string mealKey, Button altButton) { foodsLayout.Children.Clear(); List<FoodDetailModel> foodsToShow; bool isShowingAlt = _isShowingAlternative.ContainsKey(mealKey) && _isShowingAlternative[mealKey]; if (isShowingAlt && mealPlan.HasAlternative) { foodsToShow = mealPlan.AlternativeFoodItems; altButton.Text = "Ana Öğünü Göster"; } else { foodsToShow = mealPlan.PrimaryFoodItems; altButton.Text = "Alternatifi Göster"; } if (foodsToShow == null || !foodsToShow.Any()) { foodsLayout.Children.Add(new Label { Text = "Bu öğün için yiyecek listesi boş." }); } else { foreach (var food in foodsToShow) foodsLayout.Children.Add(CreateFoodDetailView(food)); } altButton.IsVisible = mealPlan.HasAlternative && (mealPlan.PrimaryFoodItems.Any() || mealPlan.AlternativeFoodItems.Any()); }
    private View CreateFoodDetailView(FoodDetailModel food) { var grid = new Grid(); var namePortionStack = new VerticalStackLayout { Spacing = 0 }; namePortionStack.Children.Add(new Label { Text = food.Name, FontAttributes = FontAttributes.Bold, TextColor = DarkPrimaryColor }); namePortionStack.Children.Add(new Label { Text = $"Porsiyon: {food.Portion}", FontSize = 12, TextColor = Colors.DarkSlateGray }); grid.Add(namePortionStack, 0, 0); var macrosStack = new VerticalStackLayout { Spacing = 0, HorizontalOptions = LayoutOptions.End }; macrosStack.Children.Add(new Label { Text = $"🔥 {food.Calories:F0} kcal", FontSize = 13, FontAttributes = FontAttributes.Bold, TextColor = AccentColorPink, HorizontalTextAlignment = TextAlignment.End }); var pcfStack = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 8, HorizontalOptions = LayoutOptions.End }; pcfStack.Children.Add(new Label { Text = $"P: {food.Protein:F0}g", FontSize = 11, TextColor = Color.FromHex("#4CAF50") }); pcfStack.Children.Add(new Label { Text = $"C: {food.Carbs:F0}g", FontSize = 11, TextColor = Color.FromHex("#2196F3") }); pcfStack.Children.Add(new Label { Text = $"F: {food.Fat:F0}g", FontSize = 11, TextColor = Color.FromHex("#FFC107") }); macrosStack.Children.Add(pcfStack); grid.Add(macrosStack, 1, 0); return new Frame { CornerRadius = 8, Padding = new Thickness(10, 8), BackgroundColor = CardBackgroundColor, BorderColor = SubtleGrayBorder, HasShadow = false, Content = grid }; }
    private StackLayout CreateMealSectionView(string title, string icon, View foodsView, Button altButton, Button completeButton) { var headerStack = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 10, HorizontalOptions = LayoutOptions.Start, Children = { new Label { Text = icon, FontSize = 22, VerticalOptions = LayoutOptions.Center }, new Label { Text = title, FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = DarkPrimaryColor, VerticalOptions = LayoutOptions.Center } } }; return new StackLayout { Orientation = StackOrientation.Vertical, Spacing = 10, Children = { headerStack, new Frame { CornerRadius = 12, Padding = 15, BackgroundColor = CardBackgroundColor, HasShadow = false, BorderColor = AccentColorPink, Content = new StackLayout { Spacing = 10, Children = { foodsView, altButton, new StackLayout { HorizontalOptions = LayoutOptions.End, Children = { completeButton } } } } } } }; }
    private BoxView CreateSeparator() => new BoxView { Color = SubtleGrayBorder, HeightRequest = 1, Margin = new Thickness(0, 10, 0, 10) };
}