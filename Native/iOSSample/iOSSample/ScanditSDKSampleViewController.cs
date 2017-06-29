using System;
using System.Drawing;
using Foundation;
using UIKit;
using ScanditBarcodeScanner.iOS;


namespace iOSSample
{
	public partial class ScanditSDKSampleViewController : UIViewController
	{
		public static string appKey = "--- ENTER YOUR SCANDIT LICENSE KEY HERE ---";

		PickerScanDelegate scanDelegate;
		OverlayCancelDelegate cancelDelegate;

		public ScanditSDKSampleViewController () : base ("ScanditSDKSampleViewController", null)
		{
			// Set the app key before instantiating the picker.
			License.SetAppKey(appKey);
		}

		partial void scanButtonClicked (Foundation.NSObject sender)
		{

			// The scanning behavior of the barcode picker is configured through scan
			// settings. We start with empty scan settings and enable a very generous
			// set of symbologies. In your own apps, only enable the symbologies you
			// actually need.
			ScanSettings settings = ScanSettings.DefaultSettings ();
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


			// Some 1d barcode symbologies allow you to encode variable-length data. By default, the
			// Scandit BarcodeScanner SDK only scans barcodes in a certain length range. If your
			// application requires scanning of one of these symbologies, and the length is falling
			// outside the default range, you may need to adjust the "active symbol counts" for this
			// symbology. This is shown in the following 3 lines of code.

			NSMutableSet codeLengths = new NSMutableSet();
			int i = 0;
			for (i= 7; i <= 20; i++) {
				codeLengths.Add (new NSNumber (i));
			}
			settings.SettingsForSymbology (Symbology.Code128).ActiveSymbolCounts = codeLengths;
			// For details on defaults and how to calculate the symbol counts for each symbology, take
			// a look at http://docs.scandit.com/stable/c_api/symbologies.html.

			// Setup the barcode scanner
			BarcodePicker picker = new BarcodePicker (settings);
			picker.OverlayView.ShowToolBar (true);

			// Add delegates for the scan and cancel event. We keep references to the
			// delegates until the picker is no longer used as the delegates are softly
			// referenced and can be removed because of low memory.
			scanDelegate = new PickerScanDelegate();
			picker.ScanDelegate = scanDelegate;

			cancelDelegate = new OverlayCancelDelegate(this, picker);
			picker.OverlayView.CancelDelegate = cancelDelegate;

			PresentViewController (picker, true, null);

			picker.StartScanning ();
		}

		public class PickerScanDelegate : ScanDelegate {

			public override void DidScan (BarcodePicker picker, IScanSession session) {
				if (session.NewlyRecognizedCodes.Count > 0) {
					Barcode code = session.NewlyRecognizedCodes.GetItem<Barcode> (0);
					Console.WriteLine ("barcode scanned: {0}, '{1}'", code.SymbologyString, code.Data);

					// Stop the scanner directly on the session.
					session.StopScanning ();

					// If you want to edit something in the view hierarchy make sure to run it on the UI thread.
					UIApplication.SharedApplication.InvokeOnMainThread(() => {
						UIAlertView alert = new UIAlertView () { 
							Title = code.SymbologyString + " Barcode Detected", Message = "" + code.Data
						};
						alert.AddButton ("OK");

						alert.Clicked += (object o, UIButtonEventArgs e) => {
							picker.StartScanning ();
						};

						alert.Show ();
					});
				}
			}
		}

		public class OverlayCancelDelegate : CancelDelegate {

			UIViewController presentingViewController;
			BarcodePicker picker;

			public OverlayCancelDelegate(UIViewController controller, BarcodePicker picker) {
				presentingViewController = controller;
				this.picker = picker;
			}

			public override void DidCancel(ScanOverlay overlay, NSDictionary status) {
				Console.WriteLine ("Cancel was pressed.");
				presentingViewController.DismissViewController (true, null);
				picker.StopScanning();
			}
		}
	}
}

