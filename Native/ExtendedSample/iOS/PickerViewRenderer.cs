using System;
using ExtendedSample;
using ExtendedSample.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ScanditBarcodeScanner.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(PickerView), typeof(PickerViewRenderer))]
namespace ExtendedSample.iOS
{
    public class PickerViewRenderer : ViewRenderer<PickerView, UIView>
    {
        private RotationSettingAwareBarcodePicker barcodePicker;
        private PickerScanDelegate scanDelegate;
        private PickerView pickerView;
        private bool detachedFromWindow;

        public PickerViewRenderer()
        {
            var appKey = PickerView.GetAppKey();
            License.SetAppKey(appKey);
        }

        public override void WillMoveToWindow(UIWindow window)
        {
            base.WillMoveToWindow(window);

            if (window == null) // Disappear
            {
                this.detachedFromWindow = true;
                this.pickerView.PauseScanning();
            }
        }
        
        public override void MovedToWindow()
        {
            base.MovedToWindow();

            if (this.detachedFromWindow)
            {
                this.pickerView.StartScanning();
                this.detachedFromWindow = false;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<PickerView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                pickerView = e.NewElement;

                e.NewElement.StartScanningRequested += OnStartScanningRequested;
                e.NewElement.PauseScanningRequested += OnPauseScanningRequested;

                barcodePicker = new RotationSettingAwareBarcodePicker(CreateScanSettings());
                SetNativeControl(barcodePicker.View);
                scanDelegate = new PickerScanDelegate
                {
                    PickerView = pickerView,
                    ContinuousAfterScan = pickerView.Settings.ContinuousAfterScan
                };
                barcodePicker.ScanDelegate = scanDelegate;

                ApplyOverlaySettings();
                barcodePicker.StartScanning();
            }
            if (e.OldElement != null)
            {
                e.OldElement.StartScanningRequested -= OnStartScanningRequested;
                e.OldElement.PauseScanningRequested -= OnPauseScanningRequested;
            }
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(widthConstraint, heightConstraint),
                                   new Size(widthConstraint, heightConstraint));
        }

        private void OnStartScanningRequested(object sender, EventArgs e)
        {
            ApplyOverlaySettings();
            scanDelegate.ContinuousAfterScan = pickerView.Settings.ContinuousAfterScan;
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
            var scanSettings = ScanSettings.DefaultSettings();

            // Symbologies
            scanSettings.SetSymbologyEnabled(Symbology.EAN13, settings.Ean13Upc12);
            scanSettings.SetSymbologyEnabled(Symbology.UPC12, settings.Ean13Upc12);
            scanSettings.SetSymbologyEnabled(Symbology.EAN8, settings.Ean8);
            scanSettings.SetSymbologyEnabled(Symbology.UPCE, settings.Upce);
            scanSettings.SetSymbologyEnabled(Symbology.TwoDigitAddOn, settings.TwoDigitAddOn);
            scanSettings.SetSymbologyEnabled(Symbology.FiveDigitAddOn, settings.FiveDigitAddOn);
            scanSettings.SetSymbologyEnabled(Symbology.Code11, settings.Code11);
            scanSettings.SetSymbologyEnabled(Symbology.Code25, settings.Code25);
            scanSettings.SetSymbologyEnabled(Symbology.Code32, settings.Code32);
            scanSettings.SetSymbologyEnabled(Symbology.Code39, settings.Code39);
            scanSettings.SetSymbologyEnabled(Symbology.Code93, settings.Code93);
            scanSettings.SetSymbologyEnabled(Symbology.Code128, settings.Code128);
            scanSettings.SetSymbologyEnabled(Symbology.ITF, settings.Interleaved2Of5);
            scanSettings.SetSymbologyEnabled(Symbology.MSIPlessey, settings.MsiPlessey);
            scanSettings.SetSymbologyEnabled(Symbology.GS1Databar, settings.Gs1Databar);
            scanSettings.SetSymbologyEnabled(Symbology.GS1DatabarExpanded, settings.Gs1DatabarExpanded);
            scanSettings.SetSymbologyEnabled(Symbology.GS1DatabarLimited, settings.Gs1DatabarLimited);
            scanSettings.SetSymbologyEnabled(Symbology.Codabar, settings.Codabar);
            scanSettings.SetSymbologyEnabled(Symbology.QR, settings.Qr);
            scanSettings.SetSymbologyEnabled(Symbology.Datamatrix, settings.DataMatrix);
            scanSettings.SetSymbologyEnabled(Symbology.PDF417, settings.Pdf417);
            scanSettings.SetSymbologyEnabled(Symbology.MicroPDF417, settings.MicroPdf417);
            scanSettings.SetSymbologyEnabled(Symbology.Aztec, settings.Aztec);
            scanSettings.SetSymbologyEnabled(Symbology.MaxiCode, settings.MaxiCode);
            scanSettings.SetSymbologyEnabled(Symbology.RM4SCC, settings.Rm4scc);
            scanSettings.SetSymbologyEnabled(Symbology.KIX, settings.Kix);
            scanSettings.SetSymbologyEnabled(Symbology.DotCode, settings.Kix);
            scanSettings.SetSymbologyEnabled(Symbology.MicroQr, settings.MicroQR);
            scanSettings.SetSymbologyEnabled(Symbology.LAPA4SC, settings.Lapa4sc);

            if (settings.QrInverted)
            {
                var qrSettings = scanSettings.SettingsForSymbology(Symbology.QR);
                qrSettings.ColorInvertedEnabled = true;
            }

            var isScanningAreaOverridden = false;
            if (settings.DataMatrix)
            {
                var datamatrixSettings = scanSettings.SettingsForSymbology(Symbology.Datamatrix);

                datamatrixSettings.ColorInvertedEnabled = settings.DataMatrixInverted;

                if (settings.DpmMode)
                {
                    scanSettings.RestrictedAreaScanningEnabled = true;
                    var scanninArea = new CoreGraphics.CGRect(0.33f, 0.33f, 0.33f, 0.33f);
                    scanSettings.SetActiveScanningArea(scanninArea);

                    isScanningAreaOverridden = true;

                    // Enabling the direct_part_marking_mode extension comes at the cost of increased frame processing times.
                    // It is recommended to restrict the scanning area to a smaller part of the image for best performance.
                    datamatrixSettings.SetExtensionEnabled("direct_part_marking_mode", true);
                }
            }

            if (settings.RestrictScanningArea && !isScanningAreaOverridden)
            {
                var y = settings.HotSpotY;
                var width = settings.HotSpotWidth;
                var height = settings.HotSpotHeight;
                scanSettings.ScanningHotSpot = new CoreGraphics.CGPoint(0.5, y);
                var scanninArea = new CoreGraphics.CGRect(0.5 - width / 2, y - (height / 2), width, height);
                scanSettings.SetActiveScanningArea(scanninArea);
            }

            if (settings.TwoDigitAddOn || settings.FiveDigitAddOn)
            {
                scanSettings.MaxNumberOfCodesPerFrame = 2;
            }
            else
            {
                scanSettings.MaxNumberOfCodesPerFrame = 1;
            }

            scanSettings.HighDensityModeEnabled = (settings.Resolution == Resolution.HD);

            scanSettings.MatrixScanEnabled = (settings.GuiStyle == GuiStyle.MatrixScan);

            return scanSettings;
        }

        private void ApplyOverlaySettings()
        {
            var settings = pickerView.Settings;
            barcodePicker.ShouldFollowDeviceOrientation = settings.RotationWithDevice;
            var OverlayView = barcodePicker.OverlayView;
            OverlayView.GuiStyle = (ScanditBarcodeScanner.iOS.GuiStyle)settings.GuiStyle;
            OverlayView.SetViewfinderPortraitDimension((float)settings.ViewFinderPortraitWidth,
                                                       (float)settings.ViewFinderPortraitHeight);
            OverlayView.SetViewfinderLandscapeDimension((float)settings.ViewFinderLandscapeWidth,
                                                        (float)settings.ViewFinderLandscapeHeight);
            OverlayView.SetBeepEnabled(settings.Beep);
            OverlayView.SetVibrateEnabled(settings.Vibrate);
            OverlayView.SetTorchEnabled(settings.TorchButtonVisible);
            OverlayView.SetTorchButtonMarginsAndSize((float)settings.TorchLeftMargin,
                                                     (float)settings.TorchTopMargin,
                                                     40,
                                                     40);
            OverlayView.SetCameraSwitchVisibility((CameraSwitchVisibility)settings.CameraButton);
        }

        public class PickerScanDelegate : ScanDelegate
        {
            public bool ContinuousAfterScan { get; set; }
            public PickerView PickerView { get; set; }

            public override void DidScan(BarcodePicker picker, IScanSession session)
            {
                if (session.NewlyRecognizedCodes.Count > 0)
                {
                    if (!ContinuousAfterScan)
                    {
                        // Stop the scanner directly on the session.
                        session.PauseScanning();
                    }

                    // If you want to edit something in the view hierarchy make sure to run it on the UI thread.
                    UIApplication.SharedApplication.InvokeOnMainThread(() =>
                    {
                        var symbologies = "";
                        var data = "";

                        foreach (var code in session.NewlyRecognizedCodes)
                        {
                            var separator = symbologies.Length == 0 ? "" : ", ";
                            symbologies += separator + code.SymbologyString;
                            data += separator + code.Data;
                        }
                        PickerView.DidScan(symbologies, data);
                    });
                }
            }
        }
    }
}
