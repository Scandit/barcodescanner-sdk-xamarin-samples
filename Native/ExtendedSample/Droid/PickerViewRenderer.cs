using System;
using Android.Content;
using ExtendedSample;
using ExtendedSample.Droid;
using ScanditBarcodePicker.Android;
using ScanditBarcodePicker.Android.Recognition;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PickerView), typeof(PickerViewRenderer))]
namespace ExtendedSample.Droid
{
    public class PickerViewRenderer : ViewRenderer<PickerView, Android.Views.View>
    {
        private BarcodePicker barcodePicker;
        private PickerOnScanListener onScanListener;
        private PickerView pickerView;
        private Context context;

        public PickerViewRenderer(Context context) : base(context)
        {
            this.context = context;

            ScanditLicense.AppKey = PickerView.GetAppKey();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<PickerView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                pickerView = e.NewElement;

                e.NewElement.StartScanningRequested += OnStartScanningRequested;
                e.NewElement.PauseScanningRequested += OnPauseScanningRequested;

                barcodePicker = new BarcodePicker(context, CreateScanSettings());
                SetNativeControl(barcodePicker.OverlayView);
                barcodePicker.StartScanning();
                onScanListener = new PickerOnScanListener
                {
                    PickerView = pickerView,
                    ContinuousAfterScan = pickerView.Settings.ContinuousAfterScan
                };
                barcodePicker.SetOnScanListener(onScanListener);

                ApplyOverlaySettings();
                barcodePicker.StartScanning();
            }
            if (e.OldElement != null)
            {
                e.OldElement.StartScanningRequested -= OnStartScanningRequested;
                e.OldElement.PauseScanningRequested -= OnPauseScanningRequested;
            }
        }

        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            return new SizeRequest(new Size(widthConstraint, heightConstraint),
                                   new Size(widthConstraint, heightConstraint));
        }

        private void OnStartScanningRequested(object sender, EventArgs e)
        {
            ApplyOverlaySettings();
            onScanListener.ContinuousAfterScan = pickerView.Settings.ContinuousAfterScan;
            barcodePicker.ApplyScanSettings(CreateScanSettings(), null);
            barcodePicker.StartScanning();
        }

        private void OnPauseScanningRequested(object sender, EventArgs e)
        {
            barcodePicker.PauseScanning();
        }

        private ScanSettings CreateScanSettings()
        {
            var settings = pickerView.Settings;
            var scanSettings = ScanSettings.Create();

            // Symbologies
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyEan13, settings.Ean13Upc12);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyUpca, settings.Ean13Upc12);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyTwoDigitAddOn, settings.TwoDigitAddOn);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyFiveDigitAddOn, settings.FiveDigitAddOn);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyCode11, settings.Code11);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyCode25, settings.Code25);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyCode39, settings.Code39);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyCode93, settings.Code93);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyCode128, settings.Code128);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyInterleaved2Of5, settings.Interleaved2Of5);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyMsiPlessey, settings.MsiPlessey);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyGs1Databar, settings.Gs1Databar);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyGs1DatabarExpanded, settings.Gs1DatabarExpanded);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyGs1DatabarLimited, settings.Gs1DatabarLimited);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyCodabar, settings.Codabar);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyQr, settings.Qr);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyDataMatrix, settings.DataMatrix);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyPdf417, settings.Pdf417);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyMicroPdf417, settings.MicroPdf417);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyAztec, settings.Aztec);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyMaxicode, settings.MaxiCode);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyRm4scc, settings.Rm4scc);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyKix, settings.Kix);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyDotcode, settings.Kix);
            scanSettings.SetSymbologyEnabled(Barcode.SymbologyMicroQr, settings.MicroQR);

            if (settings.QrInverted)
            {
                var qrSettings = scanSettings.GetSymbologySettings(Barcode.SymbologyQr);
                qrSettings.ColorInvertedEnabled = true;
            }

            if (settings.DataMatrixInverted)
            {
                var datamatrixSettings = scanSettings.GetSymbologySettings(Barcode.SymbologyDataMatrix);
                datamatrixSettings.ColorInvertedEnabled = true;
            }

            if (settings.RestrictScanningArea)
            {
                float y = (float)settings.HotSpotY;
                float width = (float)settings.HotSpotWidth;
                float height = (float)settings.HotSpotHeight;
                scanSettings.SetScanningHotSpot(0.5f, y);
                var scanninArea = new Android.Graphics.RectF(0.5f - (width / 2), y - (height / 2), 0.5f + (width / 2), y + (height / 2));
                scanSettings.SetActiveScanningArea(ScanSettings.OrientationPortrait, scanninArea);
                scanSettings.SetActiveScanningArea(ScanSettings.OrientationLandscape, scanninArea);
            }

            scanSettings.MaxNumberOfCodesPerFrame = settings.TwoDigitAddOn || settings.FiveDigitAddOn ? 2 : 1;

            scanSettings.HighDensityModeEnabled = (settings.Resolution == Resolution.HD);

            scanSettings.MatrixScanEnabled = (settings.GuiStyle == GuiStyle.MatrixScan);

            return scanSettings;
        }

        private void ApplyOverlaySettings()
        {
            var settings = pickerView.Settings;

            var overlayView = barcodePicker.OverlayView;
            overlayView.SetGuiStyle(GetGuiStyleValue(settings.GuiStyle));
            overlayView.SetViewfinderPortraitDimension((float)settings.ViewFinderPortraitWidth,
                                                       (float)settings.ViewFinderPortraitHeight);
            overlayView.SetViewfinderLandscapeDimension((float)settings.ViewFinderLandscapeWidth,
                                                        (float)settings.ViewFinderLandscapeHeight);
            overlayView.SetBeepEnabled(settings.Beep);
            overlayView.SetVibrateEnabled(settings.Vibrate);
            overlayView.SetTorchEnabled(settings.TorchButtonVisible);
            overlayView.SetTorchButtonMarginsAndSize((int)settings.TorchLeftMargin,
                                                     (int)settings.TorchTopMargin,
                                                     40,
                                                     40);
            overlayView.SetCameraSwitchVisibility(GetCameraSwitchVisibilityValue(settings.CameraButton));
        }

        private int GetGuiStyleValue(GuiStyle guiStyle)
        {
            switch (guiStyle)
            {
                case GuiStyle.Frame:
                    return ScanOverlay.GuiStyleDefault;
                case GuiStyle.Laser:
                    return ScanOverlay.GuiStyleLaser;
                case GuiStyle.None:
                    return ScanOverlay.GuiStyleNone;
                case GuiStyle.MatrixScan:
                    return ScanOverlay.GuiStyleMatrixScan;
                default:
                    return ScanOverlay.GuiStyleLocationsOnly;
            }
        }

        private int GetCameraSwitchVisibilityValue(CameraButton cameraButton)
        {
            switch (cameraButton)
            {
                case CameraButton.Always:
                    return ScanOverlay.CameraSwitchAlways;
                case CameraButton.OnTablet:
                    return ScanOverlay.CameraSwitchOnTablet;
                default:
                    return ScanOverlay.CameraSwitchOnTablet;
            }
        }

        public class PickerOnScanListener : Java.Lang.Object, IOnScanListener
        {
            public bool ContinuousAfterScan { get; set; }
            public PickerView PickerView { get; set; }


            public void DidScan(IScanSession session)
            {
                if (session.NewlyRecognizedCodes.Count > 0)
                {
                    if (!ContinuousAfterScan)
                    {
                        // Stop the scanner directly on the session.
                        session.PauseScanning();
                    }

                    // If you want to edit something in the view hierarchy make sure to run it on the UI thread.
                    using (var h = new Android.OS.Handler(Android.OS.Looper.MainLooper))
                    {
                        h.Post(() =>
                        {
                            var symbologies = "";
                            var data = "";

                            foreach (var code in session.NewlyRecognizedCodes)
                            {
                                var separator = symbologies.Length == 0 ? "" : ", ";
                                symbologies += separator + code.SymbologyName;
                                data += separator + code.Data;
                            }
                            PickerView.DidScan(symbologies, data);
                        });
                    }
                }
            }
        }
    }
}
