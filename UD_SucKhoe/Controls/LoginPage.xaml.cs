namespace UD_SucKhoe
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // Kiểm tra thông tin đăng nhập
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Lỗi", "Vui lòng nhập đầy đủ thông tin", "OK");
                return;
            }

            // TODO: kiểm tra logic đăng nhập (API hoặc database)

            await DisplayAlert("Thành công", "Đăng nhập thành công!", "OK");

            // 👉 Chuyển hẳn sang trang chính
            var currentWindow = Application.Current?.Windows.FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Page = new NavigationPage(new MainPage());
                NavigationPage.SetHasNavigationBar(currentWindow.Page, true);
            }
        }

        private async void OnForgotPasswordTapped(object sender, EventArgs e)
        {
            // Xử lý logic quên mật khẩu ở đây
            await DisplayAlert("Quên mật khẩu", "Chức năng đang được phát triển", "OK");
        }

        private async void OnRegisterTapped(object sender, EventArgs e)
        {
            try
            {
                var RegisterPage = new RegisterPage();

                var currentWindow = Application.Current?.Windows.FirstOrDefault();
                if (currentWindow?.Page != null)
                {
                    await currentWindow.Page.Navigation.PushModalAsync(RegisterPage);
                }
                else
                {
                    await DisplayAlert("Lỗi", "Unable to navigate: Current window or page is null.", "OK");
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Lỗi", ex.Message, "OK");
            }

        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Quay lại trang trước
            await Navigation.PopModalAsync();
        }
    }
}