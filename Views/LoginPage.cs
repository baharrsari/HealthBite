using HealthBite.Data;
using HealthBite.Models;
using HealthBite.Services;
using Microsoft.Maui.Controls;
using System.Linq;

namespace HealthBite.Views
{
    public class LoginPage : ContentPage
    {
        private Entry idEntry, passwordEntry;
        private Label errorLabel;

        public LoginPage()
        {
           
            // Navigasyon çubuğunun rengini sayfa ile uyumlu hale getirir.
            Shell.SetBackgroundColor(this, Color.FromHex("#29443F"));
            // Geri oku ve başlık gibi öğelerin rengini ayarlar.
            Shell.SetForegroundColor(this, Color.FromHex("#E4C2C1"));

            // Sayfanın üst kısmında başlık yazısının görünmemesi için boş bırakıyoruz.
            // Geri oku bu durumda bile görünmeye devam edecektir.
            Title = "";
            BackgroundColor = Color.FromHex("#29443F");

            var title = new Label { Text = "HealthBite", FontSize = 32, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#E4C2C1"), HorizontalOptions = LayoutOptions.Center };
            var slogan = new Label { Text = "Fuel Your Body, Nourish Your Life.", FontSize = 14, TextColor = Colors.WhiteSmoke, HorizontalOptions = LayoutOptions.Center };

            idEntry = CreateStyledEntry("ID Number", Keyboard.Numeric);
            passwordEntry = CreateStyledEntry("Password", isPassword: true);

            var forgotPasswordLabel = new Label
            {
                Text = "Forgot your password?",
                TextColor = Color.FromHex("#E4C2C1"),
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 15),
                TextDecorations = TextDecorations.Underline
            };
            var forgotPasswordTap = new TapGestureRecognizer();
            // DEĞİŞİKLİK: Rota metin olarak verildi
            forgotPasswordTap.Tapped += async (s, e) => await Shell.Current.GoToAsync("ForgotPasswordEnterIdPage");
            forgotPasswordLabel.GestureRecognizers.Add(forgotPasswordTap);

            var loginBtn = new Button { Text = "LOG IN", BackgroundColor = Color.FromHex("#E4C2C1"), TextColor = Colors.White, FontAttributes = FontAttributes.Bold, HeightRequest = 50, CornerRadius = 20, Shadow = new Shadow { Brush = Brush.Black, Offset = new Point(2, 2), Radius = 4, Opacity = 0.3f } };
            loginBtn.Clicked += OnLoginClicked;

            var signUpPromptLabel = new Label { HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 15, 0, 0) };
            var formattedText = new FormattedString();
            formattedText.Spans.Add(new Span { Text = "Don't have an account? ", TextColor = Colors.WhiteSmoke, FontSize = 14 });
            formattedText.Spans.Add(new Span { Text = "Register", TextColor = Color.FromHex("#E4C2C1"), FontAttributes = FontAttributes.Bold, TextDecorations = TextDecorations.Underline, FontSize = 14 });
            signUpPromptLabel.FormattedText = formattedText;
            var signUpTapGesture = new TapGestureRecognizer();
            // DEĞİŞİKLİK: Rota metin olarak verildi
            signUpTapGesture.Tapped += async (s, e) => await Shell.Current.GoToAsync("SignupPage");
            signUpPromptLabel.GestureRecognizers.Add(signUpTapGesture);

            errorLabel = new Label { TextColor = Colors.Red, FontSize = 12, IsVisible = false, HorizontalOptions = LayoutOptions.Center };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = new Thickness(30, 60, 30, 30),
                    Spacing = 20,
                    Children = { title, slogan, CreateLabeledField("ID Number", idEntry), CreateLabeledField("Password", passwordEntry), forgotPasswordLabel, loginBtn, signUpPromptLabel, errorLabel }
                }
            };
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string id = idEntry.Text?.Trim();
            string password = passwordEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(password)) { ShowError("Please enter your ID and password."); return; }
            if (!long.TryParse(id, out _)) { ShowError("ID must be numeric."); return; }

            var user = await Database.GetUserById(id);
            if (user == null || user.Password != password) { ShowError("Invalid ID info."); return; }

            SessionManager.SetCurrentUser(user);

            if (user.SelectedDietId.HasValue)
            {
                var diet = Database.GetAllDiets().FirstOrDefault(d => d.Id == user.SelectedDietId.Value);
                if (diet != null) SessionManager.SetCurrentDiet(diet);
            }
            if (user.SelectedTrainingId.HasValue)
            {
                var training = TrainingData.GetPredefinedTrainingPrograms().FirstOrDefault(p => p.Id == user.SelectedTrainingId.Value);
                if (training != null) SessionManager.SetCurrentTrainingProgram(training);
            }

            Application.Current.MainPage = new AppShell();
            // DEĞİŞİKLİK: Rota metin olarak verildi
            await Shell.Current.GoToAsync("//DashboardPage");
        }

        private Entry CreateStyledEntry(string placeholder, Keyboard keyboard = null, bool isPassword = false)
        {
            return new Entry { Placeholder = placeholder, PlaceholderColor = Color.FromArgb("#666666"), Keyboard = keyboard ?? Keyboard.Default, IsPassword = isPassword, BackgroundColor = Colors.Transparent, TextColor = Colors.Black, HeightRequest = 45 };
        }

        private View CreateLabeledField(string labelText, Entry entry)
        {
            return new StackLayout { Spacing = 6, Children = { new Label { Text = labelText, TextColor = Color.FromHex("#E4C2C1"), FontAttributes = FontAttributes.Bold }, new Frame { CornerRadius = 12, Padding = new Thickness(12, 8), BackgroundColor = Colors.White, Content = entry, HasShadow = true } } };
        }

        private void ShowError(string message)
        {
            errorLabel.Text = message;
            errorLabel.IsVisible = true;
        }
    }
}