using Xamarin.Forms;
using System.Xml.Serialization;

namespace ExtendedSample
{
    public class MainPage : TabbedPage
    {
        private Settings settings;
        private ScannerPage scannerPage;
        private SettingsPage settingsPage;

        public MainPage()
        {
            settings = SettingsArchiver.UnarchiveSettings();
            scannerPage = new ScannerPage(settings);
            scannerPage.Title = "Scanner";
            settingsPage = new SettingsPage(settings);
            settingsPage.Title = "Settings";
            Children.Add(scannerPage);
            Children.Add(settingsPage);
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if (CurrentPage is ScannerPage)
            {
                settingsPage.UpdateSettings();
                scannerPage.ResumeScanning();
                SettingsArchiver.ArchiveSettings(settings);
            }
            else
            {
                scannerPage.PauseScanning();
            }
        }
    }
}
