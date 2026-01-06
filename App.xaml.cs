using HealthBite.Services;
using HealthBite.Views;
using HealthBite.Data; // Database.Init için eklendi

namespace HealthBite;

public partial class App : Application
{
    public App()
    {
        //InitializeComponent();

        // Veritabanını başlat
        Database.Init();

        MainPage = new AppShell();

        // Oturum durumuna göre ilk sayfayı ayarla (bu kısım aynı kalabilir)
        if (SessionManager.IsLoggedIn)
        {
            Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
        }
        else
        {
            Shell.Current.GoToAsync($"//{nameof(WelcomePage)}");
        }
    }
}