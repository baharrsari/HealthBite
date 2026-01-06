using HealthBite.Data;
using HealthBite.Services;
using Microsoft.Maui.Controls;
using System;

namespace HealthBite.Views
{
    public class ChangePasswordPage : ContentPage
    {
        private Entry oldPasswordEntry, newPasswordEntry, confirmPasswordEntry;
        
        // Tema Renkleri
        private readonly Color DarkPrimaryColor = Color.FromRgb(25, 54, 48);
        private readonly Color AccentColorPink = Color.FromRgb(228, 178, 179);
        private readonly Color LightBackgroundColor = Color.FromRgb(246, 247, 249);
        
        public ChangePasswordPage()
        {
            Title = "Şifre Değiştir";
            BackgroundColor = LightBackgroundColor;

            oldPasswordEntry = new Entry { Placeholder = "Mevcut Şifreniz", IsPassword = true , BackgroundColor=Colors.White};
            newPasswordEntry = new Entry { Placeholder = "Yeni Şifreniz", IsPassword = true , BackgroundColor=Colors.White };
            confirmPasswordEntry = new Entry { Placeholder = "Yeni Şifreyi Onayla", IsPassword = true , BackgroundColor=Colors.White};

            var saveButton = new Button
            {
                Text = "Kaydet",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = AccentColorPink,
                TextColor = DarkPrimaryColor,
                Margin = new Thickness(0, 20, 0, 0),
                CornerRadius = 10
            };
            saveButton.Clicked += OnSaveButtonClicked;

            Content = new VerticalStackLayout
            {
                Padding = 30,
                Spacing = 15,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label { Text = "Şifre Değişikliği", FontSize = 24, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center, TextColor = DarkPrimaryColor },
                    new Label { Text = "Güvenliğiniz için lütfen mevcut şifrenizi girin.", FontSize = 14, HorizontalOptions = LayoutOptions.Center, Margin= new Thickness(0,0,0,15), TextColor = DarkPrimaryColor },
                    oldPasswordEntry,
                    newPasswordEntry,
                    confirmPasswordEntry,
                    saveButton
                }
            };
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var oldPass = oldPasswordEntry.Text;
            var newPass = newPasswordEntry.Text;
            var confirmPass = confirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(oldPass) || string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(confirmPass))
            {
                await DisplayAlert("Hata", "Tüm alanları doldurmanız gerekmektedir.", "Tamam");
                return;
            }

            // *** DÜZELTİLMİŞ KONTROL: Şifre uzunluğu 8 karakter olmalı ***
            if (newPass.Length < 8)
            {
                await DisplayAlert("Hata", "Yeni şifreniz en az 8 karakter olmalıdır.", "Tamam");
                return;
            }

            if (newPass != confirmPass)
            {
                await DisplayAlert("Hata", "Yeni şifreler uyuşmuyor.", "Tamam");
                return;
            }

            var user = SessionManager.GetCurrentUser();
            var userFromDb = await Database.GetUserById(user.IDNumber);

            if (userFromDb.Password != oldPass)
            {
                await DisplayAlert("Hata", "Mevcut şifreniz yanlış.", "Tamam");
                return;
            }

            userFromDb.Password = newPass;
            await Database.UpdateUser(userFromDb);
            SessionManager.UpdateCurrentUser(userFromDb);

            await DisplayAlert("Başarılı", "Şifreniz başarıyla değiştirildi.", "Tamam");
            await Navigation.PopAsync();
        }
    }
}