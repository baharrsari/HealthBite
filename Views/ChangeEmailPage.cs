using HealthBite.Data;
using HealthBite.Services;
using Microsoft.Maui.Controls;

namespace HealthBite.Views
{
    public class ChangeEmailPage : ContentPage
    {
        private Label currentEmailLabel;
        private Entry newEmailEntry;

        // Tema Renkleri
        private readonly Color DarkPrimaryColor = Color.FromRgb(25, 54, 48);
        private readonly Color AccentColorPink = Color.FromRgb(228, 178, 179);
        private readonly Color LightBackgroundColor = Color.FromRgb(246, 247, 249);
        
        public ChangeEmailPage()
        {
            Title = "E-posta Değiştir";
            BackgroundColor = LightBackgroundColor;

            currentEmailLabel = new Label
            {
                FontSize = 16,
                TextColor = Colors.DarkSlateGray,
                
                HorizontalTextAlignment = TextAlignment.Center
            };

            newEmailEntry = new Entry
            {
                Placeholder = "Yeni e-posta adresiniz",
                BackgroundColor = Colors.White,
               
                
            };

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
                    new Label { Text = "E-posta Değişikliği", FontSize = 24, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0,0,0,15), TextColor = DarkPrimaryColor },
                    new Label { Text = "Mevcut E-posta Adresiniz:", FontSize = 14, TextColor = DarkPrimaryColor },
                    currentEmailLabel,
                    newEmailEntry,
                    saveButton
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var user = SessionManager.GetCurrentUser();
            if (user != null)
            {
                currentEmailLabel.Text = user.Email;
            }
        }

        private async void OnSaveButtonClicked(object sender, System.EventArgs e)
        {
            var newEmail = newEmailEntry.Text;

            if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
            {
                await DisplayAlert("Hata", "Lütfen geçerli bir e-posta adresi girin.", "Tamam");
                return;
            }

            var user = SessionManager.GetCurrentUser();
            if (user == null)
            {
                await DisplayAlert("Hata", "Kullanıcı oturumu bulunamadı.", "Tamam");
                return;
            }

            user.Email = newEmail;
            await Database.UpdateUser(user);
            SessionManager.UpdateCurrentUser(user);

            await DisplayAlert("Başarılı", "E-posta adresiniz başarıyla güncellendi.", "Tamam");
            await Navigation.PopAsync();
        }
    }
}