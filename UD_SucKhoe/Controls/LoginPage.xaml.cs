using UD_SucKhoe.Services;

namespace UD_SucKhoe;

public partial class LoginPage : ContentPage
{
    private readonly SqlServerDatabaseService _databaseService;

    public LoginPage()
    {
        InitializeComponent();
        _databaseService = new SqlServerDatabaseService();
    }

    // Đăng nhập
    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        var emailOrUsername = UsernameEntry.Text?.Trim();
        var password = PasswordEntry.Text?.Trim();

        if (string.IsNullOrEmpty(emailOrUsername) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Lỗi", "Vui lòng nhập đầy đủ thông tin đăng nhập!", "OK");
            return;
        }

        var user = await _databaseService.LoginUser(emailOrUsername, password);

        if (user != null)
        {
            await DisplayAlert("Thành công", $"Chào mừng {user.FullName}!", "OK");

            // Lưu thông tin đăng nhập
            Preferences.Set("IsLoggedIn", true);
            Preferences.Set("UserId", user.UserID);
            Preferences.Set("Email", user.Email);
            Preferences.Set("FullName", user.FullName);

            await Navigation.PopModalAsync();
        }
        else
        {
            await DisplayAlert("Lỗi", "Email hoặc mật khẩu không đúng!", "OK");
        }
    }

    // Nhấn "Quên mật khẩu"
    private async void OnForgotPasswordTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Thông báo", "Tính năng quên mật khẩu sẽ sớm được cập nhật.", "OK");
    }

    // Nhấn "Đăng ký"
    private async void OnRegisterTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new RegisterPage());
    }

    // Nhấn "Quay lại"
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
