using UD_SucKhoe.Services;
using UD_SucKhoe.ViewModels;

namespace UD_SucKhoe;

public partial class RegisterPage : ContentPage
{
    private readonly RegisterViewModel _viewModel;

    public RegisterPage()
    {
        InitializeComponent();

        var db = new SqlServerDatabaseService();
        _viewModel = new RegisterViewModel(db);

        TestDatabaseConnection();
    }

    private async void TestDatabaseConnection()
    {
        var isConnected = await _viewModel.TestConnection();

        if (!isConnected)
        {
            await DisplayAlert("Cảnh báo", "Không thể kết nối đến cơ sở dữ liệu!", "OK");
        }
    }

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        var fullName = FullNameEntry.Text?.Trim();
        var email = EmailEntry.Text?.Trim();
        var password = PasswordEntry.Text;
        var confirmPassword = ConfirmPasswordEntry.Text;

        var (success, message) = await _viewModel.Register(
            fullName,
            email,
            password,
            confirmPassword);

        await DisplayAlert(success ? "Thành công" : "Lỗi", message, "OK");

        if (success)
        {
            await Navigation.PopModalAsync();
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}