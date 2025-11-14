using UD_SucKhoe.Services;

namespace UD_SucKhoe;

public partial class ResetPasswordPage : ContentPage
{
    private readonly string _email;
    private readonly DatabaseService _databaseService;

    public ResetPasswordPage(string email)
    {
        InitializeComponent();
        _email = email;
        _databaseService = new DatabaseService();
    }

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        // Lấy giá trị từ các Entry
        var newPass = NewPasswordEntry.Text?.Trim();
        var confirm = ConfirmPasswordEntry.Text?.Trim();

        // Validate input
        if (string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(confirm))
        {
            await DisplayAlert("Lỗi", "Vui lòng nhập đầy đủ thông tin", "OK");
            return;
        }

        // Kiểm tra độ dài mật khẩu
        if (newPass.Length < 6)
        {
            await DisplayAlert("Lỗi", "Mật khẩu phải có ít nhất 6 ký tự", "OK");
            return;
        }

        // Kiểm tra mật khẩu xác nhận
        if (newPass != confirm)
        {
            await DisplayAlert("Lỗi", "Mật khẩu xác nhận không khớp", "OK");
            return;
        }

        // Hiển thị loading
        var loadingIndicator = ShowLoading();

        try
        {
            // Cập nhật mật khẩu (DatabaseService sẽ tự động hash)
            bool success = await _databaseService.UpdateUserPassword(_email, newPass);

            HideLoading(loadingIndicator);

            if (success)
            {
                await DisplayAlert("Thành công", "Mật khẩu đã được thay đổi thành công!", "OK");

                // Chuyển về trang đăng nhập
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                await DisplayAlert("Lỗi", "Không thể cập nhật mật khẩu. Vui lòng thử lại.", "OK");
            }
        }
        catch (Exception ex)
        {
            HideLoading(loadingIndicator);
            await DisplayAlert("Lỗi", $"Đã xảy ra lỗi: {ex.Message}", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        // Quay lại trang trước hoặc trang đăng nhập
        bool answer = await DisplayAlert("Xác nhận", "Bạn có muốn hủy đổi mật khẩu?", "Có", "Không");

        if (answer)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }

    // Hiển thị loading indicator
    private ActivityIndicator ShowLoading()
    {
        var indicator = new ActivityIndicator
        {
            IsRunning = true,
            IsVisible = true,
            Color = Colors.Blue
        };

        // Thêm vào layout nếu cần
        return indicator;
    }

    // Ẩn loading indicator
    private void HideLoading(ActivityIndicator indicator)
    {
        if (indicator != null)
        {
            indicator.IsRunning = false;
            indicator.IsVisible = false;
        }
    }
}