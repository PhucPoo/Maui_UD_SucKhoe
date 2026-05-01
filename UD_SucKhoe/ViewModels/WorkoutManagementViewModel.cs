using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UD_SucKhoe.Models;
using UD_SucKhoe.Services.Database;

public class WorkoutManagementViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService;

    public ObservableCollection<Exercise> Exercises { get; set; } = new();

    public bool IsLoggedIn =>
        Preferences.Get("UserId", 0) != 0;

    public WorkoutManagementViewModel()
    {
        _databaseService = new DatabaseService();
    }

    // ================= LOAD =================
    public async Task LoadExercises()
    {
        try
        {
            Exercises.Clear();

            using (SqlConnection conn = _databaseService.GetConnection())
            {
                await conn.OpenAsync();

                string query = @"SELECT ExerciseID, ExerciseName, Type, DurationPerSet, CaloriesBurned, DifficultyLevel 
                                 FROM Exercises 
                                 ORDER BY ExerciseID";

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
            throw new Exception("Load failed: " + ex.Message);
        }
    }

    // ================= ADD =================
    public async Task AddExercise(string name, string type, int duration, int calories, string difficulty)
    {
        using (SqlConnection conn = _databaseService.GetConnection())
        {
            await conn.OpenAsync();

            string query = @"INSERT INTO Exercises 
                            (ExerciseName, Type, DurationPerSet, CaloriesBurned, DifficultyLevel) 
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
    }

    // ================= UPDATE =================
    public async Task UpdateExercise(Exercise exercise, string name, string type, int duration, int calories, string difficulty)
    {
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
    }

    // ================= DELETE =================
    public async Task DeleteExercise(int id)
    {
        using (SqlConnection conn = _databaseService.GetConnection())
        {
            await conn.OpenAsync();

            string query = "DELETE FROM Exercises WHERE ExerciseID = @ID";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}