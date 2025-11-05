namespace UD_SucKhoe
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

        }

        private async void OnMenuTapped(object sender, EventArgs e)
        {
            // Hiển thị menu
            await DisplayAlert("Menu", "Menu clicked", "OK");
        }

        private async void OnLoginTapped(object sender, EventArgs e)
        {
            // Thử cách navigation Modal thay vì Shell navigation
            try
            {
                var loginPage = new LoginPage();

                var currentWindow = Application.Current?.Windows.FirstOrDefault();
                if (currentWindow?.Page != null)
                {
                    await currentWindow.Page.Navigation.PushModalAsync(loginPage);
                }
                else
                {
                    await DisplayAlert("Lỗi", "Unable to navigate: Current window or page is null.", "OK");
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                await DisplayAlert("Lỗi", ex.Message, "OK");
            }
        }
    }
}