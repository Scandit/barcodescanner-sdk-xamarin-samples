using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using ScanditBarcodePicker.Android;
using ScanditBarcodePicker.Android.Recognition;

namespace AndroidMatrixScanSample
{
    [Activity(Label = "ScanActivity")]
    public class ScanActivity : Activity, IOnScanListener, IProcessFrameListener
    {
        public static string appKey = "--- ENTER YOUR SCANDIT APP KEY HERE ---";

        private const int CameraPermissionRequest = 0;

        private BarcodePicker barcodePicker;
        private bool deniedCameraAccess = false;
        private bool paused = true;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            // Set the app key before instantiating the picker.
            ScanditLicense.AppKey = appKey;

            InitializeAndStartBarcodeScanning();
        }

        protected override void OnPause()
        {
            base.OnPause();

            // Call GC.Collect() before stopping the scanner as the garbage collector for some reason does not 
            // collect objects without references asap but waits for a long time until finally collecting them.
            GC.Collect();
            barcodePicker.StopScanning();
            paused = true;
        }

        void GrantCameraPermissionsThenStartScanning()
        {
            if (CheckSelfPermission(Manifest.Permission.Camera) != (int)Permission.Granted)
            {
                if (deniedCameraAccess == false)
                {
                    // It's pretty clear for why the camera is required. We don't need to give a
                    // detailed reason.
                    RequestPermissions(new String[] { Manifest.Permission.Camera }, CameraPermissionRequest);
                }

            }
            else
            {
                Console.WriteLine("starting scanning");
                // We already have the permission.
                barcodePicker.StartScanning();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == CameraPermissionRequest)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    deniedCameraAccess = false;
                    if (!paused)
                    {
                        barcodePicker.StartScanning();
                    }
                }
                else
                {
                    deniedCameraAccess = true;
                }
                return;
            }
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnResume()
        {
            base.OnResume();

            paused = false;
            // Handle permissions for Marshmallow and onwards.
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                GrantCameraPermissionsThenStartScanning();
            }
            else
            {
                // Once the activity is in the foreground again, restart scanning.
                barcodePicker.StartScanning();
            }
        }

        void InitializeAndStartBarcodeScanning()
        {
            // The scanning behavior of the barcode picker is configured through scan
            // settings. We start with empty scan settings and enable a generous set
            // of 1D symbologies. MatrixScan is currently only supported for 1D
            // symbologies, enabling 2D symbologies will result in unexpected results.
            // In your own apps, only enable the symbologies you actually need.
            ScanSettings settings = ScanSettings.Create();
            int[] symbologiesToEnable = new int[] {
                                            Barcode.SymbologyEan13,
                                            Barcode.SymbologyEan8,
                                            Barcode.SymbologyUpca,
                                            Barcode.SymbologyCode39,
                                            Barcode.SymbologyCode128,
                                            Barcode.SymbologyInterleaved2Of5,
                                            Barcode.SymbologyUpce
                                        };

            foreach (int symbology in symbologiesToEnable) 
            {
                settings.SetSymbologyEnabled (symbology, true);
            }

            // Enable MatrixScan and set the max number of barcodes that can be recognized per frame
            // to some reasonable number for your use case. The max number of codes per frame does not
            // limit the number of codes that can be tracked at the same time, it only limits the
            // number of codes that can be newly recognized per frame.
            settings.MatrixScanEnabled = true;
            settings.MaxNumberOfCodesPerFrame = 10;

            // Prefer the back-facing camera, if there is any.
            settings.CameraFacingPreference = ScanSettings.CameraFacingBack;

            barcodePicker = new BarcodePicker(this, settings);
            barcodePicker.OverlayView.SetGuiStyle(ScanOverlay.GuiStyleMatrixScan);

            // Set the GUI style to MatrixScan to see a visualization of the tracked barcodes. If you
            // would like to visualize it yourself, set it to ScanOverlay.GuiStyleNone and update your
            // visualization in the didProcess() callback.
            barcodePicker.OverlayView.SetGuiStyle(ScanOverlay.GuiStyleMatrixScan);

            // When using MatrixScan vibrating is often not desired.
            barcodePicker.OverlayView.SetVibrateEnabled(false);

            // Register listener, in order to be notified about relevant events 
            // (e.g. a successfully scanned bar code).
            barcodePicker.SetOnScanListener(this);

            // Register a process frame listener to be able to reject tracked codes.
            barcodePicker.SetProcessFrameListener(this);

            // Set listener for the scan event.
            barcodePicker.SetOnScanListener(this);

            // Show the scan user interface
            SetContentView(barcodePicker);
        }

        public void DidScan(IScanSession session)
        {
            // This callback acts the same as when not tracking and can be used for the events such as
            // when a code is newly recognized. Rejecting tracked codes has to be done in didProcess().
        }

        public void DidProcess(byte[] imageBuffer, int width, int height, IScanSession session)
        {
            foreach (TrackedBarcode code in session.TrackedCodes.Values)
            {
                if (code.Symbology == Barcode.SymbologyEan8)
                {
                    session.RejectTrackedCode(code);
                }
            }

            // If you want to implement your own visualization of the code tracking, you should update
            // it in this callback.
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
        }    
    }
}
