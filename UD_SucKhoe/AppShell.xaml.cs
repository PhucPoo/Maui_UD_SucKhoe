namespace UD_SucKhoe
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(NutritionPage), typeof(NutritionPage));
            Routing.RegisterRoute(nameof(ActivityPage), typeof(ActivityPage));
            Routing.RegisterRoute(nameof(MenuPage), typeof(MenuPage));
        }


    }
}