using UD_SucKhoe.Models;
using UD_SucKhoe.Services.Database;
using UD_SucKhoe.Services.Nutrition;

namespace UD_SucKhoe;

public partial class NutritionPage : ContentPage
{
    private readonly NutritionViewModel _viewModel;
    private readonly DatabaseService _dbService;
    private readonly INutritionService _nutritionService;

    private int _currentUserId;
    private double _currentBmi;
    private Dictionary<string, MealPlan> _weeklyMealPlan;

    public NutritionPage(DatabaseService dbService, INutritionService nutritionService)
    {
        InitializeComponent();

        _dbService = dbService;
        _nutritionService = nutritionService;

        _viewModel = new NutritionViewModel(dbService, nutritionService);
    }

    private async void OnBackTapped(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new MainPage());
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadNutritionData();
    }

    private async Task LoadNutritionData()
    {
        try
        {
            bool success = await _viewModel.LoadData();

            if (!success)
            {
                await DisplayAlert("Lỗi", "Không có dữ liệu", "OK");
                return;
            }

            _currentUserId = _viewModel.CurrentUserId;
            _currentBmi = _viewModel.CurrentBmi;
            _weeklyMealPlan = _viewModel.WeeklyMealPlan;

            DisplayTodayMeal();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", ex.Message, "OK");
        }
    }

    private void DisplayTodayMeal()
    {
        try
        {
            ContentLayout.Children.Clear();

            string today = _nutritionService.ConvertDayOfWeekToVietnamese(DateTime.Today.DayOfWeek);

            if (!_weeklyMealPlan.ContainsKey(today))
            {
                ContentLayout.Children.Add(new Label
                {
                    Text = "Không có thực đơn cho hôm nay",
                    HorizontalOptions = LayoutOptions.Center
                });
                return;
            }

            var todayMeal = _weeklyMealPlan[today];

            var bmiLabel = new Label
            {
                Text = $"BMI của bạn: {_currentBmi:F1} - {GetBMICategory(_currentBmi)}",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.FromArgb("#2196F3"),
                Margin = new Thickness(0, 0, 0, 10)
            };
            ContentLayout.Children.Add(bmiLabel);

            var todayLabel = new Label
            {
                Text = $"Thực đơn {today}",
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 10, 0, 15)
            };
            ContentLayout.Children.Add(todayLabel);

            AddMealSection("🌅 Bữa sáng", todayMeal.Breakfast, "#FFF3E0");

            AddMealSection("☀️ Bữa trưa", todayMeal.Lunch, "#E8F5E9");

            AddMealSection("🍎 Bữa phụ", todayMeal.Snack, "#F3E5F5");

            AddMealSection("🌙 Bữa tối", todayMeal.Dinner, "#E3F2FD");

            var saveButton = new Button
            {
                Text = "Lưu thực đơn hôm nay",
                BackgroundColor = Color.FromArgb("#4CAF50"),
                TextColor = Colors.White,
                CornerRadius = 8,
                Margin = new Thickness(0, 20, 0, 0)
            };
            saveButton.Clicked += OnSaveTodayMealClicked;
            ContentLayout.Children.Add(saveButton);

            var saveWeekButton = new Button
            {
                Text = "Lưu thực đơn cả tuần",
                BackgroundColor = Color.FromArgb("#2196F3"),
                TextColor = Colors.White,
                CornerRadius = 8,
                Margin = new Thickness(0, 10, 0, 0)
            };
            saveWeekButton.Clicked += OnSaveWeeklyMealClicked;
            ContentLayout.Children.Add(saveWeekButton);
        }
        catch (Exception ex)
        {
            DisplayAlert("Lỗi", $"Không thể hiển thị thực đơn: {ex.Message}", "OK");
        }
    }

    private void AddMealSection(string title, List<string> items, string backgroundColor)
    {
        var frame = new Frame
        {
            BackgroundColor = Color.FromArgb(backgroundColor),
            CornerRadius = 10,
            Padding = 15,
            Margin = new Thickness(0, 0, 0, 10),
            HasShadow = true
        };

        var stack = new VerticalStackLayout { Spacing = 8 };

        stack.Children.Add(new Label
        {
            Text = title,
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#333333")
        });

        foreach (var item in items)
        {
            stack.Children.Add(new Label
            {
                Text = $"• {item}",
                FontSize = 14,
                TextColor = Color.FromArgb("#666666")
            });
        }

        frame.Content = stack;
        ContentLayout.Children.Add(frame);
    }

    private string GetBMICategory(double bmi)
    {
        if (bmi < 18.5) return "Thiếu cân (Cần tăng cân)";
        if (bmi <= 24.9) return "Bình thường (Duy trì)";
        if (bmi <= 29.9) return "Thừa cân (Cần giảm cân)";
        return "Béo phì (Cần giảm cân gấp)";
    }

    private async void OnSaveTodayMealClicked(object sender, EventArgs e)
    {
        try
        {
            var button = sender as Button;
            button.IsEnabled = false;
            button.Text = "Đang lưu...";

            string today = _nutritionService.ConvertDayOfWeekToVietnamese(DateTime.Today.DayOfWeek);
            var todayMeal = _weeklyMealPlan[today];

            bool success = true;

            success &= await _dbService.SaveMealPlanAsync(
                _currentUserId,
                DateTime.Today,
                "Breakfast",
                string.Join(", ", todayMeal.Breakfast)
            );

            success &= await _dbService.SaveMealPlanAsync(
                _currentUserId,
                DateTime.Today,
                "Lunch",
                string.Join(", ", todayMeal.Lunch)
            );

            success &= await _dbService.SaveMealPlanAsync(
                _currentUserId,
                DateTime.Today,
                "Snack",
                string.Join(", ", todayMeal.Snack)
            );

            success &= await _dbService.SaveMealPlanAsync(
                _currentUserId,
                DateTime.Today,
                "Dinner",
                string.Join(", ", todayMeal.Dinner)
            );

            string recommendedFoods = $"Breakfast: {string.Join(", ", todayMeal.Breakfast)}; " +
                                     $"Lunch: {string.Join(", ", todayMeal.Lunch)}; " +
                                     $"Snack: {string.Join(", ", todayMeal.Snack)}; " +
                                     $"Dinner: {string.Join(", ", todayMeal.Dinner)}";

            await _dbService.SaveNutritionRecommendationAsync(
                _currentUserId,
                _currentBmi,
                recommendedFoods,
                $"Thực đơn phù hợp với BMI {_currentBmi:F1} ({GetBMICategory(_currentBmi)})"
            );

            button.Text = "Lưu thực đơn hôm nay";
            button.IsEnabled = true;

            if (success)
            {
                await DisplayAlert("Thành công", "Đã lưu thực đơn hôm nay!", "OK");
            }
            else
            {
                await DisplayAlert("Lỗi", "Không thể lưu thực đơn", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", $"Lỗi khi lưu: {ex.Message}", "OK");
        }
    }

    private async void OnSaveWeeklyMealClicked(object sender, EventArgs e)
    {
        try
        {
            var button = sender as Button;
            button.IsEnabled = false;
            button.Text = "Đang lưu cả tuần...";

            bool success = await _dbService.SaveWeeklyMealPlanAsync(_currentUserId, _weeklyMealPlan);

            button.Text = "Lưu thực đơn cả tuần";
            button.IsEnabled = true;

            if (success)
            {
                await DisplayAlert("Thành công", "Đã lưu thực đơn cả tuần!", "OK");
            }
            else
            {
                await DisplayAlert("Lỗi", "Không thể lưu thực đơn", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", $"Lỗi khi lưu: {ex.Message}", "OK");
        }
    }
}