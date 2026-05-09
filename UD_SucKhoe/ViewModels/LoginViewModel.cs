using UD_SucKhoe.Services.Database;

namespace UD_SucKhoe.ViewModels;

public class LoginViewModel
{
    private readonly DatabaseService _databaseService;

    public LoginViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<(bool Success, string Message)> Login(string emailOrUsername, string password)
    {
        if (string.IsNullOrEmpty(emailOrUsername) || string.IsNullOrEmpty(password))
        {
            return (false, "Vui lòng nhập đầy đủ thông tin đăng nhập!");
        }

        try
        {
            var user = await _databaseService.ValidateUser(emailOrUsername, password);

            if (user != null)
            {
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("UserId", user.UserID);
                Preferences.Set("Email", user.Email);
                Preferences.Set("FullName", user.FullName);

                return (true, $"Chào mừng {user.FullName}!");
            }

            return (false, "Email hoặc mật khẩu không đúng!");
        }
        catch (Exception ex)
        {
            return (false, $"Đã xảy ra lỗi: {ex.Message}");
        }
    }
}