namespace UD_SucKhoe
{
    public partial class ActivityPage : ContentPage
    {
        public ActivityPage()
        {
            InitializeComponent();
        }

        private async void OnBackTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnExpertPlanTapped(object sender, EventArgs e)
        {
            // Chuyển đến trang chi tiết kế hoạch chuyên gia
            await Navigation.PushModalAsync(new WorkoutDetailPage());
            // TODO: await Navigation.PushAsync(new WorkoutDetailPage("expert"));
        }

        private async void OnWeightLossTapped(object sender, EventArgs e)
        {
            // Chuyển đến trang chi tiết giảm cân Pro
            await Navigation.PushModalAsync(new ExerciseLosingPagePro());
            // TODO: await Navigation.PushAsync(new WorkoutDetailPage("weight-loss-pro"));
        }

        private async void OnWeightLossHomeTapped(object sender, EventArgs e)
        {
            // Chuyển đến trang chi tiết giảm cân tại nhà
            await Navigation.PushModalAsync(new ExerciseLosingPageExp());
            // TODO: await Navigation.PushAsync(new WorkoutDetailPage("weight-loss-home"));
        }

        private async void OnWeightLossBeginnerTapped(object sender, EventArgs e)
        {
            // Chuyển đến trang chi tiết giảm cân người bắt đầu
            await DisplayAlert("Giảm Cân - Người bắt đầu", "22 ngày tập luyện", "OK");
            // TODO: await Navigation.PushAsync(new WorkoutDetailPage("weight-loss-beginner"));
        }

        private async void OnWeightGainTapped(object sender, EventArgs e)
        {
            // Chuyển đến trang chi tiết tăng cân
            await DisplayAlert("Tăng Cân", "Tại gym - 44 ngày tập luyện", "OK");
            // TODO: await Navigation.PushAsync(new WorkoutDetailPage("weight-gain"));
        }

        private async void OnMaintainWeightTapped(object sender, EventArgs e)
        {
            // Chuyển đến trang chi tiết giữ cân
            await DisplayAlert("Giữ Cân", "Tại nhà - 28 ngày tập luyện", "OK");
            // TODO: await Navigation.PushAsync(new WorkoutDetailPage("maintain-weight"));
        }
    }
}