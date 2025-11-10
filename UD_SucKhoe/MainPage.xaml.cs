using CommunityToolkit.Mvvm.Messaging;

namespace UD_SucKhoe;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        // Đăng ký nhận message khi đăng nhập thành công
        WeakReferenceMessenger.Default.Register<LoginMessage>(this, (r, m) =>
        {
            UpdateUserUI();
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Cập nhật UI mỗi khi màn hình hiển thị
        UpdateUserUI();
    }

    private void UpdateUserUI()
    {
        bool isLoggedIn = Preferences.Get("IsLoggedIn", false);

        if (isLoggedIn)
        {
            // Hiển thị avatar, ẩn nút đăng nhập
            AvatarBorder.IsVisible = true;
            LoginButton.IsVisible = false;

            // Lấy đường dẫn avatar
            string avatarUrl = Preferences.Get("AvatarUrl", string.Empty);

            if (!string.IsNullOrEmpty(avatarUrl))
            {
                AvatarImage.Source = avatarUrl;
            }
            else
            {
                // Hiển thị avatar mặc định
                AvatarImage.Source = "default_avatar.png";
            }
        }
        else
        {
            // Ẩn avatar, hiển thị nút đăng nhập
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
            // Xóa thông tin đăng nhập
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
        await DisplayAlert("Menu", "Chức năng menu đang được phát triển", "OK");
    }

    private async void OnActivityTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Thể dục", "Chức năng đang được phát triển", "OK");
    }

    private async void OnMindfulnessTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Sức khỏe tinh thần", "Chức năng đang được phát triển", "OK");
    }

    private async void OnNutritionTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Dinh dưỡng", "Chức năng đang được phát triển", "OK");
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
        // Đây là trang hiện tại
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
        // Hủy đăng ký khi page bị dispose
        WeakReferenceMessenger.Default.Unregister<LoginMessage>(this);
    }
}

// Define the message class
public class LoginMessage
{
}