using Xamarin.Forms;
using System.Xml.Serialization;

namespace ExtendedSample
{
    public class MainPage : TabbedPage
    {
        private Settings settings;

        public MainPage()
        {
            settings = SettingsArchiver.UnarchiveSettings();
			var scannerPage = new ScannerPage(settings);
			scannerPage.Title = "Scanner";
            var settingsPage = new SettingsPage(settings);
			settingsPage.Title = "Settings";
            Children.Add(scannerPage);
            Children.Add(settingsPage);
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
			SettingsArchiver.ArchiveSettings(settings);
		}
    }
}
