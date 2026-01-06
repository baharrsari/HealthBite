using Microsoft.Maui.Graphics;
using System;

namespace HealthBite.Drawable
{
    public class CircularProgressDrawable : IDrawable
    {
        public Color PrimaryColor { get; set; } = Color.FromHex("#4CAF50");
        public Color SecondaryColor { get; set; } = Color.FromHex("#E0E0E0");
        public float Progress { get; set; } = 0f;

        public void Draw(ICanvas canvas, RectF dirtyRect)
{
    // Canvas durumunu koruma altına alalım.
    canvas.SaveState();

    float strokeThickness = 15;
    float radius = (Math.Min(dirtyRect.Width, dirtyRect.Height) / 2) - (strokeThickness / 2);

    // --- 1. Adım: Arka Plan Halkasını (Gri) Çiz ---

    // Arka plan için bir yol (Path) oluşturalım.
    var backgroundPath = new PathF();
    // 360 derece hatasını tetiklememek için 359.99 derecelik bir yay ekleyelim.
    // Bu, görsel olarak tam bir daire ile aynıdır.
    backgroundPath.AddArc(dirtyRect.Center.X - radius, dirtyRect.Center.Y - radius, radius * 2, radius * 2, 0, 359.99f, false);

    // Arka plan yolunu çizelim.
    canvas.StrokeColor = SecondaryColor;
    canvas.StrokeSize = strokeThickness;
    // Hizalama sorununu çözmek için her iki halkanın da uçları yuvarlak olmalı.
    canvas.StrokeLineCap = LineCap.Round; 
    canvas.DrawPath(backgroundPath);

    // --- 2. Adım: İlerleme Yayını (Renkli) Çiz ---

    if (Progress > 0)
    {
        // İlerleme için yeni bir yol oluşturalım.
        var foregroundPath = new PathF();
        float startAngle = -90; // Saat 12 yönü
        float sweepAngle = 360 * Math.Clamp(Progress, 0, 1);
        float endAngle = startAngle + sweepAngle;
        
        foregroundPath.AddArc(dirtyRect.Center.X - radius, dirtyRect.Center.Y - radius, radius * 2, radius * 2, startAngle, endAngle, false);

        // İlerleme yolunu çizelim.
        canvas.StrokeColor = PrimaryColor;
        // StrokeSize ve StrokeLineCap zaten yukarıda ayarlandığı için tekrar ayarlamaya gerek yok.
        canvas.DrawPath(foregroundPath);
    }
    
    // Canvas durumunu geri yükleyelim.
    canvas.RestoreState();
}
     }
}