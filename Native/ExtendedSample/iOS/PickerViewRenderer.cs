using System;
using ExtendedSample;
using ExtendedSample.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ScanditBarcodeScanner.iOS;
using UIKit;
using CoreFoundation;

[assembly: ExportRenderer(typeof(ScannerPage), typeof(BarcodePickerRenderer))]
namespace ExtendedSample.iOS
{
    public class BarcodePickerRenderer : PageRenderer
    {
        BarcodePicker barcodePicker;
        PickerScanDelegate scanDelegate;
        ScannerPage scannerPage;
        UILabel label;

        public BarcodePickerRenderer()
        {
            License.SetAppKey(@"--- ENTER YOUR SCANDIT APP KEY HERE ---");
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            scannerPage = e.NewElement as ScannerPage;
            barcodePicker = new BarcodePicker(CreateScanSettings());
            AddChildViewController(barcodePicker);
            barcodePicker.View.Frame = new CoreGraphics.CGRect(0, 0, this.View.Bounds.Width, this.View.Bounds.Height - 100);
            View.AddSubview(barcodePicker.View);
            barcodePicker.StartScanning();
            barcodePicker.DidMoveToParentViewController(this);

            label = new UILabel();
            label.Frame = new CoreGraphics.CGRect(0, this.View.Bounds.Height - 100, this.View.Bounds.Width, 44);
            label.TextAlignment = UITextAlignment.Center;
            label.Lines = 0;
            View.AddSubview(label);

            scanDelegate = new PickerScanDelegate();
            scanDelegate.WeakLabel = new WeakReference<UILabel>(label);
            barcodePicker.ScanDelegate = scanDelegate;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            barcodePicker.ApplyScanSettings(CreateScanSettings(), null);
            ApplyOverlaySettings();
            barcodePicker.ResumeScanning();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            barcodePicker.PauseScanning();
        }

        private ScanSettings CreateScanSettings() 
        {
            var settings = scannerPage.Settings;
            var scanSettings = ScanSettings.DefaultSettings();

            // Symbologies
            scanSettings.SetSymbologyEnabled(Symbology.EAN13, settings.Ean13Upc12);
            scanSettings.SetSymbologyEnabled(Symbology.UPC12, settings.Ean13Upc12);
            scanSettings.SetSymbologyEnabled(Symbology.TwoDigitAddOn, settings.TwoDigitAddOn);
            scanSettings.SetSymbologyEnabled(Symbology.FiveDigitAddOn, settings.FiveDigitAddOn);
            scanSettings.SetSymbologyEnabled(Symbology.Code11, settings.Code11);
            scanSettings.SetSymbologyEnabled(Symbology.Code25, settings.Code25);
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

            if (settings.QrInverted)
            {
                var qrSettings = scanSettings.SettingsForSymbology(Symbology.QR);
                qrSettings.ColorInvertedEnabled = true;
            }

            if (settings.DataMatrixInverted)
            {
                var datamatrixSettings = scanSettings.SettingsForSymbology(Symbology.Datamatrix);
                datamatrixSettings.ColorInvertedEnabled = true;
            }

            if (settings.RestrictScanningArea)
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

            scanSettings.MatrixScanEnabled = (settings.GuiStyle == GuiStyle.MatrixScan); 

            return scanSettings;
        }

        private void ApplyOverlaySettings()
        {
            var settings = scannerPage.Settings;
            barcodePicker.allowedInterfaceOrientations = settings.RotationWithDevice ? UIInterfaceOrientationMask.All : UIInterfaceOrientationMask.Portrait;
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
            OverlayView.SetCameraSwitchVisibility((ScanditBarcodeScanner.iOS.CameraSwitchVisibility)settings.CameraButton);
        }

        public class PickerScanDelegate : ScanDelegate
        {
            public WeakReference<UILabel> WeakLabel { get; set; }

            public override void DidScan(BarcodePicker picker, IScanSession session)
            {
                if (session.NewlyRecognizedCodes.Count > 0)
                {
                    Barcode code = session.NewlyRecognizedCodes.GetItem<Barcode>(0);

                    // If you want to edit something in the view hierarchy make sure to run it on the UI thread.
                    UIApplication.SharedApplication.InvokeOnMainThread(() =>
                    {
                        if (WeakLabel.TryGetTarget(out var label)) {
                            label.Text = code.SymbologyString + ": " + code.Data;
                        }
                    });
                }
            }
        }
    }
}
