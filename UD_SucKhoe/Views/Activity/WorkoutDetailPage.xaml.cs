namespace UD_SucKhoe
{
    public partial class WorkoutDetailPage : ContentPage
    {
        private readonly WorkoutDetailViewModel _vm;

        public WorkoutDetailPage()
        {
            InitializeComponent();

            _vm = new WorkoutDetailViewModel();
            BindingContext = _vm;

            CheckLoginAndLoad();
        }

        private async void CheckLoginAndLoad()
        {
            if (!_vm.IsLoggedIn)
            {
                var result = await DisplayAlert(
                    "Thông báo",
                    "Bạn chưa đăng nhập, không thể sử dụng chức năng này.",
                    "Đăng nhập",
                    "Trở lại"
                );

                if (result)
                {
                    // 👉 bấm "Đăng nhập"
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    // 👉 bấm "Trở lại"
                    await Navigation.PopModalAsync();

                }

                return;
            }

            await _vm.LoadExercises();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}