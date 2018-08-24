using Android.App;
using Android.Widget;
using Android.OS;
using ScanditBarcodePicker.Android.Matrixscan;
using Java.Lang;
using ScanditBarcodePicker.Android.Recognition;
using Android.Graphics;
using Android.Views;

namespace XamarinScanditSDKSampleAndroid
{
    [Activity (Label = "XamarinScanditSDKSampleAndroid", MainLauncher = true)]
    public class MainActivity : Activity, ViewBasedMatrixScanOverlay.IViewBasedMatrixScanOverlayListener
    {
        public Point GetOffsetForCode(TrackedBarcode p0, long p1)
        {
            throw new System.NotImplementedException();
        }

        public View GetViewForCode(TrackedBarcode p0, long p1)
        {
            throw new System.NotImplementedException();
        }

        public void OnCodeTouched(TrackedBarcode p0, long p1)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button> (Resource.Id.myButton);
            
            button.Click += delegate {
                // start the scanner
                StartActivity(typeof(ScanActivity));
            };
        }
    }
}


