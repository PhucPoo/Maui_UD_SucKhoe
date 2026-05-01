namespace UD_SucKhoe.Models
{
    public class ProgressTracking
    {
        public int ProgressID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int CaloriesConsumed { get; set; }
        public int CaloriesBurned { get; set; }
        public string Note { get; set; }
    }
}
