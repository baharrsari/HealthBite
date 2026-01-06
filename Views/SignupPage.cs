using HealthBite.Data;
using HealthBite.Models;
using HealthBite.Services;
using System.Text.Json;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using System;

namespace HealthBite.Views
{
    public class SignupPage : ContentPage
    {
        private Entry nameEntry, surnameEntry, idEntry, emailEntry, passEntry, confirmPassEntry;
        private Label errorLabel;
        private Button signupBtn;

        public SignupPage()
        {
            // Navigasyon çubuğunun rengini sayfa ile uyumlu hale getirir.
            Shell.SetBackgroundColor(this, Color.FromHex("#29443F"));
            // Geri oku ve başlık gibi öğelerin rengini ayarlar.
            Shell.SetForegroundColor(this, Color.FromHex("#E4C2C1"));

            // Sayfanın üst kısmında başlık yazısının görünmemesi için boş bırakıyoruz.
            // Geri oku bu durumda bile görünmeye devam edecektir.
            //Title = "";
            BackgroundColor = Color.FromHex("#29443F");
            
            // İsim ve Soyisim alanları oluşturuluyor
            nameEntry = CreateEntry("Name");
            surnameEntry = CreateEntry("Surname");

            // *** YENİ: İsim ve Soyisim için Otomatik Büyük Harf Ayarı ***
            // Klavyenin her kelimenin ilk harfini otomatik büyük yapması sağlanıyor.
            nameEntry.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            surnameEntry.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            
            idEntry = CreateEntry("ID Number", Keyboard.Numeric);
            
            // *** YENİ: E-posta alanı için doğru klavye türü ayarlanıyor ***
            // Bu, '@' ve diğer gerekli karakterlerin klavyede görünmesini sağlar.
            emailEntry = CreateEntry("Email", Keyboard.Email); 
            
            passEntry = CreateEntry("Password", isPassword: true);
            confirmPassEntry = CreateEntry("Confirm Password", isPassword: true);
            
            signupBtn = new Button { Text = "Sign Up", BackgroundColor = Color.FromHex("#E4C2C1"), TextColor = Colors.White, CornerRadius = 20, HeightRequest = 50, FontAttributes = FontAttributes.Bold, Shadow = new Shadow { Brush = Brush.Black, Offset = new Point(2, 2), Radius = 4, Opacity = 0.3f } };
            signupBtn.Clicked += SignupClicked;

            errorLabel = new Label { TextColor = Colors.Red, FontAttributes = FontAttributes.Bold, IsVisible = false, HorizontalOptions = LayoutOptions.Center };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = new Thickness(30, 60, 30, 30),
                    Spacing = 20,
                    Children = { CreateLabeledField("Name", nameEntry), CreateLabeledField("Surname", surnameEntry), CreateLabeledField("ID Number", idEntry), CreateLabeledField("Email", emailEntry), CreateLabeledField("Password", passEntry), CreateLabeledField("Confirm Password", confirmPassEntry), signupBtn, errorLabel }
                }
            };
        }

        private async void SignupClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameEntry.Text) || string.IsNullOrWhiteSpace(surnameEntry.Text) || string.IsNullOrWhiteSpace(idEntry.Text) || string.IsNullOrWhiteSpace(emailEntry.Text) || string.IsNullOrWhiteSpace(passEntry.Text) || string.IsNullOrWhiteSpace(confirmPassEntry.Text)) { ShowError("Lütfen tüm alanları doldurun."); return; }
            if (passEntry.Text != confirmPassEntry.Text) { ShowError("Şifreler eşleşmiyor."); return; }
            if (idEntry.Text.Length != 11) { ShowError("ID numarası 11 karakter olmalıdır."); return; }
            if (passEntry.Text.Length < 8) { ShowError("Şifre en az 8 karakter olmalıdır."); return; }
            if (!long.TryParse(idEntry.Text, out _)) { ShowError("ID sadece sayılardan oluşmalıdır."); return; }
            if (await Database.IDExists(idEntry.Text)) { ShowError("Bu ID numarası zaten kayıtlı."); return; }

            signupBtn.IsEnabled = false;
            signupBtn.Text = "Gönderiliyor...";

            try
            {
                var newUser = new UserModel
                {
                    Name = nameEntry.Text.Trim(),
                    Surname = surnameEntry.Text.Trim(),
                    IDNumber = idEntry.Text.Trim(),
                    Email = emailEntry.Text.Trim(),
                    Password = passEntry.Text.Trim()
                };

                var verificationCode = new Random().Next(100000, 999999).ToString();
                
                await EmailService.SendAccountVerificationEmailAsync(newUser.Email, newUser.Name, verificationCode);

                var userJson = JsonSerializer.Serialize(newUser);
                
                await Shell.Current.GoToAsync($"{nameof(EmailCheckPage)}?userJson={Uri.EscapeDataString(userJson)}&verificationCode={verificationCode}");
            }
            catch (Exception ex)
            {
                ShowError("Doğrulama e-postası gönderilemedi.");
                System.Diagnostics.Debug.WriteLine($"Email Error: {ex.Message}");
            }
            finally
            {
                signupBtn.IsEnabled = true;
                signupBtn.Text = "Sign Up";
            }
        }
        
        private Entry CreateEntry(string placeholder, Keyboard keyboard = null, bool isPassword = false) 
        { 
            return new Entry 
            { 
                Placeholder = placeholder, 
                PlaceholderColor = Color.FromArgb("#666666"), 
                Keyboard = keyboard ?? Keyboard.Default, 
                IsPassword = isPassword, 
                BackgroundColor = Colors.Transparent, 
                TextColor = Colors.Black, 
                HeightRequest = 45 
            }; 
        }

        private View CreateLabeledField(string labelText, Entry entry) 
        { 
            return new StackLayout 
            { 
                Spacing = 6, 
                Children = 
                { 
                    new Label { Text = labelText, TextColor = Color.FromHex("#E4C2C1"), FontAttributes = FontAttributes.Bold }, 
                    new Frame { CornerRadius = 12, Padding = new Thickness(12, 8), BackgroundColor = Colors.White, Content = entry, HasShadow = true } 
                } 
            }; 
        }
        
        private void ShowError(string message) 
        { 
            errorLabel.Text = message; 
            errorLabel.IsVisible = true; 
        }
    }
}
