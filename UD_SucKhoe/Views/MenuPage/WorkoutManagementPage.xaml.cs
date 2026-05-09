using UD_SucKhoe.Models;

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

        private async void OnAddTapped(object sender, TappedEventArgs e)
        {
            string name = await DisplayPromptAsync(
                "Thêm bài tập",
                "Nhập tên bài tập");

            if (string.IsNullOrWhiteSpace(name))
                return;

            await _vm.AddExercise(
                name,
                "Cardio",
                30,
                200,
                "Medium");

            await _vm.LoadExercises();
        }

        private async void OnEditTapped(object sender, TappedEventArgs e)
        {
            var border = sender as Border;

            if (border?.BindingContext is not Exercise exercise)
                return;

            string newName = await DisplayPromptAsync(
                "Sửa bài tập",
                "Tên bài tập:",
                initialValue: exercise.ExerciseName);

            if (string.IsNullOrWhiteSpace(newName))
                return;

            await _vm.UpdateExercise(
                exercise,
                newName,
                exercise.Type,
                exercise.DurationPerSet,
                exercise.CaloriesBurned,
                exercise.DifficultyLevel);

            await _vm.LoadExercises();
        }


        private async void OnDeleteTapped(object sender, TappedEventArgs e)
        {
            var border = sender as Border;

            if (border?.BindingContext is not Exercise exercise)
                return;

            bool confirm = await DisplayAlert(
                "Xóa",
                $"Xóa {exercise.ExerciseName}?",
                "Có",
                "Không");

            if (!confirm)
                return;

            await _vm.DeleteExercise(exercise.ExerciseID);

            _vm.Exercises.Remove(exercise);
        }

    }
}