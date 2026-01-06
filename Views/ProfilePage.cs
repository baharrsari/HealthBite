using HealthBite.Models;
using HealthBite.Data;
using HealthBite.Services;
using Microsoft.Maui.Controls;

namespace HealthBite.Views;

public class ProfilePage : ContentPage
{
    Entry heightEntry, weightEntry, ageEntry;
    Picker bloodTypePicker;
    Label titleLabel;
    Button saveButton;

    public ProfilePage()
    {
        BackgroundColor = Color.FromHex("#29443F");

        titleLabel = new Label
        {
            Text = "Edit Your Profile",
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Color.FromHex("#E4C2C1")
        };

        heightEntry = CreateStyledEntry("Height (cm)");
        weightEntry = CreateStyledEntry("Weight (kg)");
        ageEntry = CreateStyledEntry("Age");

        bloodTypePicker = new Picker
        {
            Title = "Select Blood Type",
            TextColor = Colors.Black,
            ItemsSource = new List<string>
            {
                "0 Rh +", "0 Rh -",
                "A Rh +", "A Rh -",
                "B Rh +", "B Rh -",
                "AB Rh +", "AB Rh -"
            }
        };

        saveButton = new Button
        {
            Text = "Save Changes",
            BackgroundColor = Color.FromHex("#E4C2C1"),
            TextColor = Colors.White,
            FontAttributes = FontAttributes.Bold,
            HeightRequest = 50,
            CornerRadius = 20,
            Shadow = new Shadow
            {
                Brush = Brush.Black,
                Offset = new Point(2, 2),
                Radius = 4,
                Opacity = 0.3f
            }
        };
        saveButton.Clicked += SaveButton_Clicked;

        Content = new ScrollView
        {
            Content = new StackLayout
            {
                Padding = new Thickness(30, 60, 30, 30),
                Spacing = 25,
                Children = {
                    titleLabel,
                    CreateLabeledField("Height", heightEntry),
                    CreateLabeledField("Weight", weightEntry),
                    CreateLabeledField("Age", ageEntry),
                    CreateLabeledPicker("Blood Type", bloodTypePicker),
                    saveButton
                }
            }
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var user = SessionManager.GetCurrentUser();
        if (user != null)
        {
            heightEntry.Text = user.Height > 0 ? user.Height.ToString() : "";
            weightEntry.Text = user.Weight > 0 ? user.Weight.ToString() : "";
            ageEntry.Text = user.Age > 0 ? user.Age.ToString() : "";

            if (!string.IsNullOrEmpty(user.BloodType) &&
                bloodTypePicker.ItemsSource.Contains(user.BloodType))
            {
                bloodTypePicker.SelectedItem = user.BloodType;
            }
        }
    }

    private Entry CreateStyledEntry(string placeholder)
    {
        return new Entry
        {
            Placeholder = placeholder,
            PlaceholderColor = Color.FromArgb("#666666"),
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
                new Label
                {
                    Text = labelText,
                    TextColor = Color.FromHex("#E4C2C1"),
                    FontAttributes = FontAttributes.Bold
                },
                new Frame
                {
                    CornerRadius = 12,
                    Padding = new Thickness(12, 8),
                    BackgroundColor = Colors.White,
                    Content = entry,
                    HasShadow = true
                }
            }
        };
    }

    private View CreateLabeledPicker(string labelText, Picker picker)
    {
        return new StackLayout
        {
            Spacing = 6,
            Children =
            {
                new Label
                {
                    Text = labelText,
                    TextColor = Color.FromHex("#E4C2C1"),
                    FontAttributes = FontAttributes.Bold
                },
                new Frame
                {
                    CornerRadius = 12,
                    Padding = new Thickness(12, 8),
                    BackgroundColor = Colors.White,
                    Content = picker,
                    HasShadow = true
                }
            }
        };
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        var user = SessionManager.GetCurrentUser();
        if (user == null) return;

        if (double.TryParse(heightEntry.Text, out double h)) user.Height = h;
        if (double.TryParse(weightEntry.Text, out double w)) user.Weight = w;
        if (int.TryParse(ageEntry.Text, out int a)) user.Age = a;
        if (bloodTypePicker.SelectedItem != null)
            user.BloodType = bloodTypePicker.SelectedItem.ToString();

        Database.UpdateUser(user);
        await DisplayAlert("Success", "Your profile has been updated.", "OK");
    }
}
