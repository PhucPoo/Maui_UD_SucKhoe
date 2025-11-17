namespace UD_SucKhoe.Models
{
    public class MealPlan
    {
        public List<string> Breakfast { get; set; } = new();
        public List<string> Lunch { get; set; } = new();
        public List<string> Snack { get; set; } = new();
        public List<string> Dinner { get; set; } = new();
    }
}
