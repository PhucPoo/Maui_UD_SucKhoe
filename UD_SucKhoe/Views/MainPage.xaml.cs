using CommunityToolkit.Mvvm.Messaging;


namespace UD_SucKhoe;

public partial class MainPage : ContentPage
{
    private double _latestBMI;
    public MainPage()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<LoginMessage>(this, (r, m) =>
        {
            UpdateUserUI();
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateUserUI();
        _latestBMI = Preferences.Get("LatestBMI", 0.0);
    }

    private void UpdateUserUI()
    {
        bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

        if (isLoggedIn)
        {
            AvatarBorder.IsVisible = true;
            LoginButton.IsVisible = false;

            string avatarUrl = Preferences.Get("AvatarUrl", string.Empty);

            if (!string.IsNullOrEmpty(avatarUrl))
            {
                AvatarImage.Source = avatarUrl;
            }
            else
            {
                AvatarImage.Source = "default_avatar.png";
            }
        }
        else
        {
            AvatarBorder.IsVisible = false;
            LoginButton.IsVisible = true;
        }
    }

    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new LoginPage());
    }

    private async void OnAvatarTapped(object sender, EventArgs e)
    {
        string fullName = Preferences.Get("FullName", "Người dùng");

        string action = await DisplayActionSheet(
            fullName,
            "Hủy",
            null,
            "Xem hồ sơ",
            "Đăng xuất"
        );

        if (action == "Đăng xuất")
        {
            Preferences.Clear();
            UpdateUserUI();
            await DisplayAlert("Thông báo", "Đã đăng xuất thành công!", "OK");
        }
        else if (action == "Xem hồ sơ")
        {
            await Navigation.PushAsync(new ProfilePage());
        }
    }

    private async void OnMenuTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MenuPage));
    }


    private async void OnActivityTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ActivityPage));
    }

    private async void OnMindfulnessTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Sức khỏe tinh thần", "Chức năng đang được phát triển", "OK");
    }

    private async void OnNutritionTapped(object sender, EventArgs e)
    {
        _latestBMI = Preferences.Get("LatestBMI", 0.0);
        if (_latestBMI == 0)
        {
            await DisplayAlert("Thông báo", "Vui lòng tính BMI trước!", "OK");
            return;
        }


        await Shell.Current.GoToAsync(nameof(NutritionPage));
    }

    private async void OnSleepTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Giấc ngủ", "Chức năng đang được phát triển", "OK");
    }

    private async void OnBodyMeasurementsTapped(object sender, EventArgs e)
    {
        try
        {
            var bodyMeasurements = new BodyMeasurementsPage();

            bodyMeasurements.BMICalculated += (bmi) =>
            {
                _latestBMI = bmi;
                Preferences.Set("LatestBMI", bmi);
                Dispatcher.Dispatch(async () =>
                {
                    await DisplayAlert("Đã tính BMI", $"BMI của bạn: {bmi:F2}", "OK");
                });
            };

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
        await DisplayAlert("Trái tim", "Chức năng đang được phát triển", "OK");
    }

    private async void OnReproductiveHealthTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Sức khỏe sinh sản", "Chức năng đang được phát triển", "OK");
    }

    private async void OnSummaryTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Tóm tắt", "Chức năng đang được phát triển", "OK");
    }

    private async void OnBrowseTapped(object sender, EventArgs e)
    {
        await DisplayAlert("", "", "OK");
    }

    private async void OnSharingTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Chia sẻ", "Chức năng đang được phát triển", "OK");
    }

    private async void OnMedicalIDTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Thông tin y tế", "Chức năng đang được phát triển", "OK");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        WeakReferenceMessenger.Default.Unregister<LoginMessage>(this);
    }
}

public class LoginMessage
{
}