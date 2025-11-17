using Microsoft.Maui.Controls.Shapes;
using UD_SucKhoe.Helpers;

namespace UD_SucKhoe;

public partial class NutritionPage : ContentPage
{
    private double _bmi;

    public NutritionPage(double bmi)
    {
        InitializeComponent();
        _bmi = bmi;
        LoadRecommendedFoods();
    }

    private void LoadRecommendedFoods()
    {
        // Lấy gợi ý từ Helper
        var meals = NutritionHelper.GetMealsByBMI(_bmi);

        // Hiển thị BMI
        ContentLayout.Children.Add(new Label
        {
            Text = $"BMI của bạn: {_bmi:F1}",
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromHex("#1E88E5"), // xanh da trời
            HorizontalOptions = LayoutOptions.Center
        });

        // Thêm từng bữa: sáng - trưa - chiều - tối
        AddMealSection("🍳 Bữa sáng", meals.Breakfast);
        AddMealSection("🍛 Bữa trưa", meals.Lunch);
        AddMealSection("🍜 Bữa chiều", meals.Snack);
        AddMealSection("🍲 Bữa tối", meals.Dinner);
    }

    private void AddMealSection(string title, List<string> foods)
    {
        // Tiêu đề mỗi bữa ăn
        ContentLayout.Children.Add(new Label
        {
            Text = title,
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromHex("#1E88E5"), // xanh da trời
            Margin = new Thickness(0, 15, 0, 5)
        });

        // Danh sách món – mỗi món nằm trong Border
        foreach (var food in foods)
        {
            ContentLayout.Children.Add(new Border
            {
                StrokeThickness = 1,
                Stroke = Colors.LightGray,
                BackgroundColor = Color.FromArgb("#F5FBFF"), // xanh nhạt nhẹ như UI mẫu
                Padding = 12,
                Margin = new Thickness(0, 5),
                StrokeShape = new RoundRectangle { CornerRadius = 12 },

                Content = new Label
                {
                    Text = "• " + food,
                    FontSize = 16,
                    TextColor = Colors.Black
                }
            });
        }
    }
}
