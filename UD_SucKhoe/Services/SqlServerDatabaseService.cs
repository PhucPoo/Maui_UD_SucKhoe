using Microsoft.Data.SqlClient;
using UD_SucKhoe.Models;

namespace UD_SucKhoe.Services
{
    public class SqlServerDatabaseService
    {
        private readonly string _connectionString;

        public SqlServerDatabaseService()
        {
            _connectionString = new DatabaseService().GetConnectionString(); // Hoặc ConnectionString
        }

        // Đăng ký người dùng mới
        public async Task<bool> RegisterUser(string fullName, string email, string password,
            string gender = null, DateTime? dateOfBirth = null)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Kiểm tra email đã tồn tại chưa
                    var checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    using (var checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        var count = (int)await checkCmd.ExecuteScalarAsync();

                        if (count > 0)
                        {
                            return false; // Email đã tồn tại
                        }
                    }

                    // Thêm user mới
                    var insertQuery = @"
                        INSERT INTO Users (FullName, Email, PasswordHash, Gender, DateOfBirth, CreatedAt)
                        VALUES (@FullName, @Email, @PasswordHash, @Gender, @DateOfBirth, @CreatedAt)";

                    using (var cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", password); // TODO: Hash password
                        cmd.Parameters.AddWithValue("@Gender", (object)gender ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DateOfBirth", (object)dateOfBirth ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                        await cmd.ExecuteNonQueryAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Register Error: {ex.Message}");
                return false;
            }
        }

        // Đăng nhập
        public async Task<User> LoginUser(string email, string password)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"
                        SELECT UserID, FullName, Email, PasswordHash, Gender, DateOfBirth, 
                               Height, Weight, ActivityLevel, CreatedAt
                        FROM Users 
                        WHERE Email = @Email AND PasswordHash = @Password";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password); // TODO: Hash password

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new User
                                {
                                    UserID = reader.GetInt32(0),
                                    FullName = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Password = reader.GetString(3),
                                    Gender = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    DateOfBirth = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                                    Height = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                                    Weight = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                                    ActivityLevel = reader.IsDBNull(8) ? null : reader.GetString(8),
                                    CreatedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9)
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login Error: {ex.Message}");
                return null;
            }
        }

        // Cập nhật thông tin user
        public async Task<bool> UpdateUserProfile(int userId, string fullName, string gender,
            DateTime? dateOfBirth, decimal? height, decimal? weight, string activityLevel)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"
                        UPDATE Users 
                        SET FullName = @FullName, 
                            Gender = @Gender, 
                            DateOfBirth = @DateOfBirth,
                            Height = @Height,
                            Weight = @Weight,
                            ActivityLevel = @ActivityLevel
                        WHERE UserID = @UserID";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Gender", (object)gender ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DateOfBirth", (object)dateOfBirth ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Height", (object)height ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Weight", (object)weight ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ActivityLevel", (object)activityLevel ?? DBNull.Value);

                        await cmd.ExecuteNonQueryAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Update Error: {ex.Message}");
                return false;
            }
        }

        // Lấy thông tin user theo ID
        public async Task<User> GetUserById(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"
                        SELECT UserID, FullName, Email, PasswordHash, Gender, DateOfBirth, 
                               Height, Weight, ActivityLevel, CreatedAt
                        FROM Users 
                        WHERE UserID = @UserID";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new User
                                {
                                    UserID = reader.GetInt32(0),
                                    FullName = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Password = reader.GetString(3),
                                    Gender = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    DateOfBirth = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                                    Height = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                                    Weight = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                                    ActivityLevel = reader.IsDBNull(8) ? null : reader.GetString(8),
                                    CreatedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(9)
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Get User Error: {ex.Message}");
                return null;
            }
        }

        // Test connection
        public async Task<bool> TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Connection Error: {ex.Message}");
                return false;
            }
        }
    }
}