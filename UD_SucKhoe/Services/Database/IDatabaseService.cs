using Microsoft.Data.SqlClient;
using UD_SucKhoe.Models;

namespace UD_SucKhoe.Services.Database
{
    public interface IDatabaseService
    {
        SqlConnection GetConnection();

        string HashPassword(string password);

        Task<List<Exercise>> GetExpertExercises();

        Task<bool> CheckEmailExists(string email);

        Task<User?> GetUserByEmail(string email);

        Task<bool> UpdateUserPassword(string email, string newPassword);

        Task<User?> ValidateUser(string email, string password);

        Task InsertProgressAsync(int userId, double height, double weight);

        Task<ProgressTracking?> GetLatestProgressAsync(int userId);

        Task<bool> TestConnection();

        Task<bool> SaveMealPlanAsync(int userId, DateTime date, string mealType, string foodItems);

        Task<bool> SaveWeeklyMealPlanAsync(int userId, Dictionary<string, MealPlan> weeklyPlan);

        Task<bool> SaveNutritionRecommendationAsync(int userId, double bmi, string recommendedFoods, string reason);

        Task<Dictionary<string, List<string>>?> GetMealPlanByDateAsync(int userId, DateTime date);

        Task<Dictionary<DateTime, Dictionary<string, List<string>>>?> GetWeeklyMealPlanAsync(int userId, DateTime startDate);

        Task<bool> DeleteMealPlanByDateAsync(int userId, DateTime date);
    }
}