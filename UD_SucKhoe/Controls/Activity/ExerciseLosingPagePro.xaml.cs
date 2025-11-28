using UD_SucKhoe.Services;
namespace UD_SucKhoe
{
    public partial class ExerciseLosingPagePro : ContentPage
    {
        private readonly DatabaseService _db = new DatabaseService();

        public ExerciseLosingPagePro()
        {
            InitializeComponent();
            LoadExercises();
        }

        private async void LoadExercises()
        {
            var exercises = await _db.GetExpertExercises();
            ExerciseList.ItemsSource = exercises;
        }
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }


}


