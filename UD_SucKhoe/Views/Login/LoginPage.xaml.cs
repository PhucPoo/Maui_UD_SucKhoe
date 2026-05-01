using UD_SucKhoe.Services.Database;
using UD_SucKhoe.ViewModels;

namespace UD_SucKhoe;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public LoginPage()
    {
        InitializeComponent();

        var db = new DatabaseService();
        _viewModel = new LoginViewModel(db);
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        var emailOrUsername = UsernameEntry.Text?.Trim();
        var password = PasswordEntry.Text?.Trim();

        // Disable UI
        UsernameEntry.IsEnabled = false;
        PasswordEntry.IsEnabled = false;

        var (success, message) = await _viewModel.Login(emailOrUsername, password);

        await DisplayAlert(success ? "Thành công" : "Lỗi", message, "OK");

        if (success)
        {
            await Navigation.PopModalAsync();
        }

        // Enable lại
        UsernameEntry.IsEnabled = true;
        PasswordEntry.IsEnabled = true;
    }

    private async void OnForgotPasswordTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new ForgotPasswordPage());
    }

    private async void OnRegisterTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new RegisterPage());
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}