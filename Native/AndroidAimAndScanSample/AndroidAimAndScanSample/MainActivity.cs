using Android.App;
using Android.Widget;
using Android.OS;
using ScanditBarcodePicker.Android;
using ScanditBarcodePicker.Android.Recognition;
using Android.Views;
using Android;
using Android.Content;
using System;
using Android.Content.PM;
using Android.Graphics;

namespace AndroidAimAndScanSample
{
    [Activity(Label = "AndroidAimAndScanSample", MainLauncher = true, Icon = "@mipmap/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity, IOnScanListener, IDialogInterfaceOnCancelListener
    {
        private const string KEY = "AfILn1k/RXRZIumlNSAF4JEtIT0SMGJQunossSJgcNtMLM1SkR2B6l1nPX+WQYdRUEx/KegqsxEYftIzZXc8jJhLyUs/VthOmjqFJK5ny6O3T11qgR6bDc0SUoUZeYr6O/L/PZuXQQoGDjozSju29lg5OriwAxftC2XYovyu/ilQgkPopb1t7SXXqDoz9NiYOhRFXhTxRUrlYSu/364gz78bhDyd67oKk0tvXhTcHKD9I6buOvV+Pz4tf7aWNAzoW5RuWgWo+aMlBp+hMPj5T6Y8siuqCpBtuSFY9XCSX/ts9AuAD0v6medmVS3aQ7Tsi8r9X0DPfqe6bcxnpg4GjvgkZ/f+/uya9hlBdTrp2RdYrG9ItrxUN4Pe+vWnayz0SgEGjD+BJAdRSEGTf/yfZSb9wPJL1QVB99nktK25B5ni4E9J9mtCg1SoJc63WEpjEWZ1s8KQavs3KGkGDSkObs+k3C3MkwjvWq2oUF8O/Yjo3vNd+c+ZJ4Don5HXGlufhs1OiUV1tDrHa1wdlXhZOqzlm4xbPVqUsMk8QawJd/casDT2xMJF8qlM/cm5uUkzeFa0U/9cM6e71KQMweWd3s26DOxHYo1ERi/IGur9Bsa68R80kbQmii3ZNWq2R8JQI6WvWbBHJ268O5ACMpBpF6+bRPtxh+9mmorgv7sPgkr/V0QUs4tOl/hBSZFJp5BfruB7gCtQ2zxT5DrozG6MW06/UVETXoIlTfG2/zQH5rIhVpNfQRrG4ncJtqMUFsDMUrBGcNOWFrgJ7x0x1dn5JxctKoWWIYRr83o=";

        private const int CAMERA_REQUEST = 1;

        BarcodePicker barcodePicker;
        View scanButton;
        private bool paused = true;
        private bool scanning = false;

        public void DidScan(IScanSession session)
        {
            if (session.NewlyRecognizedCodes.Count > 0) {
                Barcode code = session.NewlyRecognizedCodes[0];
                Console.WriteLine("barcode scanned: {0}, '{1}'", code.SymbologyName, code.Data);

                // Call GC.Collect() before stopping the scanner as the garbage collector for some reason does not
                // collect objects without references asap but waits for a long time until finally collecting them.
                GC.Collect();

                // Stop the scanner directly on the session.
                session.PauseScanning();

                // If you want to edit something in the view hierarchy make sure to run it on the UI thread.
                RunOnUiThread(() => {
                    AlertDialog alert = new AlertDialog.Builder(this)
                        .SetTitle(code.SymbologyName + " Barcode Detected")
                        .SetMessage(code.Data)
                        .SetPositiveButton("OK", delegate {
                            scanButton.Visibility = ViewStates.Visible;
                        })
                        .SetOnCancelListener(this)
                        .Create();

                    alert.Show();
                });
            }
        }

        public void OnCancel(IDialogInterface dialog)
        {
            scanButton.Visibility = ViewStates.Visible;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.Main);
            scanButton = FindViewById(Resource.Id.start_button);
            scanButton.Click += delegate {
                scanning = true;
                GrantCameraPermissionsThenStartScanning();
                scanButton.Visibility = ViewStates.Gone;
            };

            ScanditLicense.AppKey = KEY;
            InitializeBarcodeScanning();
        }

        protected override void OnResume()
        {
            base.OnResume();

            paused = false;
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                GrantCameraPermissionsThenStartScanning();
            }
            else
            {
                barcodePicker.StartScanning();
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            GC.Collect();
            barcodePicker.StopScanning();
            paused = true;
        }

        void GrantCameraPermissionsThenStartScanning()
        {
            if (CheckSelfPermission(Manifest.Permission.Camera) != (int)Permission.Granted)
            {
                RequestPermissions(new String[] { Manifest.Permission.Camera }, CAMERA_REQUEST);
            }
            else
            {
                StartPicker();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == CAMERA_REQUEST)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    if (!paused)
                    {
                        StartPicker();
                    }
                }
                return;
            }
        }

        void StartPicker()
        {
            // Start the picker paused if we don't want to actively scan yet.
            barcodePicker.StartScanning(!scanning);
        }

        void StopPicker()
        {
            barcodePicker.StopScanning();
            scanning = false;
        }

        void InitializeBarcodeScanning()
        {
            ScanSettings settings = ScanSettings.Create();
            int[] symbologiesToEnable = {
                Barcode.SymbologyEan13,
                Barcode.SymbologyEan8,
                Barcode.SymbologyUpca,
                Barcode.SymbologyUpce,
                Barcode.SymbologyDataMatrix,
                Barcode.SymbologyQr,
                Barcode.SymbologyCode39,
                Barcode.SymbologyCode128,
                Barcode.SymbologyInterleaved2Of5
            };

            foreach (int symbology in symbologiesToEnable)
            {
                settings.SetSymbologyEnabled(symbology, true);
            }

            // Enable and set the restrict active area. This will make sure that codes are only scanned in a very thin band in the center of the image.
            settings.SetActiveScanningArea(ScanSettings.OrientationPortrait, new RectF(0f, 0.48f, 1f, 0.52f));

            // Setup the barcode scanner
            barcodePicker = new BarcodePicker(this, settings);

            // Add listener for the scan event. We keep references to the
            // delegates until the picker is no longer used as the delegates are softly
            // referenced and can be removed because of low memory.
            barcodePicker.SetOnScanListener(this);

            // Modify the GUI style to have a "laser" line instead of a square viewfinder.
            barcodePicker.OverlayView.SetGuiStyle(ScanOverlay.GuiStyleLaser);

            // Start scanning in paused state.
            barcodePicker.StartScanning(true);

            (FindViewById(Resource.Id.picker_container) as FrameLayout).AddView(barcodePicker, 0);

            GrantCameraPermissionsThenStartScanning();
        }

    }
}

