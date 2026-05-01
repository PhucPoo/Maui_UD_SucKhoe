namespace UD_SucKhoe
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; set; }

        [Obsolete]
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Services = serviceProvider;
        }


        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}