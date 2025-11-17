using UD_SucKhoe.Models;

namespace UD_SucKhoe.Helpers;

public static class NutritionHelper
{
    public static MealPlan GetMealsByBMI(double bmi)
    {
        var meal = new MealPlan();

        if (bmi < 18.5)
        {
            // Tăng cân
            meal.Breakfast.AddRange(new[]
            {
                "Bánh mì + trứng ốp la",
                "Sinh tố chuối + sữa chua",
                "Yến mạch + sữa tươi"
            });

            meal.Lunch.AddRange(new[]
            {
                "Cơm + thịt gà + rau xanh",
                "Canh xương",
                "Tráng miệng chuối"
            });

            meal.Snack.AddRange(new[]
            {
                "Sữa giàu năng lượng",
                "Hạnh nhân, óc chó"
            });

            meal.Dinner.AddRange(new[]
            {
                "Cơm + cá hồi + rau củ",
                "Khoai lang",
                "Sữa nóng"
            });
        }
        else if (bmi < 24.9)
        {
            // Duy trì
            meal.Breakfast.AddRange(new[]
            {
                "Bánh mì gạo lứt + trứng",
                "Trà xanh"
            });

            meal.Lunch.AddRange(new[]
            {
                "Cơm + thịt nạc + rau",
                "Canh bí"
            });

            meal.Snack.AddRange(new[]
            {
                "Sữa chua không đường",
                "Táo"
            });

            meal.Dinner.AddRange(new[]
            {
                "Salad ức gà",
                "Khoai tây nghiền"
            });
        }

        // ... các mức BMI khác

        return meal;
    }
}