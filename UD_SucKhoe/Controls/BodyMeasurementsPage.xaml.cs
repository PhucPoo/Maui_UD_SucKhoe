using UD_SucKhoe.Services;

namespace UD_SucKhoe;

public partial class BodyMeasurementsPage : ContentPage
{
    public BodyMeasurementsPage()
    {
        InitializeComponent();
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
            var db = new DatabaseService();

            // 👇 Thay ID người dùng thực tế nếu có
            await db.InsertProgressAsync(userId: 1, height: height, weight: weight);

            await DisplayAlert("Thành Công", "Đã lưu dữ liệu vào cơ sở dữ liệu!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", $"Không thể lưu dữ liệu: {ex.Message}", "OK");
        }
    }


    private async void OnCloseClicked(object sender, EventArgs e)
    {
        // Đóng modal page
        await Navigation.PopModalAsync();
    }
}