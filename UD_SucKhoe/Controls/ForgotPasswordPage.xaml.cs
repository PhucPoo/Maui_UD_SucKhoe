using System.Text.RegularExpressions;
using UD_SucKhoe.Services;

namespace UD_SucKhoe;

public partial class ForgotPasswordPage : ContentPage
{
    public ForgotPasswordPage()
    {
        InitializeComponent();

    }

    private async void OnSendCodeButtonClicked(object sender, EventArgs e)
    {
        var email = EmailPhoneEntry.Text?.Trim();

        // Kiểm tra input rỗng
        if (string.IsNullOrWhiteSpace(email))
        {
            await DisplayAlert("Thông báo", "Vui lòng nhập email", "OK");
            return;
        }

        // Validate email
        if (!IsValidEmail(email))
        {
            await DisplayAlert("Lỗi", "Email không hợp lệ", "OK");
            return;
        }

        // Hiển thị loading
        SendCodeButton.IsEnabled = false;
        SendCodeButton.Text = "Đang kiểm tra...";

        try
        {
            // Kiểm tra email có tồn tại trong database không
            bool emailExists = await CheckEmailExistsInDatabase(email);

            if (!emailExists)
            {
                await DisplayAlert("Lỗi", "Email không tồn tại trong hệ thống", "OK");
                return;
            }

            // Email hợp lệ, chuyển sang trang đổi mật khẩu
            await DisplayAlert(
                "Thành công",
                "Email hợp lệ! Bạn có thể đổi mật khẩu mới.",
                "OK"
            );

            // Chuyển sang trang nhập mật khẩu mới
            await Navigation.PushModalAsync(new ResetPasswordPage(email));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", $"Đã xảy ra lỗi: {ex.Message}", "OK");
        }
        finally
        {
            SendCodeButton.IsEnabled = true;
            SendCodeButton.Text = "Xác nhận";
        }
    }

    // Validate email
    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }

    // Kiểm tra email trong database
    private async Task<bool> CheckEmailExistsInDatabase(string email)
    {
        try
        {
            var db = new DatabaseService();
            return await db.CheckEmailExists(email);
        }
        catch (Exception ex)
        {
            throw new Exception($"Không thể kiểm tra email: {ex.Message}");
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    // Xử lý khi nhấn liên hệ hỗ trợ
    private async void OnSupportTapped(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Hỗ trợ",
            "Email: support@suckhoe.com\nHotline: 1900-xxxx\nGiờ làm việc: 8:00 - 22:00",
            "Đóng"
        );
    }
}