using UD_SucKhoe.Services;

namespace UD_SucKhoe;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public LoginPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
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

        try
        {
            // Hiển thị loading (optional)
            UsernameEntry.IsEnabled = false;
            PasswordEntry.IsEnabled = false;

            Console.WriteLine($"[Login] Attempting login for: {emailOrUsername}");

            // Sử dụng ValidateUser - tự động hash password và so sánh
            var user = await _databaseService.ValidateUser(emailOrUsername, password);

            if (user != null)
            {
                Console.WriteLine($"[Login] ✓ Login successful for user: {user.FullName}");

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
                Console.WriteLine($"[Login] ✗ Login failed for: {emailOrUsername}");
                await DisplayAlert("Lỗi", "Email hoặc mật khẩu không đúng!", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Login] ✗ Error: {ex.Message}");
            await DisplayAlert("Lỗi", $"Đã xảy ra lỗi: {ex.Message}", "OK");
        }
        finally
        {
            // Bật lại các control
            UsernameEntry.IsEnabled = true;
            PasswordEntry.IsEnabled = true;
        }
    }

    // Nhấn "Quên mật khẩu"
    private async void OnForgotPasswordTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ForgotPasswordPage());
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