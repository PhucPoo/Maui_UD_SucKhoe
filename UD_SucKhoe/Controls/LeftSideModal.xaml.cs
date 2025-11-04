namespace UD_SucKhoe.Controls;

public partial class LeftSideModal : ContentView
{
    public LeftSideModal()
    {
        InitializeComponent();
    }

    // Hiển thị modal với animation
    public async void Show()
    {
        this.IsVisible = true;

        // Fade in overlay
        Overlay.Opacity = 0;
        await Task.WhenAll(
            Overlay.FadeTo(0.5, 250),
            ModalPanel.TranslateTo(0, 0, 300, Easing.CubicOut)
        );
    }

    // Ẩn modal với animation
    public async void Hide()
    {
        await Task.WhenAll(
            Overlay.FadeTo(0, 250),
            ModalPanel.TranslateTo(-280, 0, 300, Easing.CubicIn)
        );

        this.IsVisible = false;
    }

    // Click vào overlay để đóng
    private void OnOverlayTapped(object sender, EventArgs e)
    {
        Hide();
    }

    // Click nút đóng
    private void OnCloseButtonTapped(object sender, EventArgs e)
    {
        Hide();
    }

    // Các menu item events
    private void OnMenuItem1Tapped(object sender, EventArgs e)
    {
        // TODO: Navigate to home
        DisplayAlert("Menu", "Trang chủ được chọn", "OK");
        Hide();
    }

    private void OnMenuItem2Tapped(object sender, EventArgs e)
    {
        // TODO: Navigate to settings
        DisplayAlert("Menu", "Cài đặt được chọn", "OK");
        Hide();
    }

    private void OnMenuItem3Tapped(object sender, EventArgs e)
    {
        // TODO: Navigate to profile
        DisplayAlert("Menu", "Hồ sơ được chọn", "OK");
        Hide();
    }

    private void OnMenuItem4Tapped(object sender, EventArgs e)
    {
        // TODO: Navigate to about
        DisplayAlert("Menu", "Giới thiệu được chọn", "OK");
        Hide();
    }

    private async void DisplayAlert(string title, string message, string cancel)
    {
        var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (currentPage != null)
        {
            await currentPage.DisplayAlert(title, message, cancel);
        }
    }
}