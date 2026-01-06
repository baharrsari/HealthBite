using Microsoft.Maui.Controls;
using System.Text.Json;
using HealthBite.Models; // UserModel için gerekli
using System;

namespace HealthBite.Views
{
    [QueryProperty(nameof(UserJson), "userJson")]
    [QueryProperty(nameof(VerificationCode), "verificationCode")]
    public class EmailCheckPage : ContentPage
    {
        public string UserJson { get; set; }
        public string VerificationCode { get; set; }

        private Entry codeEntry;
        private Label infoLabel, errorLabel;

        public EmailCheckPage()
        {
            Title = "E-posta Doğrulama";
            BackgroundColor = Color.FromHex("#29443F");

            infoLabel = new Label
            {
                Text = "Lütfen e-posta adresinize gönderilen 6 haneli doğrulama kodunu girin.",
                TextColor = Colors.WhiteSmoke,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };

            codeEntry = new Entry
            {
                Placeholder = "______",
                Keyboard = Keyboard.Numeric,
                MaxLength = 6,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Colors.White,
                TextColor = Color.FromHex("#29443F"),
                HeightRequest = 60
            };

            var verifyButton = new Button
            {
                Text = "Doğrula",
                BackgroundColor = Color.FromHex("#E4C2C1"),
                TextColor = Color.FromHex("#29443F"),
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 20,
                HeightRequest = 50,
                Margin = new Thickness(0, 20, 0, 0)
            };
            verifyButton.Clicked += OnVerifyClicked;

            errorLabel = new Label
            {
                TextColor = Colors.Red,
                FontAttributes = FontAttributes.Bold,
                IsVisible = false,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 10, 0, 0)
            };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = 30,
                    VerticalOptions = LayoutOptions.Center,
                    Spacing = 15,
                    Children = { infoLabel, codeEntry, verifyButton, errorLabel }
                }
            };
        }

        private async void OnVerifyClicked(object sender, EventArgs e)
        {
            if (codeEntry.Text == VerificationCode)
            {
                // Kod doğru, bir sonraki adıma geç
                errorLabel.IsVisible = false;
                await Shell.Current.GoToAsync($"{nameof(SignupDetailsPage)}?userJson={UserJson}");
            }
            else
            {
                // Kod yanlış
                errorLabel.Text = "Hatalı kod girdiniz. Lütfen tekrar deneyin.";
                errorLabel.IsVisible = true;
            }
        }
    }
}
