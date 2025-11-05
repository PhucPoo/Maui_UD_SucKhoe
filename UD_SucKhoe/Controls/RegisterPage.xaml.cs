namespace UD_SucKhoe
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            string fullname = FullnameEntry.Text;
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // Kiểm tra thông tin đăng nhập
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fullname))
            {
                await DisplayAlert("Lỗi", "Vui lòng nhập đầy đủ thông tin", "OK");
                return;
            }

            // Thực hiện logic đăng nhập ở đây
            // Ví dụ: gọi API, kiểm tra database, etc.

            await DisplayAlert("Thành công", "Đăng ký thành công!", "OK");

            // Quay lại trang chính sau khi đăng nhập thành công
            await Navigation.PopModalAsync();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // Quay lại trang trước
            await Navigation.PopModalAsync();
        }
    }
}