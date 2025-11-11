using Microsoft.Data.SqlClient;
using UD_SucKhoe.Models;

namespace UD_SucKhoe.Services
{
    public class DatabaseService
    {
        private readonly string connectionString =
            "Server=DESKTOP-27P4LC3;Database=DB_SucKhoe;Trusted_Connection=True;TrustServerCertificate=True;";

        // Hàm lấy danh sách user — có thể gọi ở ViewModel hoặc trang nào bạn muốn
        public async Task GetUsersAsync()
        {
            using SqlConnection conn = new(connectionString);
            await conn.OpenAsync();

            SqlCommand cmd = new("SELECT * FROM Users", conn);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Console.WriteLine(reader["Name"]);
            }
        }
        public string GetConnectionString()
        {
            return connectionString;
        }
        public async Task InsertProgressAsync(int userId, double height, double weight)
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
        public async Task<ProgressData?> GetLatestProgressAsync(int userId)
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

            return null;
        }
    }
}
