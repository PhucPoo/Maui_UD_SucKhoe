namespace UD_SucKhoe
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private async void OnCloseTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnMenuManagementTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Quản lý thực đơn", "Chức năng đang phát triển", "OK");
        }

        private async void OnWorkoutManagementTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Quản lý tập luyện", "Chức năng đang phát triển", "OK");
        }

        private async void OnSettingsTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Cài đặt", "Chức năng đang phát triển", "OK");
        }

        private async void OnAboutTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Thông tin", "Ứng dụng Sức khỏe v1.0", "OK");
        }


    }
}