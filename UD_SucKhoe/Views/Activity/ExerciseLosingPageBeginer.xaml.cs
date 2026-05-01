using UD_SucKhoe.Services.Database;
namespace UD_SucKhoe
{
    public partial class ExerciseLosingPageBeginer : ContentPage
    {
        private readonly DatabaseService _db = new DatabaseService();

        public ExerciseLosingPageBeginer()
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




