using System;
using UIKit;
using Foundation;
using CoreMedia;
using ScanditBarcodeScanner.iOS;

namespace iOSMatrixScanSample
{
    public partial class ViewController : UIViewController
    {
		public static string appKey = "--- ENTER YOUR SCANDIT APP KEY HERE ---";

		PickerScanDelegate scanDelegate;
        PickerProcessFrameDelegate processFrameDelegate;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Set the app key before instantiating the picker.
            License.SetAppKey(appKey);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // The scanning behavior of the barcode picker is configured through scan
            // settings. We start with empty scan settings and enable a generous set
            // of 1D symbologies. Matrix scan is currently only supported for 1D
            // symbologies, enabling 2D symbologies will result in unexpected results.
            // In your own apps, only enable the symbologies you actually need.
            ScanSettings settings = ScanSettings.DefaultSettings();
            NSSet symbologiesToEnable = new NSSet(
                Symbology.EAN13,
                Symbology.EAN8,
                Symbology.UPC12,
                Symbology.UPCE,
                Symbology.Code39,
                Symbology.Code128,
                Symbology.ITF
            );

            // Enable matrix scan and set the max number of barcodes that can be recognized per frame
            // to some reasonable number for your use case. The max number of codes per frame does not
            // limit the number of codes that can be tracked at the same time, it only limits the
            // number of codes that can be newly recognized per frame.
            settings.EnableSymbologies(symbologiesToEnable);
            //settings.MatrixScanEnabled = true;
            settings.MaxNumberOfCodesPerFrame = 10;
            settings.HighDensityModeEnabled = true;

            // When matrix scan is enabled beeping/vibrating is often not wanted.
            BarcodePicker picker = new BarcodePicker(settings);
            picker.OverlayView.SetBeepEnabled(false);
            picker.OverlayView.SetVibrateEnabled(false);

            // Register a SBSScanDelegate delegate, in order to be notified about relevant events
            // (e.g. a successfully scanned bar code).
            scanDelegate = new PickerScanDelegate();
            picker.ScanDelegate = scanDelegate;

            // Register a SBSProcessFrameDelegate delegate to be able to reject tracked codes.
            processFrameDelegate = new PickerProcessFrameDelegate();
            picker.ProcessFrameDelegate = processFrameDelegate;

            AddChildViewController(picker);
            picker.View.Frame = View.Bounds;
            Add(picker.View);
            picker.DidMoveToParentViewController(this);

            picker.OverlayView.GuiStyle = GuiStyle.MatrixScan;

            picker.StartScanning();
        }

        public class PickerScanDelegate : ScanDelegate
        {
            public override void DidScan(BarcodePicker picker, IScanSession session)
            {
                // This delegate method acts the same as when not in matrix scan and can be used for the events such as
                // when a code is newly recognized. Rejecting tracked codes has to be done in barcodePicker(_:didProcessFrame:session:).
            }
        }

        public class PickerProcessFrameDelegate : ProcessFrameDelegate
        {
            public override void DidCaptureImage(BarcodePicker picker, CMSampleBuffer frame, IScanSession session)
            {
                // For each tracked codes in the last processed frame.

                foreach (TrackedBarcode code in session.TrackedCodes.Values)
                {
                    // As an example, let's visually reject all EAN8 codes.
                    if (code.Symbology == Symbology.EAN8) {
                        session.RejectTrackedCode(code);
                    }
                }

                // If you want to implement your own visualization of the code matrix scan, 
                // you should update it in this callback.
            }
        }
    }
}
