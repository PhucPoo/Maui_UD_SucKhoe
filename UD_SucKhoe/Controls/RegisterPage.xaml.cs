using UD_SucKhoe.Services;

namespace UD_SucKhoe;

public partial class RegisterPage : ContentPage
{
    private readonly SqlServerDatabaseService _databaseService;

    public RegisterPage()
    {
        InitializeComponent();
        _databaseService = new SqlServerDatabaseService();

        // Kiểm tra kết nối DB khi khởi tạo
        TestDatabaseConnection();
    }

    private async void TestDatabaseConnection()
    {
        var isConnected = await _databaseService.TestConnection();
        if (!isConnected)
        {
            await DisplayAlert("Cảnh báo", "Không thể kết nối đến cơ sở dữ liệu!", "OK");
        }
    }

    // Sự kiện nút "Đăng ký"
    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        var fullName = FullNameEntry.Text?.Trim();
        var email = EmailEntry.Text?.Trim();
        var password = PasswordEntry.Text;
        var confirmPassword = ConfirmPasswordEntry.Text;

        // Kiểm tra nhập liệu
        if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Lỗi", "Vui lòng điền đầy đủ thông tin!", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Lỗi", "Mật khẩu xác nhận không khớp!", "OK");
            return;
        }

        if (password.Length < 6)
        {
            await DisplayAlert("Lỗi", "Mật khẩu phải có ít nhất 6 ký tự!", "OK");
            return;
        }

        // Gọi hàm đăng ký
        var success = await _databaseService.RegisterUser(fullName, email, password);

        if (success)
        {
            await DisplayAlert("Thành công", "Đăng ký tài khoản thành công!", "OK");
            await Navigation.PopModalAsync();
        }
        else
        {
            await DisplayAlert("Lỗi", "Email đã tồn tại hoặc có lỗi xảy ra!", "OK");
        }
    }

    // Sự kiện "Quay lại"
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
