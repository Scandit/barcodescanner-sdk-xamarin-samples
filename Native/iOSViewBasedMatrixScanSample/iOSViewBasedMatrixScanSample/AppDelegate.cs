using Foundation;
using UIKit;
using ScanditBarcodeScanner.iOS;

namespace iOSViewBasedMatrixScanSample
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            License.SetAppKey("-- ENTER YOUR SCANDIT LICENSE KEY HERE --");
            return true;
        }
    }
}

