using UD_SucKhoe.Services.Database;

namespace UD_SucKhoe;

public partial class BodyMeasurementsPage : ContentPage
{
    public event Action<double> BMICalculated;

    public BodyMeasurementsPage()
    {
        InitializeComponent();
    }

    // 👉 Tự động load dữ liệu khi trang xuất hiện
    protected override async void OnAppearing()
    {
        base.OnAppearing();



        // Delay nhỏ để đảm bảo UI đã render xong
        await Task.Delay(100);
        await LoadLatestDataAsync();
    }

    // 👉 Method load dữ liệu mới nhất từ DB
    private async Task LoadLatestDataAsync()
    {
        try
        {
            int userId = Preferences.Get("UserId", 0);

            System.Diagnostics.Debug.WriteLine($"[DEBUG] UserId from Preferences: {userId}");

            if (userId == 0)
            {
                System.Diagnostics.Debug.WriteLine("[DEBUG] UserId = 0, user not logged in");
                return; // Chưa đăng nhập thì bỏ qua
            }

            var db = new DatabaseService();
            var latestProgress = await db.GetLatestProgressAsync(userId);

            System.Diagnostics.Debug.WriteLine($"[DEBUG] Latest progress data: {(latestProgress == null ? "NULL" : $"Height={latestProgress.Height}, Weight={latestProgress.Weight}")}");

            // Kiểm tra nếu không có dữ liệu thì return
            if (latestProgress == null)
            {
                System.Diagnostics.Debug.WriteLine("[DEBUG] No data found in database");
                return;
            }

            // Điền thông tin vào Entry
            HeightEntry.Text = latestProgress.Height.ToString();
            WeightEntry.Text = latestProgress.Weight.ToString();

            System.Diagnostics.Debug.WriteLine($"[DEBUG] Set HeightEntry: {latestProgress.Height}, WeightEntry: {latestProgress.Weight}");

            // Tự động tính BMI
            double heightInMeters = latestProgress.Height / 100;
            double bmi = latestProgress.Weight / (heightInMeters * heightInMeters);

            BMICalculated?.Invoke(bmi);

            // Hiển thị kết quả BMI
            BMIValueLabel.Text = bmi.ToString("F2");

            // Xác định trạng thái BMI
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
            // Hiển thị lỗi thay vì silent fail
            System.Diagnostics.Debug.WriteLine($"[ERROR] Load data error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"[ERROR] StackTrace: {ex.StackTrace}");

            // Hiển thị alert để debug
            await DisplayAlert("Debug Error", $"Lỗi load dữ liệu: {ex.Message}", "OK");
        }
    }

    private void OnCalculateBMIClicked(object sender, EventArgs e)
    {
        // Kiểm tra input
        if (string.IsNullOrWhiteSpace(HeightEntry.Text) ||
            string.IsNullOrWhiteSpace(WeightEntry.Text))
        {
            DisplayAlert("Lỗi", "Vui lòng nhập đầy đủ chiều cao và cân nặng!", "OK");
            return;
        }

        // Parse giá trị
        if (!double.TryParse(HeightEntry.Text, out double height) ||
            !double.TryParse(WeightEntry.Text, out double weight))
        {
            DisplayAlert("Lỗi", "Vui lòng nhập số hợp lệ!", "OK");
            return;
        }

        // Kiểm tra giá trị hợp lệ
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

        // Tính BMI (chiều cao cần chuyển từ cm sang m)
        double heightInMeters = height / 100;
        double bmi = weight / (heightInMeters * heightInMeters);

        BMICalculated?.Invoke(bmi);

        // Hiển thị kết quả
        BMIValueLabel.Text = bmi.ToString("F2");

        // Xác định trạng thái BMI
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

        // Hiển thị thông tin đã nhập
        HeightResultLabel.Text = $"Chiều cao: {height} cm";
        HeightResultLabel.IsVisible = true;

        WeightResultLabel.Text = $"Cân nặng: {weight} kg";
        WeightResultLabel.IsVisible = true;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Kiểm tra xem đã nhập thông tin chưa
        if (string.IsNullOrWhiteSpace(HeightEntry.Text) ||
            string.IsNullOrWhiteSpace(WeightEntry.Text))
        {
            await DisplayAlert("Lỗi", "Vui lòng nhập đầy đủ thông tin trước khi lưu!", "OK");
            return;
        }

        // Parse giá trị để validate
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