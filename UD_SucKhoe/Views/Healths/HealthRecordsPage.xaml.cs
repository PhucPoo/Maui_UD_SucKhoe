using UD_SucKhoe.Services.Database;

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
            int userId = Preferences.Get("UserId", 0);

            if (userId == 0)
            {
                await DisplayAlert("Lỗi", "Vui lòng đăng nhập lại!", "OK");
                return;
            }

            var latest = await _dbService.GetLatestProgressAsync(userId);

            if (latest != null)
            {
                WeightLabel.Text = $"Cân nặng: {latest.Weight} kg";
                HeightLabel.Text = $"Chiều cao: {latest.Height} cm";

                double heightInMeters = latest.Height / 100;
                double bmi = latest.Weight / (heightInMeters * heightInMeters);

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
        await Navigation.PushAsync(new MainPage());
    }
}
