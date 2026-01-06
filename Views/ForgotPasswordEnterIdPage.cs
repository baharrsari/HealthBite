using HealthBite.Data;
using HealthBite.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace HealthBite.Views
{
    public class ForgotPasswordEnterIdPage : ContentPage
    {
        private Entry idEntry;
        private Button sendCodeButton;
        private Label errorLabel;

        public ForgotPasswordEnterIdPage()
        {
            // ... Constructor içeriği aynı ...
            Title = "Şifremi Unuttum";
            BackgroundColor = Color.FromHex("#29443F");

            idEntry = new Entry { Placeholder = "ID Numaranız", Keyboard = Keyboard.Numeric, MaxLength = 11, BackgroundColor=Colors.White };
            
            sendCodeButton = new Button
            {
                Text = "Doğrulama Kodu Gönder",
                BackgroundColor = Color.FromHex("#E4C2C1"),
                TextColor = Color.FromHex("#29443F"),
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 20
            };
            sendCodeButton.Clicked += OnSendCodeClicked;

            errorLabel = new Label { TextColor = Colors.Red, IsVisible = false, HorizontalOptions = LayoutOptions.Center };

            Content = new StackLayout
            {
                Padding = 30,
                Spacing = 20,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label { Text = "Hesabınızı Doğrulayın", FontSize = 24, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#E4C2C1"), HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Lütfen kayıtlı ID numaranızı girin. E-posta adresinize bir doğrulama kodu göndereceğiz.", FontSize = 14, TextColor = Colors.WhiteSmoke, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center },
                    idEntry,
                    sendCodeButton,
                    errorLabel
                }
            };
        }

        private async void OnSendCodeClicked(object sender, EventArgs e)
        {
            // ... (Kontroller aynı) ...
            if (string.IsNullOrWhiteSpace(idEntry.Text) || idEntry.Text.Length != 11)
            {
                ShowError("Lütfen geçerli bir 11 haneli ID numarası girin.");
                return;
            }

            sendCodeButton.IsEnabled = false;
            sendCodeButton.Text = "Gönderiliyor...";
            
            try
            {
                var user = await Database.GetUserById(idEntry.Text);
                if (user == null)
                {
                    ShowError("Bu ID numarasına sahip bir kullanıcı bulunamadı.");
                    return;
                }

                var verificationCode = new Random().Next(100000, 999999).ToString();
                
                // *** DEĞİŞİKLİK BURADA: Doğru metot çağrılıyor ***
                await EmailService.SendPasswordResetEmailAsync(user.Email, user.Name, verificationCode);

                await DisplayAlert("Başarılı", $"{user.Email} adresinize bir doğrulama kodu gönderildi.", "Tamam");

                await Shell.Current.GoToAsync($"{nameof(ForgotPasswordVerifyCodePage)}?userId={user.IDNumber}&code={verificationCode}");
            }
            catch (Exception ex)
            {
                ShowError("Kod gönderilirken bir hata oluştu. Lütfen tekrar deneyin.");
                System.Diagnostics.Debug.WriteLine($"Forgot Password Error: {ex.Message}");
            }
            finally
            {
                sendCodeButton.IsEnabled = true;
                sendCodeButton.Text = "Doğrulama Kodu Gönder";
            }
        }

        private void ShowError(string message)
        {
            errorLabel.Text = message;
            errorLabel.IsVisible = true;
        }
    }
}
