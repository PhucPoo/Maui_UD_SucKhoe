namespace UD_SucKhoe.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public string ActivityLevel { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}