namespace UD_SucKhoe;
public partial class HealthRecordsPage : ContentPage
{
    public HealthRecordsPage()
    {
        InitializeComponent();
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
