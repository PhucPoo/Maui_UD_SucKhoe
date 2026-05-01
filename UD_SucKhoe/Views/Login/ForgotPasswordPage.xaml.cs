using UD_SucKhoe.Services.Database;
using UD_SucKhoe.ViewModels;

namespace UD_SucKhoe;

public partial class ForgotPasswordPage : ContentPage
{
    private readonly ForgotPasswordViewModel _viewModel;

    public ForgotPasswordPage()
    {
        InitializeComponent();

        var db = new DatabaseService();
        _viewModel = new ForgotPasswordViewModel(db);
    }

    private async void OnSendCodeButtonClicked(object sender, EventArgs e)
    {
        var email = EmailPhoneEntry.Text?.Trim();

        // UI loading
        SendCodeButton.IsEnabled = false;
        SendCodeButton.Text = "Đang kiểm tra...";

        var (success, message) = await _viewModel.CheckEmail(email);

        await DisplayAlert(success ? "Thành công" : "Lỗi", message, "OK");

        if (success)
        {
            await Navigation.PushModalAsync(new ResetPasswordPage(email));
        }

        SendCodeButton.IsEnabled = true;
        SendCodeButton.Text = "Xác nhận";
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void OnSupportTapped(object sender, EventArgs e)
    {
        await DisplayAlert(
            "Hỗ trợ",
            "Email: support@suckhoe.com\nHotline: 1900-xxxx\nGiờ làm việc: 8:00 - 22:00",
            "Đóng"
        );
    }
}