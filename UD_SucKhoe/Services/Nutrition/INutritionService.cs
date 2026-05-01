using UD_SucKhoe.Models;

namespace UD_SucKhoe.Services.Nutrition
{
    public interface INutritionService
    {
        MealPlan GetMealsByBMI(double bmi);

        Dictionary<string, MealPlan> GetWeeklyMealsByBMI(double bmi);

        MealPlan GetMealByDayOfWeek(double bmi, string dayOfWeek);

        string ConvertDayOfWeekToVietnamese(DayOfWeek day);
    }
}