using UD_SucKhoe.Services;

namespace UD_SucKhoe.ViewModels;

public class RegisterViewModel
{
    private readonly SqlServerDatabaseService _databaseService;

    public RegisterViewModel(SqlServerDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    // Test DB
    public async Task<bool> TestConnection()
    {
        return await _databaseService.TestConnection();
    }

    // Đăng ký
    public async Task<(bool Success, string Message)> Register(
        string fullName,
        string email,
        string password,
        string confirmPassword)
    {
        // Validate
        if (string.IsNullOrEmpty(fullName) ||
            string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password))
        {
            return (false, "Vui lòng điền đầy đủ thông tin!");
        }

        if (password != confirmPassword)
        {
            return (false, "Mật khẩu xác nhận không khớp!");
        }

        if (password.Length < 6)
        {
            return (false, "Mật khẩu phải có ít nhất 6 ký tự!");
        }

        try
        {
            // Hash password
            string hashedPassword = _databaseService.HashPassword(password);

            var success = await _databaseService.RegisterUser(fullName, email, hashedPassword);

            if (success)
            {
                return (true, "Đăng ký tài khoản thành công!");
            }

            return (false, "Email đã tồn tại hoặc có lỗi xảy ra!");
        }
        catch (Exception ex)
        {
            return (false, $"Lỗi hệ thống: {ex.Message}");
        }
    }
}