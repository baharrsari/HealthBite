using HealthBite.Data;
using HealthBite.Models;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;

namespace HealthBite.Views
{
    public class TrainingProgramsPage : ContentPage
    {
        // Renk Paleti - HealthBite Genel Estetiğine Uygun
        private readonly Color DarkPrimaryColor = Color.FromRgb(25, 54, 48);
        private readonly Color AccentColorPink = Color.FromRgb(228, 178, 179);
        private readonly Color LightBackgroundColor = Color.FromRgb(246, 247, 249);
        private readonly Color CardBackgroundColor = Colors.White;
        private readonly Color TextColorSubtle = Colors.DarkSlateGray;
        private readonly Color TextColorLight = Colors.Gray;

        public TrainingProgramsPage()
        {
            Title = "";
            BackgroundColor = LightBackgroundColor;

            Shell.SetBackgroundColor(this, LightBackgroundColor);
            Shell.SetForegroundColor(this, DarkPrimaryColor);
            
            // --- YENİ EKLENEN BAŞLIK ---
            var pageTitleLabel = new Label
            {
                Text = "Antrenman Programlarını Keşfet",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                TextColor = DarkPrimaryColor,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 30, 0, 20)
            };
            // --------------------------

            var allPrograms = TrainingData.GetPredefinedTrainingPrograms();
            var groupedPrograms = allPrograms.GroupBy(p => p.Location)
                                             .Select(g => new TrainingProgramGroup(g.Key, g.ToList()))
                                             .ToList();

            var groupHeaderTemplate = new DataTemplate(() =>
            {
                var label = new Label
                {
                    FontSize = 24,
                    FontAttributes = FontAttributes.Bold,
                    Padding = new Thickness(0, 30, 0, 15),
                    TextColor = DarkPrimaryColor,
                    HorizontalOptions = LayoutOptions.Start
                };
                label.SetBinding(Label.TextProperty, "CategoryTitle");
                return label;
            });

            var itemTemplate = new DataTemplate(() =>
            {
                var titleLabel = new Label
                {
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = DarkPrimaryColor,
                    Margin = new Thickness(0, 0, 0, 2)
                };
                titleLabel.SetBinding(Label.TextProperty, "Title");

                var descriptionLabel = new Label
                {
                    FontSize = 14,
                    TextColor = TextColorSubtle,
                    MaxLines = 3,
                    LineBreakMode = LineBreakMode.WordWrap,
                    Margin = new Thickness(0, 0, 0, 5)
                };
                descriptionLabel.SetBinding(Label.TextProperty, "Description");

                var durationLabel = new Label
                {
                    FontSize = 12,
                    TextColor = TextColorLight,
                    FontAttributes = FontAttributes.Italic
                };
                durationLabel.SetBinding(Label.TextProperty, "DurationInWeeks", stringFormat: "{0} Haftalık Program");
                
                var cardContent = new VerticalStackLayout
                {
                    Padding = 15,
                    Spacing = 0,
                    Children = { titleLabel, descriptionLabel, durationLabel }
                };

                var frame = new Frame
                {
                    CornerRadius = 12,
                    Padding = 0,
                    Margin = new Thickness(0),
                    HasShadow = false,
                    BorderColor = AccentColorPink,
                    BackgroundColor = CardBackgroundColor,
                    Content = cardContent
                };

                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += async (s, e) =>
                {
                    if (s is Frame tappedFrame && tappedFrame.BindingContext is TrainingProgramModel program)
                    {
                        await Shell.Current.GoToAsync($"{nameof(TrainingProgramDetailPage)}?programId={program.Id}");
                    }
                };
                frame.GestureRecognizers.Add(tapGesture);

                return frame;
            });

            var collectionView = new CollectionView
            {
                ItemsSource = groupedPrograms,
                IsGrouped = true,
                ItemTemplate = itemTemplate,
                GroupHeaderTemplate = groupHeaderTemplate,
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 15
                }
            };
            
            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = new Thickness(20, 0, 20, 20),
                    // Başlık (pageTitleLabel) listenin üzerine eklendi
                    Children = { pageTitleLabel, collectionView }
                }
            };
        }
    }

    public class TrainingProgramGroup : List<TrainingProgramModel>
    {
        public string CategoryTitle { get; private set; }

        public TrainingProgramGroup(string categoryTitle, List<TrainingProgramModel> programs) : base(programs)
        {
            CategoryTitle = categoryTitle;
        }
    }
}