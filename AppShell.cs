using HealthBite.Views;
using HealthBite.Services;
using Microsoft.Maui.Controls.Shapes;

namespace HealthBite;

public class AppShell : Shell
{
    public AppShell()
    {
        // 1. Rota Kayıtları
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
        Routing.RegisterRoute(nameof(SignupDetailsPage), typeof(SignupDetailsPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(DietProgramsPage), typeof(DietProgramsPage));
        Routing.RegisterRoute(nameof(DailyMealCheckPage), typeof(DailyMealCheckPage));
        Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
        Routing.RegisterRoute(nameof(WeightHistoryPage), typeof(WeightHistoryPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
        Routing.RegisterRoute(nameof(ChangeEmailPage), typeof(ChangeEmailPage));
        Routing.RegisterRoute(nameof(TrainingProgramsPage), typeof(TrainingProgramsPage));
        Routing.RegisterRoute(nameof(TrainingProgramDetailPage), typeof(TrainingProgramDetailPage));
        Routing.RegisterRoute(nameof(WorkoutOfTheDayPage), typeof(WorkoutOfTheDayPage));
        Routing.RegisterRoute(nameof(WaterIntakeDetailPage), typeof(WaterIntakeDetailPage));
        Routing.RegisterRoute(nameof(EmailCheckPage), typeof(EmailCheckPage));
         Routing.RegisterRoute(nameof(ForgotPasswordEnterIdPage), typeof(ForgotPasswordEnterIdPage));
        Routing.RegisterRoute(nameof(ForgotPasswordVerifyCodePage), typeof(ForgotPasswordVerifyCodePage));
        Routing.RegisterRoute(nameof(ForgotPasswordResetPasswordPage), typeof(ForgotPasswordResetPasswordPage));

        // 2. Genel Shell Ayarları
        this.FlyoutBehavior = FlyoutBehavior.Flyout;
        this.FlyoutWidth = DeviceInfo.Platform == DevicePlatform.WinUI ? 350 : 280;
        this.FlyoutBackgroundColor = Color.FromHex("#29443F");
        this.FlyoutBackdrop = new SolidColorBrush(Color.FromRgba(0, 0, 0, 0.6));

        // 3. Flyout Header (Başlık Alanı)
        var flyoutHeader = new StackLayout
        {
            Padding = new Thickness(20, 40, 20, 20),
            BackgroundColor = Color.FromHex("#29443F"),
            Children =
            {
                new Label
                {
                    Text = "HealthBite",
                    TextColor = Color.FromHex("#E4C2C1"),
                    FontSize = 28,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 0)
                },
                new Label
                {
                    Text = "Sağlıklı Yaşam Asistanınız",
                    TextColor = Colors.WhiteSmoke,
                    FontSize = 14,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 5, 0, 0)
                }
            }
        };
        this.FlyoutHeader = flyoutHeader;

        // 4. Flyout Footer (Alt Alan - Çıkış Yap Butonu)
        var logoutButton = new Button
        {
            Text = "Çıkış Yap",
            TextColor = Color.FromHex("#E4C2C1"),
            BackgroundColor = Colors.Transparent,
            FontAttributes = FontAttributes.Bold,
            FontSize = 16,
            Margin = new Thickness(20, 10, 20, 20),
            HeightRequest = 45,
            CornerRadius = 22,
            BorderColor = Color.FromHex("#E4C2C1"),
            BorderWidth = 1
        };
        logoutButton.Clicked += async (s, e) =>
        {
            bool confirm = await Shell.Current.DisplayAlert("Çıkış", "Uygulamadan çıkmak istediğinizden emin misiniz?", "Evet", "Hayır");
            if (confirm)
            {
                SessionManager.ClearSession();
                await Shell.Current.GoToAsync($"//{nameof(WelcomePage)}");
                Shell.Current.FlyoutIsPresented = false;
            }
        };
        this.FlyoutFooter = new StackLayout { Children = { logoutButton } };

        // 5. ItemTemplate (Menü Öğesi Şablonu)
        var flyoutItemTemplate = new DataTemplate(() =>
        {
            var grid = new Grid
            {
                ColumnSpacing = 15,
                Padding = new Thickness(25, 18),
                ColumnDefinitions =
                {
                    // İkon kaldırıldığı için bu sütuna artık gerek yok, ancak şablon yapısını bozmamak için kalabilir veya düzenlenebilir.
                    // Şimdilik ikonun yer kaplamaması için genişliğini 0 yapalım.
                    new ColumnDefinition { Width = 0 }, // İkon için ayrılan alanı sıfırladık.
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            // İkon artık yüklenmeyecek.
            var icon = new Image();
            icon.SetBinding(Image.SourceProperty, nameof(FlyoutItem.FlyoutIcon)); // Bu satır kalsa da kaynak null olacağı için sorun olmaz.

            var label = new Label
            {
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#E4C2C1"),
                FontSize = 17,
                FontAttributes = FontAttributes.None
            };
            label.SetBinding(Label.TextProperty, nameof(FlyoutItem.Title));

            // Grid'e sadece label'ı eklemek yeterli olacaktır.
            // grid.Children.Add(icon);
            // Grid.SetColumn(icon, 0);

            grid.Children.Add(label);
            Grid.SetColumn(label, 1);

            return grid;
        });
        this.ItemTemplate = flyoutItemTemplate;

        // 6. Shell Öğeleri (WelcomePage ve Flyout Menü Öğeleri)
        var welcomeShellContent = new ShellContent
        {
            Title = "Welcome",
            Route = nameof(WelcomePage),
            ContentTemplate = new DataTemplate(typeof(WelcomePage))
        };
        Shell.SetFlyoutItemIsVisible(welcomeShellContent, false);
        Items.Add(welcomeShellContent);

        // Dashboard için FlyoutItem oluşturuluyor (İkon referansı kaldırıldı)
        var dashboardFlyoutItem = new FlyoutItem
        {
            Title = "Dashboard",
            Route = nameof(DashboardPage),
            // FlyoutIcon = "dashboard_icon.png", // BU SATIR KALDIRILDI
            Items =
            {
                new ShellContent
                {
                    Title = "Dashboard",
                    ContentTemplate = new DataTemplate(typeof(DashboardPage)),
                    Route = nameof(DashboardPage)
                }
            }
        };
        Shell.SetNavBarIsVisible(dashboardFlyoutItem, true);
        Items.Add(dashboardFlyoutItem);

        // Diğer menü öğeleri (İkon referansları kaldırıldı)
       // Items.Add(new FlyoutItem { Title = "Profilim", Route = nameof(ProfilePage), /* FlyoutIcon = "profile_icon.png", */ Items = { new ShellContent { Title = "Profilim", ContentTemplate = new DataTemplate(typeof(ProfilePage)), Route = nameof(ProfilePage) } } });
        Items.Add(new FlyoutItem { Title = "Diyet Programlarım", Route = nameof(DietProgramsPage), /* FlyoutIcon = "diet_icon.png", */ Items = { new ShellContent { Title = "Diyet Programlarım", ContentTemplate = new DataTemplate(typeof(DietProgramsPage)), Route = nameof(DietProgramsPage) } } });
        Items.Add(new FlyoutItem { Title = "Günlük Öğün Takibi", Route = nameof(DailyMealCheckPage), /* FlyoutIcon = "meal_check_icon.png", */ Items = { new ShellContent { Title = "Günlük Öğün Takibi", ContentTemplate = new DataTemplate(typeof(DailyMealCheckPage)), Route = nameof(DailyMealCheckPage) } } });
        Items.Add(new FlyoutItem { Title = "Antrenman Programları", Route = nameof(TrainingProgramsPage), /* FlyoutIcon = "workout_icon.png", */ Items = { new ShellContent { Title = "Antrenman Programları", ContentTemplate = new DataTemplate(typeof(TrainingProgramsPage)), Route = nameof(TrainingProgramsPage) } } });
        Items.Add(new FlyoutItem { Title = "Kilo Geçmişim", Route = nameof(WeightHistoryPage), /* FlyoutIcon = "weight_icon.png", */ Items = { new ShellContent { Title = "Kilo Geçmişim", ContentTemplate = new DataTemplate(typeof(WeightHistoryPage)), Route = nameof(WeightHistoryPage) } } });
        Items.Add(new FlyoutItem { Title = "Ayarlar", Route = nameof(SettingsPage), /* FlyoutIcon = "settings_icon.png", */ Items = { new ShellContent { Title = "Ayarlar", ContentTemplate = new DataTemplate(typeof(SettingsPage)), Route = nameof(SettingsPage) } } });
    }
}