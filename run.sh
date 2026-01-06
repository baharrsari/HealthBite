#!/bin/bash

# --- Yapılandırma Ayarları ---
# Projenizin kök dizini (örneğin, .csproj dosyasının bulunduğu dizin)
# Bu yolu KENDİ projenizin dizinine göre GÜNCELLEYİN.
PROJECT_DIR="/c/Users/sarim/OneDrive/Desktop/HealthBite1/HealthBite"

# Uygulamanızın Android paket adı (AndroidManifest.xml'deki package değeri)
# Bu değişken, uygulama durdurma ve başlatma işlemleri için kullanılır.
APP_PACKAGE_NAME="com.companyname.healthbite" 

# --- Script Başlangıcı ---
echo "----------------------------------------------------"
echo "🚀 .NET MAUI Android projesi için HIZLI derleme ve doğrudan emülatörde başlatma başlatılıyor..."
echo "Hedef Dizin: $PROJECT_DIR"
echo "Uygulama Paket Adı: $APP_PACKAGE_NAME"
echo "----------------------------------------------------"

# Proje dizinine geçiş yap
cd "$PROJECT_DIR" || { echo "❌ Hata: Proje dizini bulunamadı: $PROJECT_DIR"; exit 1; }

# Mevcut uygulama örneğini durdur (önceki çalıştırmadan kalmışsa)
# Bu adım, uygulamanın temiz bir başlangıç yapmasını sağlar.
echo "🛑 Mevcut uygulama örneği durduruluyor ($APP_PACKAGE_NAME)..."
# '>/dev/null 2>&1' ile adb çıktısını ve hatalarını gizleyerek terminali daha temiz tutarız.
adb shell am force-stop "$APP_PACKAGE_NAME" > /dev/null 2>&1

# 1. Projeyi derle ve doğrudan emülatörde çalıştır
echo "✅ Proje derleniyor ve doğrudan emülatörde başlatılıyor (Debug modunda)..."
# -f net8.0-android: Android hedef çerçevesi (kullandığınız .NET sürümüne göre değişebilir, örn: net7.0-android)
# -c Debug: Gelişme ve hızlı test için Debug konfigürasyonu (Release'den daha hızlı derlenir)
# -t:Run: Derleme sonrası uygulamayı hemen çalıştırır. Bu, hem derlemeyi hem de emülatörde çalıştırmayı tek bir komutta birleştirir.
dotnet build -t:Run -f net8.0-android # <-- BU SATIR DA BU ŞEKİLDE OLMALI (iOS yerine Android)

# dotnet build -t:Run komutunun başarıyla tamamlanıp tamamlanmadığını kontrol et
if [ $? -ne 0 ]; then
    echo "❌ Hata: 'dotnet build -t:Run' komutu başarısız oldu. Lütfen yukarıdaki hataları kontrol edin."
    echo "Emülatörün çalıştığından, ADB'nin doğru yapılandırıldığından ve projenizin sorunsuz derlendiğinden emin olun."
    exit 1
fi

echo "----------------------------------------------------"
echo "🚀 İşlem tamamlandı: Uygulama emülatörde başlatıldı!"
echo "----------------------------------------------------"