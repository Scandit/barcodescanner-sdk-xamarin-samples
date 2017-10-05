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

        public BarcodePickerRenderer()
        {
            License.SetAppKey(@"--- ENTER YOUR SCANDIT APP KEY HERE ---");
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

			scanDelegate = new PickerScanDelegate();

			scannerPage = e.NewElement as ScannerPage;
            barcodePicker = new BarcodePicker(CreateScanSettings());
            AddChildViewController(barcodePicker);
            View.AddSubview(barcodePicker.View);
            barcodePicker.StartScanning();
            barcodePicker.DidMoveToParentViewController(this);
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
            barcodePicker.StartScanning();
        }

        private ScanSettings CreateScanSettings() 
        {
            var settings = scannerPage.Settings;
            var scanSettings = ScanSettings.DefaultSettings();
			
            scanDelegate.ContinuousAfterScan = settings.ContinuousAfterScan;

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
			scanSettings.SetSymbologyEnabled(Symbology.DotCode, settings.Kix);

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

            if (settings.ContinuousAfterScan)
            {
                scanSettings.CodeDuplicateFilter = -1;
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
			public bool ContinuousAfterScan { get; set; }

			public override void DidScan(BarcodePicker picker, IScanSession session)
			{
				if (session.NewlyRecognizedCodes.Count > 0)
				{
					Barcode code = session.NewlyRecognizedCodes.GetItem<Barcode>(0);

                    if (!ContinuousAfterScan)
                    {
                        // Stop the scanner directly on the session.
                        session.PauseScanning();
                    }

					// If you want to edit something in the view hierarchy make sure to run it on the UI thread.
					UIApplication.SharedApplication.InvokeOnMainThread(() =>
                    {
                        if (ContinuousAfterScan)
                        {
							UIAlertView alert = new UIAlertView()
							{
								Title = code.SymbologyString + " Barcode Detected",
								Message = code.Data
							};
							alert.Show();

							const int NSEC_PER_SEC = 1000000000;
							var time = new DispatchTime(DispatchTime.Now, 1 * NSEC_PER_SEC);
							// Dismiss after 1 second
							DispatchQueue.MainQueue.DispatchAfter(time, () =>
							{
								alert.DismissWithClickedButtonIndex(0, true);
							});
                        }
                        else 
                        {
							UIAlertView alert = new UIAlertView()
							{
								Title = code.SymbologyString + " Barcode Detected",
								Message = code.Data
							};
							alert.AddButton("OK");

							alert.Clicked += (object o, UIButtonEventArgs e) =>
							{
								picker.ResumeScanning();
							};

							alert.Show();
                        }
					});
				}
			}
		}
    }
}
