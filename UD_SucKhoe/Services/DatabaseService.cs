using Microsoft.Data.SqlClient;

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
    }
}
