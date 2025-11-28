using UD_SucKhoe.Services;

namespace UD_SucKhoe;
public partial class HealthRecordsPage : ContentPage
{
    private readonly DatabaseService _dbService;
    public HealthRecordsPage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
        LoadLatestProgressAsync();
    }
    private async Task LoadLatestProgressAsync()
    {
        try
        {
            // 👉 Lấy UserId từ Preferences (người dùng đã đăng nhập)
            int userId = Preferences.Get("UserId", 0);

            if (userId == 0)
            {
                await DisplayAlert("Lỗi", "Vui lòng đăng nhập lại!", "OK");
                return;
            }

            // 👉 Lấy dữ liệu mới nhất từ database với UserId đúng
            var latest = await _dbService.GetLatestProgressAsync(userId);

            if (latest != null)
            {
                WeightLabel.Text = $"Cân nặng: {latest.Weight} kg";
                HeightLabel.Text = $"Chiều cao: {latest.Height} cm";

                // 👉 Bonus: Tính và hiển thị BMI (nếu có Label)
                double heightInMeters = latest.Height / 100;
                double bmi = latest.Weight / (heightInMeters * heightInMeters);

                // Nếu bạn có BMILabel trong XAML thì uncomment dòng này:
                // BMILabel.Text = $"BMI: {bmi:F2}";
            }
            else
            {
                WeightLabel.Text = "Chưa có dữ liệu cân nặng";
                HeightLabel.Text = "Chưa có dữ liệu chiều cao";
                await DisplayAlert("Thông báo", "Chưa có dữ liệu trong hệ thống!\n\nVui lòng nhập thông tin trong phần 'Đo Lường'.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", $"Không thể tải dữ liệu: {ex.Message}", "OK");
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
