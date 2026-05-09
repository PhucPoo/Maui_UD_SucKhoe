using UD_SucKhoe.Services.Database;

namespace UD_SucKhoe;

public partial class BodyMeasurementsPage : ContentPage
{
    public event Action<double> BMICalculated;

    public BodyMeasurementsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();



        await Task.Delay(100);
        await LoadLatestDataAsync();
    }

    private async Task LoadLatestDataAsync()
    {
        try
        {
            int userId = Preferences.Get("UserId", 0);

            System.Diagnostics.Debug.WriteLine($"[DEBUG] UserId from Preferences: {userId}");

            if (userId == 0)
            {
                System.Diagnostics.Debug.WriteLine("[DEBUG] UserId = 0, user not logged in");
                return;
            }

            var db = new DatabaseService();
            var latestProgress = await db.GetLatestProgressAsync(userId);

            System.Diagnostics.Debug.WriteLine($"[DEBUG] Latest progress data: {(latestProgress == null ? "NULL" : $"Height={latestProgress.Height}, Weight={latestProgress.Weight}")}");

            if (latestProgress == null)
            {
                System.Diagnostics.Debug.WriteLine("[DEBUG] No data found in database");
                return;
            }

            HeightEntry.Text = latestProgress.Height.ToString();
            WeightEntry.Text = latestProgress.Weight.ToString();

            System.Diagnostics.Debug.WriteLine($"[DEBUG] Set HeightEntry: {latestProgress.Height}, WeightEntry: {latestProgress.Weight}");

            double heightInMeters = latestProgress.Height / 100;
            double bmi = latestProgress.Weight / (heightInMeters * heightInMeters);

            BMICalculated?.Invoke(bmi);

            BMIValueLabel.Text = bmi.ToString("F2");

            string status;
            Color statusColor;
            string description;

            if (bmi < 18.5)
            {
                status = "Thiếu Cân";
                statusColor = Color.FromArgb("#E74C3C");
                description = "Bạn nên tăng cân để đạt chỉ số lý tưởng";
            }
            else if (bmi < 25)
            {
                status = "Bình Thường";
                statusColor = Color.FromArgb("#27AE60");
                description = "Chỉ số BMI của bạn ở mức lý tưởng";
            }
            else if (bmi < 30)
            {
                status = "Thừa Cân";
                statusColor = Color.FromArgb("#F39C12");
                description = "Bạn nên giảm cân để cải thiện sức khỏe";
            }
            else
            {
                status = "Béo Phì";
                statusColor = Color.FromArgb("#E74C3C");
                description = "Bạn cần giảm cân để cải thiện sức khỏe";
            }

            BMIStatusLabel.Text = status;
            BMIStatusLabel.TextColor = statusColor;
            BMIValueLabel.TextColor = statusColor;
            BMIDescriptionLabel.Text = description;

            // Hiển thị frame BMI
            BMIFrame.IsVisible = true;

            HeightResultLabel.Text = $"Chiều cao: {latestProgress.Height} cm";
            HeightResultLabel.IsVisible = true;

            WeightResultLabel.Text = $"Cân nặng: {latestProgress.Weight} kg";
            WeightResultLabel.IsVisible = true;

            System.Diagnostics.Debug.WriteLine("[DEBUG] UI updated successfully");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ERROR] Load data error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"[ERROR] StackTrace: {ex.StackTrace}");

            await DisplayAlert("Debug Error", $"Lỗi load dữ liệu: {ex.Message}", "OK");
        }
    }

    private void OnCalculateBMIClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(HeightEntry.Text) ||
            string.IsNullOrWhiteSpace(WeightEntry.Text))
        {
            DisplayAlert("Lỗi", "Vui lòng nhập đầy đủ chiều cao và cân nặng!", "OK");
            return;
        }

        if (!double.TryParse(HeightEntry.Text, out double height) ||
            !double.TryParse(WeightEntry.Text, out double weight))
        {
            DisplayAlert("Lỗi", "Vui lòng nhập số hợp lệ!", "OK");
            return;
        }

        if (height <= 0 || height > 300)
        {
            DisplayAlert("Lỗi", "Chiều cao không hợp lệ! (0-300 cm)", "OK");
            return;
        }

        if (weight <= 0 || weight > 500)
        {
            DisplayAlert("Lỗi", "Cân nặng không hợp lệ! (0-500 kg)", "OK");
            return;
        }

        double heightInMeters = height / 100;
        double bmi = weight / (heightInMeters * heightInMeters);

        BMICalculated?.Invoke(bmi);

        BMIValueLabel.Text = bmi.ToString("F2");

        string status;
        Color statusColor;
        string description;

        if (bmi < 18.5)
        {
            status = "Thiếu Cân";
            statusColor = Color.FromArgb("#E74C3C");
            description = "Bạn nên tăng cân để đạt chỉ số lý tưởng";
        }
        else if (bmi < 25)
        {
            status = "Bình Thường";
            statusColor = Color.FromArgb("#27AE60");
            description = "Chỉ số BMI của bạn ở mức lý tưởng";
        }
        else if (bmi < 30)
        {
            status = "Thừa Cân";
            statusColor = Color.FromArgb("#F39C12");
            description = "Bạn nên giảm cân để cải thiện sức khỏe";
        }
        else
        {
            status = "Béo Phì";
            statusColor = Color.FromArgb("#E74C3C");
            description = "Bạn cần giảm cân để cải thiện sức khỏe";
        }

        BMIStatusLabel.Text = status;
        BMIStatusLabel.TextColor = statusColor;
        BMIValueLabel.TextColor = statusColor;
        BMIDescriptionLabel.Text = description;

        BMIFrame.IsVisible = true;

        HeightResultLabel.Text = $"Chiều cao: {height} cm";
        HeightResultLabel.IsVisible = true;

        WeightResultLabel.Text = $"Cân nặng: {weight} kg";
        WeightResultLabel.IsVisible = true;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(HeightEntry.Text) ||
            string.IsNullOrWhiteSpace(WeightEntry.Text))
        {
            await DisplayAlert("Lỗi", "Vui lòng nhập đầy đủ thông tin trước khi lưu!", "OK");
            return;
        }

        if (!double.TryParse(HeightEntry.Text, out double height) ||
            !double.TryParse(WeightEntry.Text, out double weight))
        {
            await DisplayAlert("Lỗi", "Dữ liệu không hợp lệ!", "OK");
            return;
        }

        try
        {
            int userId = Preferences.Get("UserId", 0);

            if (userId == 0)
            {
                await DisplayAlert("Lỗi", "Không xác định được người dùng. Vui lòng đăng nhập lại!", "OK");
                return;
            }

            var db = new DatabaseService();
            await db.InsertProgressAsync(userId, height, weight);

            await DisplayAlert("Thành Công", "Đã lưu dữ liệu vào cơ sở dữ liệu!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", $"Không thể lưu dữ liệu: {ex.Message}", "OK");
        }
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}