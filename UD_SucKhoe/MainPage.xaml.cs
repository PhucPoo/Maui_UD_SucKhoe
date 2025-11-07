namespace UD_SucKhoe;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    private async void OnMenuTapped(object sender, EventArgs e)
    {
        // Hiển thị menu
        await DisplayAlert("Menu", "Menu clicked", "OK");
    }

    private async void OnLoginTapped(object sender, EventArgs e)
    {
        // Thử cách navigation Modal thay vì Shell navigation
        try
        {
            var loginPage = new LoginPage();

            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow?.Page != null)
            {
                await currentWindow.Page.Navigation.PushModalAsync(loginPage);
            }
            else
            {
                await DisplayAlert("Lỗi", "Unable to navigate: Current window or page is null.", "OK");
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            await DisplayAlert("Lỗi", ex.Message, "OK");
        }
    }

    // Category Cards Events
    private async void OnActivityTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Health Data", "Activity tapped!", "OK");
    }

    private async void OnMindfulnessTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Health Data", "Mindfulness tapped!", "OK");
    }

    private async void OnNutritionTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Health Data", "Nutrition tapped!", "OK");
    }

    private async void OnSleepTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Health Data", "Sleep tapped!", "OK");
    }

    // Menu Items Events
    private async void OnBodyMeasurementsTapped(object sender, EventArgs e)
    {
        try
        {
            var bodyMeasurements = new BodyMeasurementsPage();

            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow?.Page != null)
            {
                await currentWindow.Page.Navigation.PushModalAsync(bodyMeasurements);
            }
            else
            {
                await DisplayAlert("Lỗi", "Unable to navigate: Current window or page is null.", "OK");
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            await DisplayAlert("Lỗi", ex.Message, "OK");
        }
    }

    private async void OnHealthRecordsTapped(object sender, EventArgs e)
    {
        try
        {
            var healthRecords = new HealthRecordsPage();

            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow?.Page != null)
            {
                await currentWindow.Page.Navigation.PushAsync(healthRecords);
            }
            else
            {
                await DisplayAlert("Lỗi", "Unable to navigate: Current window or page is null.", "OK");
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            await DisplayAlert("Lỗi", ex.Message, "OK");
        }
    }

    private async void OnHeartTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Health Data", "Heart selected!", "OK");
    }

    private async void OnReproductiveHealthTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Health Data", "Reproductive Health selected!", "OK");
    }

    // Tab Bar Events
    private async void OnSummaryTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Tab Bar", "Summary tab tapped!", "OK");
    }

    private async void OnBrowseTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Tab Bar", "Browse tab tapped! (Current)", "OK");
    }

    private async void OnSharingTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Tab Bar", "Sharing tab tapped!", "OK");
    }

    private async void OnMedicalIDTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Tab Bar", "Medical ID tab tapped!", "OK");
    }
}