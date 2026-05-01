using UD_SucKhoe.Models;
using UD_SucKhoe.Services.Database;
using UD_SucKhoe.Services.Nutrition;

public class NutritionViewModel
{
    private readonly DatabaseService _dbService;
    private readonly INutritionService _nutritionService;

    public int CurrentUserId { get; set; }
    public double CurrentBmi { get; set; }
    public Dictionary<string, MealPlan> WeeklyMealPlan { get; set; }

    public NutritionViewModel(DatabaseService dbService, INutritionService nutritionService)
    {
        _dbService = dbService;
        _nutritionService = nutritionService;
    }

    public async Task<bool> LoadData()
    {
        CurrentUserId = Preferences.Get("UserId", 0);

        if (CurrentUserId == 0)
            return false;

        var progress = await _dbService.GetLatestProgressAsync(CurrentUserId);

        if (progress == null)
            return false;

        CurrentBmi = progress.Weight / Math.Pow(progress.Height / 100, 2);

        WeeklyMealPlan = _nutritionService.GetWeeklyMealsByBMI(CurrentBmi);

        return true;
    }

    public MealPlan GetTodayMeal()
    {
        string today = _nutritionService.ConvertDayOfWeekToVietnamese(DateTime.Today.DayOfWeek);

        if (WeeklyMealPlan != null && WeeklyMealPlan.ContainsKey(today))
            return WeeklyMealPlan[today];

        return new MealPlan();
    }

    public string GetToday()
    {
        return _nutritionService.ConvertDayOfWeekToVietnamese(DateTime.Today.DayOfWeek);
    }

    public string GetBMICategory()
    {
        double bmi = CurrentBmi;

        if (bmi < 18.5) return "Thiếu cân (Cần tăng cân)";
        if (bmi <= 24.9) return "Bình thường (Duy trì)";
        if (bmi <= 29.9) return "Thừa cân (Cần giảm cân)";
        return "Béo phì (Cần giảm cân gấp)";
    }
}