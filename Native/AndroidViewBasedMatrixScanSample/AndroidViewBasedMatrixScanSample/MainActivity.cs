using Android.App;
using Android.OS;
using Android.Support.V7.App;
using ScanditBarcodePicker.Android;

namespace AndroidViewBasedMatrixScanSample
{
    /*
     * This example shows how to use the view-based Matrix Scan of the Scandit BarcodeScanner SDK, which
     * is a high-level abstraction of the base Scandit Matrix Scan.
     *
     * The sample demonstrates how the view-based Matrix Scan can be used to track multiple barcodes
     * simultaneously and how to apply multiple overlays over the detected barcodes.
     */
    [Activity(Label = "AndroidViewBasedMatrixScanSample", MainLauncher = true, Icon = "@mipmap/ic_launcher")]
    public class MainActivity : AppCompatActivity
    {
        // Enter your Scandit SDK License key here.
        // Your Scandit SDK License key is available via your Scandit SDK web account.
        private const string scanditSdkAppKey = "-- ENTER YOUR SCANDIT LICENSE KEY HERE --";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set the app key before instantiating the picker.
            ScanditLicense.AppKey = scanditSdkAppKey;

            SetContentView(Resource.Layout.Main);

            var fragmentManager = SupportFragmentManager;
            fragmentManager.BeginTransaction()
                           .Add(Resource.Id.fragment_container, new ShelfManagementFragment())
                           .Commit();
        }
    }
}

