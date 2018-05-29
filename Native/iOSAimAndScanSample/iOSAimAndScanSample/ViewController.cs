using System;
using UIKit;
using Foundation;
using CoreMedia;
using ScanditBarcodeScanner.iOS;

namespace iOSAimAndScanSample
{
    public partial class ViewController : UIViewController
    {
        public static string appKey = "-- ENTER YOUR SCANDIT LICENSE KEY HERE --";

        BarcodePicker picker;
        PickerScanDelegate scanDelegate;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Set the app key before instantiating the picker.
            License.SetAppKey(appKey);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // The scanning behavior of the barcode picker is configured through scan
            // settings. We start with empty scan settings and enable a very generous
            // set of symbologies. In your own apps, only enable the symbologies you
            // actually need.
            ScanSettings settings = ScanSettings.DefaultSettings();
            NSSet symbologiesToEnable = new NSSet(
                Symbology.EAN13,
                Symbology.EAN8,
                Symbology.UPC12,
                Symbology.UPCE,
                Symbology.Datamatrix,
                Symbology.QR,
                Symbology.Code39,
                Symbology.Code128,
                Symbology.ITF
            );
            settings.EnableSymbologies(symbologiesToEnable);

            // Enable and set the restrict active area. This will make sure that codes are only scanned in a very thin band in the center of the image.
            settings.SetActiveScanningArea(new CoreGraphics.CGRect(0, 0.48, 1, 0.04));

            // Setup the barcode scanner
            picker = new BarcodePicker(settings);
            picker.OverlayView.ShowToolBar(false);

            // Add delegate for the scan event. We keep references to the
            // delegates until the picker is no longer used as the delegates are softly
            // referenced and can be removed because of low memory.
            scanDelegate = new PickerScanDelegate();
            picker.ScanDelegate = scanDelegate;

            AddChildViewController(picker);
            picker.View.Frame = View.Bounds;
            Add(picker.View);
            picker.DidMoveToParentViewController(this);

            // Modify the GUI style to have a "laser" line instead of a square viewfinder.
            picker.OverlayView.GuiStyle = GuiStyle.Laser;

            // Start scanning in paused state.
            picker.StartScanning(true);

            showAimAndScanButton();
        }

        private void showAimAndScanButton()
        {
            // Add a button which would enable a user to resume scanning.
            UIButton button = new UIButton();
            var margins = View.LayoutMarginsGuide;
            button.TouchUpInside += (object sender, System.EventArgs e) =>
            {
                picker.ResumeScanning();
            };
            UIColor brandColor = new UIColor(57f / 255f, 193f / 255f, 204f / 255f, 1f);
            button.BackgroundColor = brandColor;
            button.SetTitle("Scan Barcodes", UIControlState.Normal);
            button.TranslatesAutoresizingMaskIntoConstraints = false;
            picker.View.AddSubview(button);
            NSLayoutConstraint trailingConstraint = NSLayoutConstraint.Create(button, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, View, NSLayoutAttribute.Trailing, 1, -8);
            NSLayoutConstraint bottomConstraint = NSLayoutConstraint.Create(button, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, View, NSLayoutAttribute.Bottom, 1, -8);
            NSLayoutConstraint leadingConstraint = NSLayoutConstraint.Create(button, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, View, NSLayoutAttribute.Leading, 1, 8);
            NSLayoutConstraint heightConstraint = NSLayoutConstraint.Create(button, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, 60);
            View.AddConstraints(new NSLayoutConstraint[] {heightConstraint, leadingConstraint, bottomConstraint, trailingConstraint});
        }

        public class PickerScanDelegate : ScanDelegate
        {

            public override void DidScan(BarcodePicker picker, IScanSession session)
            {
                if (session.NewlyRecognizedCodes.Count > 0)
                {
                    Barcode code = session.NewlyRecognizedCodes.GetItem<Barcode>(0);
                    Console.WriteLine("barcode scanned: {0}, '{1}'", code.SymbologyString, code.Data);

                    // Pause the scanner directly on the session.
                    session.PauseScanning();

                    // If you want to edit something in the view hierarchy make sure to run it on the UI thread.
                    UIApplication.SharedApplication.InvokeOnMainThread(() => {
                        UIAlertView alert = new UIAlertView()
                        {
                            Title = code.SymbologyString + " Barcode Detected",
                            Message = "" + code.Data
                        };
                        alert.AddButton("OK");

                        alert.Show();
                    });
                }
            }
        }

    }
}