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
    private async void LoadLatestProgressAsync()
    {
        try
        {
            var latest = await _dbService.GetLatestProgressAsync(1); // 👈 UserID = 1 (hoặc truyền từ đăng nhập)
            if (latest != null)
            {
                WeightLabel.Text = $"Cân nặng: {latest.Weight} kg";
                HeightLabel.Text = $"Chiều cao: {latest.Height} cm";
            }
            else
            {
                await DisplayAlert("Thông báo", "Chưa có dữ liệu trong hệ thống!", "OK");
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
