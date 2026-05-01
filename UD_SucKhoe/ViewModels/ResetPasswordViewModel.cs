using UD_SucKhoe.Services.Database;

namespace UD_SucKhoe.ViewModels;

public class ResetPasswordViewModel
{
    private readonly DatabaseService _databaseService;

    public ResetPasswordViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<(bool Success, string Message)> ResetPassword(
        string email,
        string newPass,
        string confirmPass)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(confirmPass))
        {
            return (false, "Vui lòng nhập đầy đủ thông tin");
        }

        if (newPass.Length < 6)
        {
            return (false, "Mật khẩu phải có ít nhất 6 ký tự");
        }

        if (newPass != confirmPass)
        {
            return (false, "Mật khẩu xác nhận không khớp");
        }

        try
        {
            bool success = await _databaseService.UpdateUserPassword(email, newPass);

            if (success)
            {
                return (true, "Mật khẩu đã được thay đổi thành công!");
            }

            return (false, "Không thể cập nhật mật khẩu. Vui lòng thử lại.");
        }
        catch (Exception ex)
        {
            return (false, $"Đã xảy ra lỗi: {ex.Message}");
        }
    }
}