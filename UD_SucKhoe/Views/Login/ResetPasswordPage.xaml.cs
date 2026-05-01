using UD_SucKhoe.Services.Database;
using UD_SucKhoe.ViewModels;

namespace UD_SucKhoe;

public partial class ResetPasswordPage : ContentPage
{
    private readonly string _email;
    private readonly ResetPasswordViewModel _viewModel;

    public ResetPasswordPage(string email)
    {
        InitializeComponent();

        _email = email;

        var db = new DatabaseService();
        _viewModel = new ResetPasswordViewModel(db);
    }

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        var newPass = NewPasswordEntry.Text?.Trim();
        var confirm = ConfirmPasswordEntry.Text?.Trim();

        var loadingIndicator = ShowLoading();

        var (success, message) = await _viewModel.ResetPassword(_email, newPass, confirm);

        HideLoading(loadingIndicator);

        await DisplayAlert(success ? "Thành công" : "Lỗi", message, "OK");

        if (success)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Xác nhận", "Bạn có muốn hủy đổi mật khẩu?", "Có", "Không");

        if (answer)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }

    // Loading giữ nguyên UI
    private ActivityIndicator ShowLoading()
    {
        var indicator = new ActivityIndicator
        {
            IsRunning = true,
            IsVisible = true,
            Color = Colors.Blue
        };

        return indicator;
    }

    private void HideLoading(ActivityIndicator indicator)
    {
        if (indicator != null)
        {
            indicator.IsRunning = false;
            indicator.IsVisible = false;
        }
    }
}