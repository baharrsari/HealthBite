using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using HealthBite.Data;
using HealthBite.Services;
using System.Collections.Generic; // List için eklendi

namespace HealthBite.Views
{
    public class SettingsPage : ContentPage
    {
        private Picker themePicker;
        private Switch notificationsSwitch;
        private Entry waterGoalEntry;
        private Button saveWaterGoalButton;
        private Label versionLabel, privacyPolicyLabel;
        private Frame generalSettingsFrame, goalsSettingsFrame, accountSettingsFrame;
        private List<Label> sectionTitles = new();
        private List<Label> rowLabels = new();
        private List<Button> accountButtons = new();

        // Renk Paletleri
        private readonly Color LightPageBackground = Color.FromRgb(246, 247, 249);
        private readonly Color LightCardBackground = Colors.White;
        private readonly Color LightText = Color.FromRgb(25, 54, 48);
        private readonly Color LightSubtleText = Colors.DarkSlateGray;
        private readonly Color LightBorder = Color.FromRgb(220, 220, 220);
        private readonly Color LightAccent = Color.FromRgb(228, 178, 179);
        private readonly Color DarkPageBackground = Color.FromRgb(18, 18, 18);
        private readonly Color DarkCardBackground = Color.FromRgb(30, 30, 30);
        private readonly Color DarkText = Colors.WhiteSmoke;
        private readonly Color DarkSubtleText = Colors.LightGray;
        private readonly Color DarkBorder = Color.FromRgb(45, 45, 45);

        public SettingsPage()
        {
            Title = "Ayarlar";

            // --- Arayüz Elemanlarını Oluşturma ---
            themePicker = new Picker { Title = "Uygulama Teması" };
            themePicker.Items.Add("Sistem Varsayılanı");
            themePicker.Items.Add("Açık Mod");
            themePicker.Items.Add("Koyu Mod");
            themePicker.SelectedIndexChanged += OnThemeChanged;

            notificationsSwitch = new Switch();
            notificationsSwitch.Toggled += OnNotificationsToggled;

            waterGoalEntry = new Entry { Placeholder = "Örn: 2500", Keyboard = Keyboard.Numeric };
            saveWaterGoalButton = new Button { Text = "Kaydet", TextColor = LightText, FontAttributes = FontAttributes.Bold, CornerRadius = 10 };
            saveWaterGoalButton.Clicked += OnSaveWaterGoalClicked;

            versionLabel = new Label { HorizontalOptions = LayoutOptions.Center };
            privacyPolicyLabel = new Label { Text = "Gizlilik Politikası", TextDecorations = TextDecorations.Underline, HorizontalOptions = LayoutOptions.Center };
            var privacyTapGesture = new TapGestureRecognizer();
            privacyTapGesture.Tapped += async (s, e) => { await DisplayAlert("Gizlilik Politikası", "Gizlilik politikası metni burada yer alacak.", "Tamam"); };
            privacyPolicyLabel.GestureRecognizers.Add(privacyTapGesture);
            
            // --- UI Bölümlerini Oluşturma ---
            goalsSettingsFrame = CreateSettingsSection("Hedefler", new VerticalStackLayout
            {
                Spacing = 10,
                Children = { CreateSettingsRow("Günlük Su Hedefi (ml)", waterGoalEntry), new HorizontalStackLayout { HorizontalOptions = LayoutOptions.End, Children = { saveWaterGoalButton } } }
            });
            
            var changeEmailButton = CreateAccountButton("E-posta Adresini Değiştir", OnChangeEmailClicked);
            var changePasswordButton = CreateAccountButton("Şifreyi Değiştir", OnChangePasswordClicked);
            accountButtons.Add(changeEmailButton);
            accountButtons.Add(changePasswordButton);
            accountSettingsFrame = CreateSettingsSection("Hesap Yönetimi", new VerticalStackLayout
            {
                Spacing = 15,
                Children = { changeEmailButton, changePasswordButton }
            });

            generalSettingsFrame = CreateSettingsSection("Genel Ayarlar", new VerticalStackLayout
            {
                Spacing = 15,
                Children = { CreateSettingsRow("Tema", themePicker), CreateSettingsRow("Bildirimlere İzin Ver", notificationsSwitch) }
            });

            // --- Sayfa Yerleşimini Oluşturma ---
            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 25,
                    Children = { accountSettingsFrame, generalSettingsFrame, goalsSettingsFrame,
                        new VerticalStackLayout
                        {
                            Padding = new Thickness(0, 20), Spacing = 10,
                            Children = { versionLabel, privacyPolicyLabel }
                        }
                    }
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Tema ayarını yükle
            var theme = Preferences.Get("app_theme", "system");
            switch (theme)
            {
                case "light": themePicker.SelectedIndex = 1; ApplyTheme(AppTheme.Light); break;
                case "dark": themePicker.SelectedIndex = 2; ApplyTheme(AppTheme.Dark); break;
                default: themePicker.SelectedIndex = 0; ApplyTheme(App.Current.RequestedTheme); break;
            }
            
            // Diğer ayarları yükle
            notificationsSwitch.IsToggled = Preferences.Get("notifications_enabled", true);
            waterGoalEntry.Text = Preferences.Get("water_goal_ml", 2000).ToString();
        }

        private void ApplyTheme(AppTheme theme)
        {
            bool isDarkMode = theme == AppTheme.Dark;
            
            BackgroundColor = isDarkMode ? DarkPageBackground : LightPageBackground;
            versionLabel.TextColor = isDarkMode ? DarkSubtleText : LightSubtleText;
            privacyPolicyLabel.TextColor = isDarkMode ? LightAccent : LightText;
            
            foreach (var frame in new[] { generalSettingsFrame, goalsSettingsFrame, accountSettingsFrame })
            {
                frame.BackgroundColor = isDarkMode ? DarkCardBackground : LightCardBackground;
                frame.BorderColor = isDarkMode ? DarkBorder : LightBorder;
            }

            foreach (var label in sectionTitles) { label.TextColor = isDarkMode ? DarkText : LightText; }
            foreach (var label in rowLabels) { label.TextColor = isDarkMode ? DarkText : LightText; }

            waterGoalEntry.TextColor = isDarkMode ? DarkText : LightText;
            waterGoalEntry.PlaceholderColor = isDarkMode ? DarkSubtleText : LightSubtleText;
            notificationsSwitch.OnColor = isDarkMode ? LightAccent : LightAccent; // OnColor her iki temada da aynı olabilir.
            saveWaterGoalButton.BackgroundColor = isDarkMode ? LightAccent : LightAccent;
            saveWaterGoalButton.TextColor = isDarkMode ? DarkText : LightText;
            
            foreach (var button in accountButtons)
            {
                button.TextColor = isDarkMode ? LightAccent : LightText;
                button.BorderColor = isDarkMode ? LightAccent : LightText;
            }
        }
        
        private void OnThemeChanged(object sender, EventArgs e)
        {
            var selected = themePicker.SelectedItem.ToString();
            string themeValue;
            AppTheme newTheme;
            switch (selected)
            {
                case "Açık Mod": newTheme = AppTheme.Light; themeValue = "light"; break;
                case "Koyu Mod": newTheme = AppTheme.Dark; themeValue = "dark"; break;
                default: newTheme = AppTheme.Unspecified; themeValue = "system"; break;
            }
            App.Current.UserAppTheme = newTheme;
            Preferences.Set("app_theme", themeValue);
            ApplyTheme(newTheme == AppTheme.Unspecified ? App.Current.RequestedTheme : newTheme);
        }

        // *** DÜZELTİLMİŞ METOT 1 ***
        private void OnNotificationsToggled(object sender, ToggledEventArgs e)
        {
            // Switch'in mevcut durumunu (true/false) al
            bool isEnabled = e.Value;
            
            // Seçimi cihaz hafızasına kaydet
            Preferences.Set("notifications_enabled", isEnabled);
            
            // Kullanıcıya geri bildirim ver (isteğe bağlı ama önerilir)
            DisplayAlert("Ayarlar Güncellendi", $"Bildirimler şimdi {(isEnabled ? "açık" : "kapalı")}.", "Tamam");
        }

        // *** DÜZELTİLMİŞ METOT 2 ***
        private void OnSaveWaterGoalClicked(object sender, EventArgs e)
        {
            // Entry'deki metni al ve sayıya çevirmeye çalış
            if (int.TryParse(waterGoalEntry.Text, out int newGoal) && newGoal > 0)
            {
                // Başarılı olursa, yeni hedefi cihaz hafızasına kaydet
                Preferences.Set("water_goal_ml", newGoal);
                
                // Kullanıcıya geri bildirim ver
                DisplayAlert("Başarılı", "Yeni su hedefiniz kaydedildi.", "Harika!");
            }
            else
            {
                // Başarısız olursa, kullanıcıyı uyar
                DisplayAlert("Hata", "Lütfen geçerli bir sayı girin.", "Tamam");
            }
        }
        
        private async void OnChangeEmailClicked(object sender, EventArgs e) => await Navigation.PushAsync(new ChangeEmailPage());
        private async void OnChangePasswordClicked(object sender, EventArgs e) => await Navigation.PushAsync(new ChangePasswordPage());


        // --- YARDIMCI METODLAR ---
        private Frame CreateSettingsSection(string title, Layout content)
        {
            var titleLabel = new Label { Text = title, FontSize = 18, FontAttributes = FontAttributes.Bold };
            sectionTitles.Add(titleLabel);
            return new Frame
            {
                Padding = 20, CornerRadius = 15, HasShadow = true,
                Content = new VerticalStackLayout
                {
                    Spacing = 15,
                    Children = { titleLabel, new BoxView { HeightRequest = 1, Color = Colors.LightGray }, content }
                }
            };
        }

        private Grid CreateSettingsRow(string labelText, View control)
        {
            var label = new Label { Text = labelText, FontSize = 16, VerticalOptions = LayoutOptions.Center };
            rowLabels.Add(label);
            var grid = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star }, new ColumnDefinition { Width = GridLength.Auto } },
                VerticalOptions = LayoutOptions.Center
            };
            grid.Add(label, 0);
            grid.Add(control, 1);
            return grid;
        }
        
        private Button CreateAccountButton(string text, EventHandler clickHandler)
        {
            var button = new Button { Text = text, BackgroundColor = Colors.Transparent, BorderWidth = 1, CornerRadius = 10, HorizontalOptions = LayoutOptions.Fill };
            button.Clicked += clickHandler;
            return button;
        }
    }
}