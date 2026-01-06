// Dosya: HealthBite/Views/WelcomePage.cs

namespace HealthBite.Views;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
        Shell.SetNavBarIsVisible(this, false);

        BackgroundColor = Color.FromHex("#29443F");

        var title = new Label
        {
            Text = "HealthBite",
            TextColor = Color.FromHex("#E4C2C1"),
            FontSize = 32,
            HorizontalOptions = LayoutOptions.Center
        };

        var subtitle = new Label
        {
            Text = "Fuel Your Body, Nourish Your Life.",
            TextColor = Color.FromHex("#E4C2C1"),
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Center
        };

        // --- BUTON STİL DEĞİŞİKLİKLERİ ---
        var loginBtn = new Button
        {
            Text = "LOG IN",
            BackgroundColor = Color.FromHex("#E4C2C1"),
            TextColor = Colors.White,
            FontAttributes = FontAttributes.Bold, // Yazı kalın yapıldı
            CornerRadius = 25,                   // Köşe yuvarlaklığı ayarlandı
            HeightRequest = 50,                  // Yükseklik sabitlendi
            Margin = new Thickness(40, 0, 40, 0) // Yatay kenar boşlukları artırıldı
        };
        loginBtn.Clicked += async (s, e) => await Shell.Current.GoToAsync(nameof(LoginPage));

        var signupBtn = new Button
        {
            Text = "SIGN UP",
            BackgroundColor = Color.FromHex("#E4C2C1"),
            TextColor = Colors.White,
            FontAttributes = FontAttributes.Bold, // Yazı kalın yapıldı
            CornerRadius = 25,                   // Köşe yuvarlaklığı ayarlandı
            HeightRequest = 50,                  // Yükseklik sabitlendi
            Margin = new Thickness(40, 0, 40, 0) // Yatay kenar boşlukları artırıldı
        };
        signupBtn.Clicked += async (s, e) => await Shell.Current.GoToAsync(nameof(SignupPage));

        // --- STACKLAYOUT DEĞİŞİKLİĞİ ---
        Content = new StackLayout
        {
            VerticalOptions = LayoutOptions.Center,
            Spacing = 15, // Elemanlar arasına 15 birim boşluk eklendi
            Children = { title, subtitle, loginBtn, signupBtn }
        };
    }
}