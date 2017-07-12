using Xamarin.Forms;
using System.Xml.Serialization;

namespace ExtendedSample
{
    public partial class ScannerPage : ContentPage
    {
        private Settings settings;

        public ScannerPage(Settings settings) {
            this.settings = settings;
			InitializeComponent();
		}
    }
}
