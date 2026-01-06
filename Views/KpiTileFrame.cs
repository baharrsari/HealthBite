using Microsoft.Maui.Controls;

namespace HealthBite.Views
{
    public class KpiTileFrame : Frame
    {
        public KpiTileFrame()
        {
            // Tüm stil özelliklerini doğrudan burada, kendi içinde tanımlıyoruz.
            BackgroundColor = Colors.White;
            CornerRadius = 15;
            Padding = 15;
            HasShadow = true;
            BorderColor = Color.FromHex("#E0E0E0");
        }
    }
}