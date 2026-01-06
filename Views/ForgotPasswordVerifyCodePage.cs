using Microsoft.Maui.Controls;
using System;

namespace HealthBite.Views
{
    [QueryProperty(nameof(UserId), "userId")]
    [QueryProperty(nameof(CorrectCode), "code")]
    public class ForgotPasswordVerifyCodePage : ContentPage
    {
        public string UserId { get; set; }
        public string CorrectCode { get; set; }
        
        private Entry codeEntry;
        private Label errorLabel;

        public ForgotPasswordVerifyCodePage()
        {
            Title = "Kodu Doğrula";
            BackgroundColor = Color.FromHex("#29443F");

            codeEntry = new Entry { Placeholder = "6 Haneli Kod", Keyboard = Keyboard.Numeric, MaxLength = 6, HorizontalTextAlignment = TextAlignment.Center, FontSize = 22,BackgroundColor=Colors.White, TextColor = Color.FromHex("#E4C2C1") };
            
            var verifyButton = new Button
            {
                Text = "Kodu Doğrula",
                BackgroundColor = Color.FromHex("#E4C2C1"),
                TextColor = Color.FromHex("#29443F"),
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 20
            };
            verifyButton.Clicked += OnVerifyClicked;

            errorLabel = new Label { TextColor = Colors.Red, IsVisible = false, HorizontalOptions = LayoutOptions.Center };

            Content = new StackLayout
            {
                Padding = 30,
                Spacing = 20,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label { Text = "E-postanıza gelen kodu girin.", FontSize = 18, TextColor = Colors.WhiteSmoke, HorizontalOptions = LayoutOptions.Center },
                    codeEntry,
                    verifyButton,
                    errorLabel
                }
            };
        }

        private async void OnVerifyClicked(object sender, EventArgs e)
        {
            if (codeEntry.Text == CorrectCode)
            {
                errorLabel.IsVisible = false;
                await Shell.Current.GoToAsync($"{nameof(ForgotPasswordResetPasswordPage)}?userId={UserId}");
            }
            else
            {
                errorLabel.Text = "Hatalı kod girdiniz.";
                errorLabel.IsVisible = true;
            }
        }
    }
}
