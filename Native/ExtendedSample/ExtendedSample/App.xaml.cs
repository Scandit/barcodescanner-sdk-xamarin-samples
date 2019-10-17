using Xamarin.Forms;

namespace ExtendedSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            base.OnStart();

            ((MainPage)this.MainPage).StartScanning();
        }

        protected override void OnSleep()
        {
            base.OnSleep();

            ((MainPage)this.MainPage).StopScanning();
        }

        protected override void OnResume()
        {
            base.OnResume();

            ((MainPage)this.MainPage).StartScanning();
        }
    }
}
