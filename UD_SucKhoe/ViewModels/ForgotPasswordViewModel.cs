using System.Text.RegularExpressions;
using UD_SucKhoe.Services.Database;

namespace UD_SucKhoe.ViewModels;

public class ForgotPasswordViewModel
{
    private readonly DatabaseService _databaseService;

    public ForgotPasswordViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<(bool Success, string Message)> CheckEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return (false, "Vui lòng nhập email");
        }

        if (!IsValidEmail(email))
        {
            return (false, "Email không hợp lệ");
        }

        try
        {
            bool exists = await _databaseService.CheckEmailExists(email);

            if (!exists)
            {
                return (false, "Email không tồn tại trong hệ thống");
            }

            return (true, "Email hợp lệ! Bạn có thể đổi mật khẩu mới.");
        }
        catch (Exception ex)
        {
            return (false, $"Đã xảy ra lỗi: {ex.Message}");
        }
    }

    private bool IsValidEmail(string email)
    {
        var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }
}