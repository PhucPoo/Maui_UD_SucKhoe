using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using UD_SucKhoe.Models;
using UD_SucKhoe.Services;

namespace UD_SucKhoe
{
    public partial class WorkoutManagementPage : ContentPage
    {
        public ObservableCollection<Exercise> Exercises { get; set; }
        private readonly DatabaseService _databaseService;

        public WorkoutManagementPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            Exercises = new ObservableCollection<Exercise>();
            WorkoutCollectionView.ItemsSource = Exercises;
            LoadExercises();
        }

        private async void OnBackTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        // Load dữ liệu từ database
        private async void LoadExercises()
        {
            try
            {
                Exercises.Clear();

                using (SqlConnection conn = _databaseService.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = "SELECT ExerciseID, ExerciseName, Type, DurationPerSet, CaloriesBurned, DifficultyLevel FROM Exercises ORDER BY ExerciseID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Exercises.Add(new Exercise
                            {
                                ExerciseID = reader.GetInt32(0),
                                ExerciseName = reader.GetString(1),
                                Type = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                DurationPerSet = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                CaloriesBurned = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                DifficultyLevel = reader.IsDBNull(5) ? "" : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể tải dữ liệu: {ex.Message}", "OK");
            }
        }

        // Thêm bài tập mới
        private async void OnAddTapped(object sender, EventArgs e)
        {
            try
            {
                string name = await DisplayPromptAsync("Thêm bài tập", "Tên bài tập:", "OK", "Hủy", placeholder: "Vd: Push-up");
                if (string.IsNullOrWhiteSpace(name))
                    return;

                string type = await DisplayPromptAsync("Loại bài tập", "Loại (Cardio/Strength/Flexibility):", "OK", "Hủy", placeholder: "Vd: Strength");
                if (string.IsNullOrWhiteSpace(type))
                    type = "";

                string durationStr = await DisplayPromptAsync("Thời gian", "Thời gian mỗi set (phút):", "OK", "Hủy", keyboard: Keyboard.Numeric, placeholder: "Vd: 15");
                int duration = 0;
                if (!string.IsNullOrWhiteSpace(durationStr))
                    int.TryParse(durationStr, out duration);

                string caloriesStr = await DisplayPromptAsync("Calo", "Calo tiêu hao:", "OK", "Hủy", keyboard: Keyboard.Numeric, placeholder: "Vd: 100");
                int calories = 0;
                if (!string.IsNullOrWhiteSpace(caloriesStr))
                    int.TryParse(caloriesStr, out calories);

                string difficulty = await DisplayPromptAsync("Độ khó", "Độ khó (Easy/Medium/Hard):", "OK", "Hủy", placeholder: "Vd: Medium");
                if (string.IsNullOrWhiteSpace(difficulty))
                    difficulty = "";

                using (SqlConnection conn = _databaseService.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"INSERT INTO Exercises (ExerciseName, Type, DurationPerSet, CaloriesBurned, DifficultyLevel) 
                                    VALUES (@Name, @Type, @Duration, @Calories, @Difficulty)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Type", (object)type ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Duration", duration > 0 ? duration : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Calories", calories > 0 ? calories : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Difficulty", (object)difficulty ?? DBNull.Value);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                await DisplayAlert("Thành công", "Đã thêm bài tập mới!", "OK");
                LoadExercises();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể thêm bài tập: {ex.Message}", "OK");
            }
        }

        // Sửa bài tập
        private async void OnEditTapped(object sender, EventArgs e)
        {
            try
            {
                var exercise = (sender as View)?.BindingContext as Exercise;
                if (exercise == null) return;

                string name = await DisplayPromptAsync("Sửa bài tập", "Tên bài tập:", "OK", "Hủy", initialValue: exercise.ExerciseName);
                if (string.IsNullOrWhiteSpace(name))
                    return;

                string type = await DisplayPromptAsync("Loại bài tập", "Loại:", "OK", "Hủy", initialValue: exercise.Type);
                if (string.IsNullOrWhiteSpace(type))
                    type = "";

                string durationStr = await DisplayPromptAsync("Thời gian", "Thời gian mỗi set (phút):", "OK", "Hủy", keyboard: Keyboard.Numeric, initialValue: exercise.DurationPerSet.ToString());
                int duration = 0;
                if (!string.IsNullOrWhiteSpace(durationStr))
                    int.TryParse(durationStr, out duration);

                string caloriesStr = await DisplayPromptAsync("Calo", "Calo tiêu hao:", "OK", "Hủy", keyboard: Keyboard.Numeric, initialValue: exercise.CaloriesBurned.ToString());
                int calories = 0;
                if (!string.IsNullOrWhiteSpace(caloriesStr))
                    int.TryParse(caloriesStr, out calories);

                string difficulty = await DisplayPromptAsync("Độ khó", "Độ khó:", "OK", "Hủy", initialValue: exercise.DifficultyLevel);
                if (string.IsNullOrWhiteSpace(difficulty))
                    difficulty = "";

                using (SqlConnection conn = _databaseService.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"UPDATE Exercises 
                                    SET ExerciseName = @Name, 
                                        Type = @Type, 
                                        DurationPerSet = @Duration, 
                                        CaloriesBurned = @Calories, 
                                        DifficultyLevel = @Difficulty 
                                    WHERE ExerciseID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", exercise.ExerciseID);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Type", (object)type ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Duration", duration > 0 ? duration : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Calories", calories > 0 ? calories : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Difficulty", (object)difficulty ?? DBNull.Value);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                await DisplayAlert("Thành công", "Đã cập nhật bài tập!", "OK");
                LoadExercises();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể cập nhật bài tập: {ex.Message}", "OK");
            }
        }

        // Xóa bài tập
        private async void OnDeleteTapped(object sender, EventArgs e)
        {
            try
            {
                var exercise = (sender as View)?.BindingContext as Exercise;
                if (exercise == null) return;

                bool confirm = await DisplayAlert("Xác nhận", $"Bạn có chắc muốn xóa bài tập '{exercise.ExerciseName}'?", "Xóa", "Hủy");
                if (!confirm) return;

                using (SqlConnection conn = _databaseService.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = "DELETE FROM Exercises WHERE ExerciseID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", exercise.ExerciseID);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                await DisplayAlert("Thành công", "Đã xóa bài tập!", "OK");
                LoadExercises();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể xóa bài tập: {ex.Message}", "OK");
            }
        }
    }


}