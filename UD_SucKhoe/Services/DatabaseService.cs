using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using UD_SucKhoe.Models;

namespace UD_SucKhoe.Services
{
    public class DatabaseService
    {

        private readonly string connectionString =
            "Server=DESKTOP-27P4LC3;Database=DB_SucKhoe;Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Hash password (dùng SHA256)
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);

        }




        public async Task<List<Exercise>> GetExpertExercises()
        {
            var list = new List<Exercise>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT ExerciseID, ExerciseName, Type, DurationPerSet, CaloriesBurned, DifficultyLevel FROM Exercises";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new Exercise
                        {
                            ExerciseID = reader.GetInt32(0),
                            ExerciseName = reader.GetString(1),
                            Type = reader.GetString(2),
                            DurationPerSet = reader.GetInt32(3),
                            CaloriesBurned = reader.GetInt32(4),
                            DifficultyLevel = reader.GetString(5)
                        });
                    }
                }
            }

            return list;
        }


        // Kiểm tra email có tồn tại không

        public async Task<bool> CheckEmailExists(string email)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        int count = (int)await command.ExecuteScalarAsync();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking email: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return false;
            }
        }

        // Lấy thông tin user theo email
        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    // Chỉ SELECT các cột cần thiết - Sử dụng PasswordHash thay vì Password
                    string query = "SELECT UserID, Email, PasswordHash, FullName FROM Users WHERE Email = @Email";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new User
                                {
                                    UserID = reader.GetInt32(0),
                                    Email = reader.GetString(1),
                                    Password = reader.GetString(2),
                                    FullName = reader.IsDBNull(3) ? "" : reader.GetString(3)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
            return null;
        }

        // Cập nhật mật khẩu (tự động hash)
        public async Task<bool> UpdateUserPassword(string email, string newPassword)
        {
            try
            {
                Console.WriteLine($"[DB] Starting password update for: {email}");

                using (var connection = new SqlConnection(connectionString))
                {
                    Console.WriteLine($"[DB] Opening connection...");
                    await connection.OpenAsync();
                    Console.WriteLine($"[DB] Connection opened successfully");

                    // Kiểm tra email có tồn tại không trước
                    string checkQuery = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
                    using (var checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)await checkCmd.ExecuteScalarAsync();

                        if (count == 0)
                        {
                            Console.WriteLine($"[DB] ✗ Email '{email}' không tồn tại trong database");
                            return false;
                        }
                        Console.WriteLine($"[DB] ✓ Email found in database");
                    }

                    // Hash mật khẩu mới
                    string hashedPassword = HashPassword(newPassword);
                    Console.WriteLine($"[DB] Password hashed. Length: {hashedPassword.Length}");

                    // Cập nhật mật khẩu - Sử dụng PasswordHash
                    string query = "UPDATE Users SET PasswordHash = @Password WHERE Email = @Email";
                    Console.WriteLine($"[DB] Query: {query}");

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Password", hashedPassword);
                        command.Parameters.AddWithValue("@Email", email);

                        Console.WriteLine($"[DB] Executing update query...");
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        Console.WriteLine($"[DB] Rows affected: {rowsAffected}");

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"[DB] ✓ Password updated successfully for {email}");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"[DB] ✗ No rows updated");
                            return false;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"[DB] ✗ SQL ERROR: {sqlEx.Message}");
                Console.WriteLine($"[DB] ✗ Error Number: {sqlEx.Number}");
                Console.WriteLine($"[DB] ✗ StackTrace: {sqlEx.StackTrace}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB] ✗ GENERAL ERROR: {ex.Message}");
                Console.WriteLine($"[DB] ✗ StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[DB] ✗ InnerException: {ex.InnerException.Message}");
                }
                return false;
            }
        }

        // Xác thực đăng nhập
        public async Task<User?> ValidateUser(string email, string password)
        {
            try
            {
                string hashedPassword = HashPassword(password);

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    // Chỉ SELECT các cột cần thiết - Sử dụng PasswordHash và FullName
                    string query = @"SELECT UserID, Email, PasswordHash, FullName
                                   FROM Users 
                                   WHERE Email = @Email AND PasswordHash = @Password";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", hashedPassword);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new User
                                {
                                    UserID = reader.GetInt32(0),
                                    Email = reader.GetString(1),
                                    Password = reader.GetString(2),
                                    FullName = reader.IsDBNull(3) ? "" : reader.GetString(3)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating user: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
            return null;
        }

        // Lấy danh sách users
        public async Task GetUsersAsync()
        {
            try
            {
                using SqlConnection conn = new(connectionString);
                await conn.OpenAsync();
                SqlCommand cmd = new("SELECT UserID, Email, FullName FROM Users", conn);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Console.WriteLine(reader["FullName"]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting users: {ex.Message}");
            }
        }

        public string GetConnectionString()
        {
            return connectionString;
        }

        // Thêm progress tracking
        public async Task InsertProgressAsync(int userId, double height, double weight)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"INSERT INTO [dbo].[ProgressTracking] 
                                    (UserID, [Date], [Weight], [CaloriesConsumed], [CaloriesBurned], [Note], [Height])
                                    VALUES (@UserID, @Date, @Weight, @CaloriesConsumed, @CaloriesBurned, @Note, @Height)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@Date", DateTime.Now);
                        command.Parameters.AddWithValue("@Weight", weight);
                        command.Parameters.AddWithValue("@CaloriesConsumed", DBNull.Value);
                        command.Parameters.AddWithValue("@CaloriesBurned", DBNull.Value);
                        command.Parameters.AddWithValue("@Note", DBNull.Value);
                        command.Parameters.AddWithValue("@Height", height);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting progress: {ex.Message}");
            }
        }

        // Lấy progress mới nhất
        public async Task<ProgressData?> GetLatestProgressAsync(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT TOP 1 [Weight], [Height]
                             FROM [dbo].[ProgressTracking]
                             WHERE [UserID] = @UserID
                             ORDER BY [Date] DESC";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new ProgressData
                                {
                                    Weight = (double)reader.GetDecimal(0),
                                    Height = reader.GetDouble(1)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting latest progress: {ex.Message}");
            }
            return null;
        }




        // Method test connection
        public async Task<bool> TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    Console.WriteLine("✓ Connection successful!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Connection failed: {ex.Message}");
                return false;
            }
        }
        // Thêm các phương thức này vào class DatabaseService

        // 1. Lưu thực đơn cho 1 ngày
        public async Task<bool> SaveMealPlanAsync(int userId, DateTime date, string mealType, string foodItems)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Kiểm tra xem đã có meal plan cho ngày và loại bữa ăn này chưa
                    string checkQuery = @"SELECT COUNT(1) FROM MealPlans 
                                 WHERE UserID = @UserID AND Date = @Date AND MealType = @MealType";

                    using (var checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@UserID", userId);
                        checkCmd.Parameters.AddWithValue("@Date", date.Date);
                        checkCmd.Parameters.AddWithValue("@MealType", mealType);

                        int exists = (int)await checkCmd.ExecuteScalarAsync();

                        if (exists > 0)
                        {
                            // Cập nhật nếu đã tồn tại
                            string updateQuery = @"UPDATE MealPlans 
                                          SET FoodID = @FoodID, Quantity = @Quantity
                                          WHERE UserID = @UserID AND Date = @Date AND MealType = @MealType";

                            using (var updateCmd = new SqlCommand(updateQuery, connection))
                            {
                                updateCmd.Parameters.AddWithValue("@UserID", userId);
                                updateCmd.Parameters.AddWithValue("@Date", date.Date);
                                updateCmd.Parameters.AddWithValue("@MealType", mealType);
                                updateCmd.Parameters.AddWithValue("@FoodID", DBNull.Value); // Có thể link với bảng Foods nếu cần
                                updateCmd.Parameters.AddWithValue("@Quantity", foodItems);

                                await updateCmd.ExecuteNonQueryAsync();
                            }
                        }
                        else
                        {
                            // Thêm mới nếu chưa tồn tại
                            string insertQuery = @"INSERT INTO MealPlans (UserID, Date, MealType, FoodID, Quantity)
                                          VALUES (@UserID, @Date, @MealType, @FoodID, @Quantity)";

                            using (var insertCmd = new SqlCommand(insertQuery, connection))
                            {
                                insertCmd.Parameters.AddWithValue("@UserID", userId);
                                insertCmd.Parameters.AddWithValue("@Date", date.Date);
                                insertCmd.Parameters.AddWithValue("@MealType", mealType);
                                insertCmd.Parameters.AddWithValue("@FoodID", DBNull.Value);
                                insertCmd.Parameters.AddWithValue("@Quantity", foodItems);

                                await insertCmd.ExecuteNonQueryAsync();
                            }
                        }
                    }

                    Console.WriteLine($"✓ Saved meal plan: {mealType} for {date.ToShortDateString()}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving meal plan: {ex.Message}");
                return false;
            }
        }

        // 2. Lưu toàn bộ thực đơn trong tuần
        public async Task<bool> SaveWeeklyMealPlanAsync(int userId, Dictionary<string, MealPlan> weeklyPlan)
        {
            try
            {
                DateTime today = DateTime.Today;

                // Tìm ngày thứ 2 của tuần hiện tại
                int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
                DateTime monday = today.AddDays(-daysUntilMonday);

                var dayMapping = new Dictionary<string, int>
        {
            {"Thứ 2", 0}, {"Thứ 3", 1}, {"Thứ 4", 2}, {"Thứ 5", 3},
            {"Thứ 6", 4}, {"Thứ 7", 5}, {"Chủ nhật", 6}
        };

                foreach (var day in weeklyPlan)
                {
                    DateTime mealDate = monday.AddDays(dayMapping[day.Key]);
                    var meal = day.Value;

                    // Lưu Breakfast
                    if (meal.Breakfast.Any())
                    {
                        string breakfastItems = string.Join(", ", meal.Breakfast);
                        await SaveMealPlanAsync(userId, mealDate, "Breakfast", breakfastItems);
                    }

                    // Lưu Lunch
                    if (meal.Lunch.Any())
                    {
                        string lunchItems = string.Join(", ", meal.Lunch);
                        await SaveMealPlanAsync(userId, mealDate, "Lunch", lunchItems);
                    }

                    // Lưu Snack
                    if (meal.Snack.Any())
                    {
                        string snackItems = string.Join(", ", meal.Snack);
                        await SaveMealPlanAsync(userId, mealDate, "Snack", snackItems);
                    }

                    // Lưu Dinner
                    if (meal.Dinner.Any())
                    {
                        string dinnerItems = string.Join(", ", meal.Dinner);
                        await SaveMealPlanAsync(userId, mealDate, "Dinner", dinnerItems);
                    }
                }

                Console.WriteLine($"✓ Saved weekly meal plan for user {userId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving weekly meal plan: {ex.Message}");
                return false;
            }
        }

        // 3. Lưu gợi ý dinh dưỡng vào bảng Recommendations
        public async Task<bool> SaveNutritionRecommendationAsync(int userId, double bmi, string recommendedFoods, string reason)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO Recommendations 
                            (UserID, Date, RecommendedCalories, RecommendedFoods, RecommendedExercises, Reason)
                            VALUES (@UserID, @Date, @RecommendedCalories, @RecommendedFoods, @RecommendedExercises, @Reason)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@Date", DateTime.Now);

                        // Tính calories khuyến nghị dựa trên BMI
                        int recommendedCalories = bmi < 18.5 ? 2500 : (bmi <= 24.9 ? 2000 : 1500);
                        command.Parameters.AddWithValue("@RecommendedCalories", recommendedCalories);
                        command.Parameters.AddWithValue("@RecommendedFoods", recommendedFoods);
                        command.Parameters.AddWithValue("@RecommendedExercises", DBNull.Value);
                        command.Parameters.AddWithValue("@Reason", reason);

                        await command.ExecuteNonQueryAsync();
                    }

                    Console.WriteLine($"✓ Saved nutrition recommendation for user {userId}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving recommendation: {ex.Message}");
                return false;
            }
        }

        // 4. Lấy thực đơn theo ngày
        public async Task<Dictionary<string, List<string>>?> GetMealPlanByDateAsync(int userId, DateTime date)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"SELECT MealType, Quantity 
                            FROM MealPlans 
                            WHERE UserID = @UserID AND Date = @Date
                            ORDER BY 
                                CASE MealType
                                    WHEN 'Breakfast' THEN 1
                                    WHEN 'Lunch' THEN 2
                                    WHEN 'Snack' THEN 3
                                    WHEN 'Dinner' THEN 4
                                END";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@Date", date.Date);

                        var mealPlan = new Dictionary<string, List<string>>();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                string mealType = reader.GetString(0);
                                string quantity = reader.GetString(1);

                                mealPlan[mealType] = quantity.Split(", ").ToList();
                            }
                        }

                        return mealPlan.Any() ? mealPlan : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting meal plan: {ex.Message}");
                return null;
            }
        }

        // 5. Lấy thực đơn cả tuần
        public async Task<Dictionary<DateTime, Dictionary<string, List<string>>>?> GetWeeklyMealPlanAsync(int userId, DateTime startDate)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    DateTime endDate = startDate.AddDays(7);

                    string query = @"SELECT Date, MealType, Quantity 
                            FROM MealPlans 
                            WHERE UserID = @UserID AND Date >= @StartDate AND Date < @EndDate
                            ORDER BY Date, 
                                CASE MealType
                                    WHEN 'Breakfast' THEN 1
                                    WHEN 'Lunch' THEN 2
                                    WHEN 'Snack' THEN 3
                                    WHEN 'Dinner' THEN 4
                                END";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@StartDate", startDate.Date);
                        command.Parameters.AddWithValue("@EndDate", endDate.Date);

                        var weeklyPlan = new Dictionary<DateTime, Dictionary<string, List<string>>>();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                DateTime date = reader.GetDateTime(0);
                                string mealType = reader.GetString(1);
                                string quantity = reader.GetString(2);

                                if (!weeklyPlan.ContainsKey(date))
                                {
                                    weeklyPlan[date] = new Dictionary<string, List<string>>();
                                }

                                weeklyPlan[date][mealType] = quantity.Split(", ").ToList();
                            }
                        }

                        return weeklyPlan.Any() ? weeklyPlan : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting weekly meal plan: {ex.Message}");
                return null;
            }
        }

        // 6. Xóa thực đơn theo ngày
        public async Task<bool> DeleteMealPlanByDateAsync(int userId, DateTime date)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = "DELETE FROM MealPlans WHERE UserID = @UserID AND Date = @Date";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@Date", date.Date);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        Console.WriteLine($"✓ Deleted {rowsAffected} meal plan(s) for {date.ToShortDateString()}");
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting meal plan: {ex.Message}");
                return false;
            }
        }
    }
}