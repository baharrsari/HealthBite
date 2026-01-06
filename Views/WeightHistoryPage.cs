using HealthBite.Data;
using HealthBite.Models;
using HealthBite.Services;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Charts;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HealthBite.Views
{
    public class WeightHistoryPage : ContentPage
    {
        private Entry weightEntry;
        private DatePicker datePicker;
        private Button saveButton;
        private CollectionView historyCollectionView;
        private ObservableCollection<WeightEntryModel> weightHistory = new ObservableCollection<WeightEntryModel>();
        
        private SfCartesianChart chart;
        private StackLayout pageContentLayout;
        private Label pageTitle;

        // Renkler
        private readonly Color DarkPrimaryColor = Color.FromRgb(25, 54, 48);
        private readonly Color AccentColorPink = Color.FromRgb(228, 178, 179);
        private readonly Color LightBackgroundColor = Color.FromRgb(246, 247, 249);
        private readonly Color CardBackgroundColor = Colors.White;
        private readonly Color TextColorOnLight = Color.FromRgb(25, 54, 48);
        private readonly Color SubtleGrayText = Color.FromRgb(120, 120, 120);

        public WeightHistoryPage()
        {
            Title = "Kilo Geçmişi"; 
            BackgroundColor = LightBackgroundColor;
            
            pageTitle = new Label { Text = "Kilo Takibi", FontSize = 28, FontAttributes = FontAttributes.Bold, TextColor = DarkPrimaryColor, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 30, 0, 20) };
            weightEntry = new Entry { Placeholder = "Kilonuzu girin (kg)", Keyboard = Keyboard.Numeric, BackgroundColor = LightBackgroundColor, TextColor = TextColorOnLight };
            datePicker = new DatePicker { Date = DateTime.Today, Format = "D", TextColor = TextColorOnLight };
            saveButton = new Button { Text = "Kaydet", BackgroundColor = DarkPrimaryColor, TextColor = Colors.White, FontAttributes = FontAttributes.Bold, CornerRadius = 20, HeightRequest = 50, Margin = new Thickness(0, 10, 0, 0) };
            saveButton.Clicked += OnSaveButtonClicked;
            
            var entryCard = new Frame { CornerRadius = 15, Padding = 20, Margin = new Thickness(20, 0), BackgroundColor = CardBackgroundColor, BorderColor = AccentColorPink,HasShadow = false, Content = new StackLayout { Spacing = 15, Children = { new Label { Text = "Yeni Kilo Ekle", FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = DarkPrimaryColor }, weightEntry, datePicker, saveButton } } };
            var historyTitle = new Label { Text = "Geçmiş Kayıtlar", FontSize = 22, FontAttributes = FontAttributes.Bold, TextColor = DarkPrimaryColor, Margin = new Thickness(20, 30, 20, 10) };
            
            historyCollectionView = new CollectionView 
            { 
                ItemsSource = weightHistory, 
                ItemTemplate = new DataTemplate(() => 
                { 
                    var dateLabel = new Label { FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = DarkPrimaryColor };
                    // DÜZELTME: Tarih formatı 'yyyy' olarak düzeltildi.
                    dateLabel.SetBinding(Label.TextProperty, new Binding("DateRecorded", stringFormat: "{0:dd MMMM yyyy}")); 
                    
                    var weightLabel = new Label { FontSize = 16, TextColor = SubtleGrayText, HorizontalOptions = LayoutOptions.End }; 
                    weightLabel.SetBinding(Label.TextProperty, new Binding("WeightInKg", stringFormat: "{0:F1} kg")); 
                    
                    var grid = new Grid { Padding = new Thickness(0, 10), ColumnDefinitions = { new ColumnDefinition { Width = GridLength.Star }, new ColumnDefinition { Width = GridLength.Auto } } }; 
                    grid.Add(dateLabel, 0); 
                    grid.Add(weightLabel, 1); 
                    
                    return new Frame { Content = grid, CornerRadius = 10, Padding = new Thickness(15), Margin = new Thickness(0, 5), BackgroundColor = CardBackgroundColor, BorderColor = AccentColorPink }; 
                }), 
                EmptyView = new Label { Text = "Henüz kilo kaydı yok.", HorizontalOptions = LayoutOptions.Center, Margin = 20, TextColor = SubtleGrayText } 
            };
            
            pageContentLayout = new StackLayout { Padding = new Thickness(0, 0, 0, 20), Children = { pageTitle, entryCard, historyTitle, historyCollectionView } };
            Content = new ScrollView { Content = pageContentLayout };
        }

        protected override async void OnAppearing() { base.OnAppearing(); await LoadWeightHistory(); }

        private async Task LoadWeightHistory()
        {
            var user = SessionManager.GetCurrentUser();
            if (user == null) return;
            var entries = await Database.GetWeightEntriesForUser(user.Id);
            weightHistory.Clear();
            foreach (var entry in entries.OrderByDescending(e => e.DateRecorded)) { weightHistory.Add(entry); }
            UpdateChart(entries);
        }
        
        private void UpdateChart(List<WeightEntryModel> entries)
        {
            if (chart != null && pageContentLayout.Children.Contains(chart))
            {
                pageContentLayout.Children.Remove(chart);
            }

            if (entries == null || entries.Count < 2) return;

            var sortedEntries = entries.OrderBy(e => e.DateRecorded).ToList();

            chart = new SfCartesianChart
            {
                HeightRequest = 220,
                BackgroundColor = Colors.Transparent
            };
    
            var minWeight = sortedEntries.Min(e => e.WeightInKg);
            var maxWeight = sortedEntries.Max(e => e.WeightInKg);
            var buffer = 5;

            var yAxis = new NumericalAxis
            {
                LabelStyle = new ChartAxisLabelStyle { TextColor = SubtleGrayText },
                MajorTickStyle = new ChartAxisTickStyle { Stroke = Colors.Transparent },
                AxisLineStyle = new ChartLineStyle { Stroke = Colors.Transparent },
                ShowMajorGridLines = true,
                MajorGridLineStyle = new ChartLineStyle { Stroke = Color.FromRgb(230, 230, 230), StrokeWidth = 0.5f },
                Minimum = minWeight - buffer,
                Maximum = maxWeight + buffer
            };
            chart.YAxes.Add(yAxis);

            var xAxis = new DateTimeAxis
            {
                LabelStyle = new ChartAxisLabelStyle { TextColor = SubtleGrayText, LabelFormat = "dd MMM" },
                MajorTickStyle = new ChartAxisTickStyle { Stroke = Colors.Transparent },
                AxisLineStyle = new ChartLineStyle { Stroke = SubtleGrayText, StrokeWidth = 0.5f },
                RangePadding = DateTimeRangePadding.Additional
            };
            chart.XAxes.Add(xAxis);

            var series = new SplineSeries
            {
                ItemsSource = sortedEntries,
                XBindingPath = "DateRecorded",
                YBindingPath = "WeightInKg",
                Fill = AccentColorPink,
                StrokeWidth = 4,
                ShowDataLabels = true,
                ShowMarkers = true,
                DataLabelSettings = new CartesianDataLabelSettings
                {
                    LabelStyle = new ChartDataLabelStyle
                    {
                        TextColor = DarkPrimaryColor,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 12
                    },
                    UseSeriesPalette = false,
                    LabelPlacement = DataLabelPlacement.Outer
                },
                MarkerSettings = new ChartMarkerSettings
                {
                    Type = ShapeType.Circle,
                    Stroke = DarkPrimaryColor,
                    StrokeWidth = 2.5,
                    Fill = CardBackgroundColor,
                    Width = 12,
                    Height = 12
                }
            };

            chart.Series.Add(series);
            
            pageContentLayout.Children.Insert(1, chart);
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var user = SessionManager.GetCurrentUser();
            if (user == null) { await DisplayAlert("Hata", "Kullanıcı bulunamadı. Lütfen tekrar giriş yapın.", "Tamam"); return; }
            if (string.IsNullOrWhiteSpace(weightEntry.Text) || !double.TryParse(weightEntry.Text, out double weight)) { await DisplayAlert("Geçersiz Giriş", "Lütfen geçerli bir kilo değeri girin.", "Tamam"); return; }
            var newEntry = new WeightEntryModel { UserId = user.Id, DateRecorded = datePicker.Date, WeightInKg = weight };
            await Database.AddWeightEntry(newEntry);
            
            user.Weight = weight;
            await Database.UpdateUser(user); 
            SessionManager.SetCurrentUser(user);
            
            weightEntry.Text = string.Empty;
            await LoadWeightHistory();
            await DisplayAlert("Başarılı", "Kilonuz başarıyla kaydedildi.", "Harika!");
        }
    }
}