using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Scandit;
using ScanditBarcodePicker.Android;
using ScanditBarcodePicker.Android.Recognition;

namespace XamarinScanditSDKSampleAndroid
{
	[Activity (Label = "ScanActivity")]			
	public class ScanActivity : Activity, IOnScanListener, IDialogInterfaceOnCancelListener
	{
		private BarcodePicker picker;
		public static string appKey = "-- ENTER YOUR SCANDIT LICENSE KEY HERE --";
		private const int CameraRequestPermission = 0; // this int will be returned when we are granted permission
		private bool mDeniedCameraAccess = false;
		private bool mPaused = true;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			// Set the app key before instantiating the picker.
			ScanditLicense.AppKey = appKey;
			
			// The scanning behavior of the barcode picker is configured through scan
			// settings. We start with empty scan settings and enable a very generous
			// set of symbologies. In your own apps, only enable the symbologies you
			// actually need.
			ScanSettings settings = ScanSettings.Create ();
			int[] symbologiesToEnable = new int[] {
				Barcode.SymbologyEan13,
				Barcode.SymbologyEan8,
				Barcode.SymbologyUpca,
				Barcode.SymbologyDataMatrix,
				Barcode.SymbologyQr,
				Barcode.SymbologyCode39,
				Barcode.SymbologyCode128,
				Barcode.SymbologyInterleaved2Of5,
				Barcode.SymbologyUpce
			};
			
			for (int sym = 0; sym < symbologiesToEnable.Length; sym++) {
				settings.SetSymbologyEnabled (symbologiesToEnable[sym], true);
			}	
			
			// Some 1d barcode symbologies allow you to encode variable-length data. By default, the
			// Scandit BarcodeScanner SDK only scans barcodes in a certain length range. If your
			// application requires scanning of one of these symbologies, and the length is falling
			// outside the default range, you may need to adjust the "active symbol counts" for this
			// symbology. This is shown in the following few lines of code.

			SymbologySettings symSettings = settings.GetSymbologySettings(Barcode.SymbologyCode128);
			short[] activeSymbolCounts = new short[] {
				7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20
			};
			symSettings.SetActiveSymbolCounts(activeSymbolCounts);
			// For details on defaults and how to calculate the symbol counts for each symbology, take
			// a look at http://docs.scandit.com/stable/c_api/symbologies.html.

			picker = new BarcodePicker (this, settings);

			// Set listener for the scan event.
			picker.SetOnScanListener (this);
			
			// Show the scan user interface
			SetContentView (picker);
		}

		public void DidScan(IScanSession session) 
		{
			if (session.NewlyRecognizedCodes.Count > 0) {
				Barcode code = session.NewlyRecognizedCodes [0];
				Console.WriteLine ("barcode scanned: {0}, '{1}'", code.SymbologyName, code.Data);

				// Call GC.Collect() before stopping the scanner as the garbage collector for some reason does not 
				// collect objects without references asap but waits for a long time until finally collecting them.
				GC.Collect ();

				// Stop the scanner directly on the session.
				session.StopScanning ();

				// If you want to edit something in the view hierarchy make sure to run it on the UI thread.
				RunOnUiThread (() => {
					AlertDialog alert = new AlertDialog.Builder (this)
						.SetTitle (code.SymbologyName + " Barcode Detected")
						.SetMessage (code.Data)
						.SetPositiveButton("OK", delegate {
							picker.StartScanning ();
						})
						.SetOnCancelListener(this)
						.Create ();

					alert.Show ();
				});
			}
		}

		private void grantCameraPermissionsThenStartScanning()
		{
			Console.WriteLine("get permission");
			if (CheckSelfPermission(Manifest.Permission.Camera)
				!= Permission.Granted)
			{
				Console.WriteLine("get permission1");

				if (mDeniedCameraAccess == false)
				{
					Console.WriteLine("get permission2");

					// it's pretty clear for why the camera is required. We don't need to give a
					// detailed reason.
					RequestPermissions(new String[] { Manifest.Permission.Camera },
									   CameraRequestPermission);
				}

			}
			else {
				Console.WriteLine("get permission3 " + CheckSelfPermission(Manifest.Permission.Camera));

				// we already have the permission
				picker.StartScanning();
			}
		}

		override public void OnRequestPermissionsResult(int requestCode,
									   string[] permissions, Permission[] grantResults)
		{
			Console.WriteLine("got permission");
			if (requestCode == CameraRequestPermission)
			{
				if (grantResults.Length > 0
				    && grantResults[0] == Permission.Granted)
				{
					mDeniedCameraAccess = false;
					if (!mPaused)
					{
						picker.StartScanning();
					}
				}
				else {
					mDeniedCameraAccess = true;
				}
				return;
			}
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		public void OnCancel(IDialogInterface dialog) {
			picker.StartScanning ();
		}

		protected override void OnResume () 
		{
			Console.WriteLine("resume");
			mPaused = false;
			// handle permissions for Marshmallow and onwards...
			if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
			{
				grantCameraPermissionsThenStartScanning();
			}
			else {
				// Once the activity is in the foreground again, restart scanning.
				picker.StartScanning();
			}
			base.OnResume ();
		}

		protected override void OnPause () 
		{
			// Call GC.Collect() before stopping the scanner as the garbage collector for some reason does not 
			// collect objects without references asap but waits for a long time until finally collecting them.
			GC.Collect ();
			picker.StopScanning ();
			base.OnPause();
			mPaused = true;
		}

		public override void OnBackPressed () 
		{
			base.OnBackPressed ();
			Finish ();
		}
	}
}

