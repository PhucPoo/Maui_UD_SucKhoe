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

        // Hash password (dùng SHA256)
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
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
    }
}