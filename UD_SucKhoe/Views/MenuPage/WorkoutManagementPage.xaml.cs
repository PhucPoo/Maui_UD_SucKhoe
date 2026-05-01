namespace UD_SucKhoe
{
    public partial class WorkoutManagementPage : ContentPage
    {
        private readonly WorkoutManagementViewModel _vm;

        public WorkoutManagementPage()
        {
            InitializeComponent();

            _vm = new WorkoutManagementViewModel();
            BindingContext = _vm;

            WorkoutCollectionView.ItemsSource = _vm.Exercises;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!_vm.IsLoggedIn)
            {
                var result = await DisplayAlert(
                    "Thông báo",
                    "Bạn chưa đăng nhập, không thể sử dụng chức năng này.",
                    "Đăng nhập",
                    "Trở lại"
                );

                if (result)
                    await Navigation.PushModalAsync(new LoginPage());
                else
                    await Navigation.PopModalAsync();

                return;
            }

            await _vm.LoadExercises();
        }

        private async void OnBackTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}