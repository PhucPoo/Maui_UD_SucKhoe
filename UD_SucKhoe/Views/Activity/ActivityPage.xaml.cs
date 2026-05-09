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
            await Navigation.PushModalAsync(new WorkoutDetailPage());
        }

        private async void OnWeightLossTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ExerciseLosingPagePro());
        }

        private async void OnWeightLossHomeTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ExerciseLosingPageExp());
        }

        private async void OnWeightLossBeginnerTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Giảm Cân - Người bắt đầu", "22 ngày tập luyện", "OK");
        }

        private async void OnWeightGainTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Tăng Cân", "Tại gym - 44 ngày tập luyện", "OK");
        }

        private async void OnMaintainWeightTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Giữ Cân", "Tại nhà - 28 ngày tập luyện", "OK");
        }
    }
}