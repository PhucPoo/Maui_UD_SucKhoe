using Microsoft.Maui.Controls;

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

            // Thực hiện logic đăng nhập ở đây
            // Ví dụ: gọi API, kiểm tra database, etc.

            await DisplayAlert("Thành công", "Đăng nhập thành công!", "OK");

            // Quay lại trang chính sau khi đăng nhập thành công
            await Navigation.PopModalAsync();
        }

        private async void OnRegisterTapped(object sender, EventArgs e)
        {
            // Chuyển đến trang đăng ký (bạn cần tạo RegisterPage)
            await DisplayAlert("Thông báo", "Chức năng đăng ký đang được phát triển", "OK");
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Quay lại trang trước
            await Navigation.PopModalAsync();
        }
    }
}