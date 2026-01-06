using HealthBite.Data;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace HealthBite.Views
{
    [QueryProperty(nameof(UserId), "userId")]
    public class ForgotPasswordResetPasswordPage : ContentPage
    {
        public string UserId { get; set; }

        private Entry newPasswordEntry;
        private Entry confirmPasswordEntry;

        public ForgotPasswordResetPasswordPage()
        {
            Title = "Yeni Şifre Belirle";
            BackgroundColor = Color.FromHex("#29443F");

            newPasswordEntry = new Entry { Placeholder = "Yeni Şifre", IsPassword = true, BackgroundColor = Colors.White, TextColor=Color.FromHex("#E4C2C1")};
            confirmPasswordEntry = new Entry { Placeholder = "Yeni Şifreyi Onayla", IsPassword = true, BackgroundColor = Colors.White, TextColor=Color.FromHex("#E4C2C1")};

            var saveButton = new Button
            {
                Text = "Yeni Şifreyi Kaydet",
                BackgroundColor = Color.FromHex("#E4C2C1"),
                TextColor = Color.FromHex("#29443F"),
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 20
            };
            saveButton.Clicked += OnSaveClicked;

            Content = new StackLayout
            {
                Padding = 30,
                Spacing = 20,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label { Text = "Yeni Şifrenizi Belirleyin", FontSize = 22, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#E4C2C1"), HorizontalOptions = LayoutOptions.Center },
                    newPasswordEntry,
                    confirmPasswordEntry,
                    saveButton
                }
            };
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var newPass = newPasswordEntry.Text;
            var confirmPass = confirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(confirmPass))
            {
                await DisplayAlert("Hata", "Tüm alanlar doldurulmalıdır.", "Tamam");
                return;
            }

            if (newPass.Length < 8)
            {
                await DisplayAlert("Hata", "Yeni şifre en az 8 karakter olmalıdır.", "Tamam");
                return;
            }

            if (newPass != confirmPass)
            {
                await DisplayAlert("Hata", "Girdiğiniz şifreler eşleşmiyor.", "Tamam");
                return;
            }

            var user = await Database.GetUserById(UserId);
            if (user != null)
            {
                user.Password = newPass;
                await Database.UpdateUser(user);
                await DisplayAlert("Başarılı", "Şifreniz başarıyla güncellendi. Şimdi giriş yapabilirsiniz.", "Harika!");
                
                // *** DÜZELTİLMİŞ KOD ***
                // Uygulamanın kapanmasını engellemek için, açılan tüm sayfaları
                // kapatıp Login sayfasına geri dönüyoruz.
                await Shell.Current.GoToAsync("../../../");
            }
            else
            {
                await DisplayAlert("Kritik Hata", "Kullanıcı bilgisi bulunamadı.", "Tamam");
            }
        }
    }
}