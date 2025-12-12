using UD_SucKhoe.Helpers;
using UD_SucKhoe.Models;
using UD_SucKhoe.Services;

namespace UD_SucKhoe;

public partial class NutritionPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private int _currentUserId;
    private double _currentBmi;
    private Dictionary<string, MealPlan> _weeklyMealPlan;

    public NutritionPage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
    }

    private async void OnBackTapped(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
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
            // Lấy thông tin user hiện tại (giả sử đã đăng nhập)
            _currentUserId = Preferences.Get("UserId", 0);

            if (_currentUserId == 0)
            {
                await DisplayAlert("Lỗi", "Vui lòng đăng nhập", "OK");
                return;
            }

            // Lấy chiều cao và cân nặng mới nhất
            var progress = await _dbService.GetLatestProgressAsync(_currentUserId);

            if (progress == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng cập nhật chiều cao và cân nặng trước", "OK");
                return;
            }

            // Tính BMI
            _currentBmi = progress.Weight / Math.Pow(progress.Height / 100, 2);

            // Lấy thực đơn theo BMI
            _weeklyMealPlan = NutritionHelper.GetWeeklyMealsByBMI(_currentBmi);

            // Hiển thị thực đơn hôm nay
            DisplayTodayMeal();

            // Tùy chọn: Lưu toàn bộ thực đơn tuần vào DB
            // await SaveWeeklyMealPlanToDatabase();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", $"Không thể tải dữ liệu: {ex.Message}", "OK");
        }
    }

    private void DisplayTodayMeal()
    {
        try
        {
            // Xóa nội dung cũ
            ContentLayout.Children.Clear();

            // Lấy ngày hiện tại
            string today = NutritionHelper.ConvertDayOfWeekToVietnamese(DateTime.Today.DayOfWeek);

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

            // Hiển thị thông tin BMI
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

            // Hiển thị thực đơn hôm nay
            var todayLabel = new Label
            {
                Text = $"Thực đơn {today}",
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 10, 0, 15)
            };
            ContentLayout.Children.Add(todayLabel);

            // Sáng
            AddMealSection("🌅 Bữa sáng", todayMeal.Breakfast, "#FFF3E0");

            // Trưa
            AddMealSection("☀️ Bữa trưa", todayMeal.Lunch, "#E8F5E9");

            // Phụ
            AddMealSection("🍎 Bữa phụ", todayMeal.Snack, "#F3E5F5");

            // Tối
            AddMealSection("🌙 Bữa tối", todayMeal.Dinner, "#E3F2FD");

            // Nút lưu thực đơn
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

            // Nút lưu cả tuần
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

        // Tiêu đề
        stack.Children.Add(new Label
        {
            Text = title,
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#333333")
        });

        // Danh sách món ăn
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

    // Lưu thực đơn hôm nay
    private async void OnSaveTodayMealClicked(object sender, EventArgs e)
    {
        try
        {
            var button = sender as Button;
            button.IsEnabled = false;
            button.Text = "Đang lưu...";

            string today = NutritionHelper.ConvertDayOfWeekToVietnamese(DateTime.Today.DayOfWeek);
            var todayMeal = _weeklyMealPlan[today];

            bool success = true;

            // Lưu từng bữa ăn
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

            // Lưu recommendation
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

    // Lưu thực đơn cả tuần
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