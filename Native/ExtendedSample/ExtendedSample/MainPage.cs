using Xamarin.Forms;
using System.Xml.Serialization;

namespace ExtendedSample
{
    public class MainPage : TabbedPage
    {
        private Settings _settings;

        public MainPage()
        {
            _settings = SettingsArchiver.UnarchiveSettings();
			var scannerPage = new ScannerPage(_settings);
			scannerPage.Title = "Scanner";
            var settingsPage = new SettingsPage(_settings);
			settingsPage.Title = "Settings";
            Children.Add(scannerPage);
            Children.Add(settingsPage);
        }
    }
}
