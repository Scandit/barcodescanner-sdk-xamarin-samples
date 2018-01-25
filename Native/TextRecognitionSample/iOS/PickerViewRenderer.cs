using System;
using TextRecognitionSample;
using TextRecognitionSample.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ScanditBarcodeScanner.iOS;
using UIKit;
using CoreFoundation;
using Foundation;

[assembly: ExportRenderer(typeof(PickerView), typeof(PickerViewRenderer))]
namespace TextRecognitionSample.iOS
{
    public class PickerViewRenderer : ViewRenderer<PickerView, UIView>
    {
        private BarcodePicker barcodePicker;
        private PickerScanDelegate scanDelegate;
        private PickerTextRecognitionDelegate textRecognitionDelegate;
        private PickerPropertyObserver propertyObserver;
        private PickerView pickerView;

        public PickerViewRenderer()
        {
            License.SetAppKey("--- ENTER YOUR SCANDIT APP KEY HERE ---");
        }

        protected override void OnElementChanged(ElementChangedEventArgs<PickerView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                pickerView = e.NewElement;

                e.NewElement.StartScanningRequested += OnStartScanningRequested;
                e.NewElement.PauseScanningRequested += OnPauseScanningRequested;

                barcodePicker = new BarcodePicker(CreateScanSettings());
                SetNativeControl(barcodePicker.View);
                barcodePicker.StartScanning();

                // Set the delegate to receive recognize text event callbacks
                textRecognitionDelegate = new PickerTextRecognitionDelegate();
                textRecognitionDelegate.PickerView = pickerView;
                barcodePicker.TextRecognitionDelegate = textRecognitionDelegate;

                // Set the delegate to receive scan event callbacks
                scanDelegate = new PickerScanDelegate();
                scanDelegate.PickerView = pickerView;
                barcodePicker.ScanDelegate = scanDelegate;

                // Listen to changes between the text recognition mode and the code recognition mode.
                propertyObserver = new PickerPropertyObserver();
                propertyObserver.PickerViewRenderer = this;
                barcodePicker.AddPropertyObserver(propertyObserver);

                ApplyOverlaySettings(RecognitionMode.Text);
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
            ApplyOverlaySettings(RecognitionMode.Text);
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

            // Create the scan recognition settings
            var textRecognitionSettings = new TextRecognitionSettings();

            // Set the area in which text is to be recognized.
            var ScanArea = new CoreGraphics.CGRect(0.0, settings.ScanAreaY(), 1.0, settings.ScanAreaHeight());
            textRecognitionSettings.AreaLandscape = ScanArea;
            textRecognitionSettings.AreaPortrait = ScanArea;

            // Set the regular expression
            NSError error;
            textRecognitionSettings.Regex = new NSRegularExpression(new NSString(settings.RegularExpression()), 
                                                                    NSRegularExpressionOptions.CaseInsensitive, 
                                                                    out error);

            // Create the scan settings
            var scanSettings = ScanSettings.DefaultSettings();

            // Set the text recognition settings
            scanSettings.TextRecognitionSettings = textRecognitionSettings;

            // Set the recognize mode to recognize text
            scanSettings.RecognitionMode = RecognitionMode.Text;

            // Enabling some symbologies
            var symbologies = new Symbology[] { Symbology.EAN8, Symbology.EAN13, Symbology.UPCE, Symbology.UPC12, Symbology.Datamatrix, Symbology.QR, Symbology.Code39, Symbology.Code128, Symbology.ITF };
            foreach (Symbology symbology in symbologies)
            {
                scanSettings.SetSymbologyEnabled(symbology, true);
            }

            var centerY = settings.ScanAreaY() + settings.ScanAreaHeight() / 2;
            scanSettings.ScanningHotSpot = new CoreGraphics.CGPoint(0.5, centerY);

            return scanSettings;
        }

        private void ApplyOverlaySettings(RecognitionMode recognitionMode)
        {
            var OverlayView = barcodePicker.OverlayView;

            // Hide the torch button
            OverlayView.SetTorchEnabled(false);

            var height = recognitionMode == RecognitionMode.Text ? 0.1f : 0.4f;
            OverlayView.SetViewfinderDimension(0.9f, height, 0.6f, height);
        }

        public class PickerScanDelegate : ScanDelegate
        {
            public bool ContinuousAfterScan { get; set; }
            public PickerView PickerView { get; set; }

            public override void DidScan(BarcodePicker picker, IScanSession session)
            {
                if (session.NewlyRecognizedCodes.Count > 0)
                {
                    Barcode code = session.NewlyRecognizedCodes.GetItem<Barcode>(0);
                    session.PauseScanning();

                    UIApplication.SharedApplication.InvokeOnMainThread(() =>
                    {
                        PickerView.DidScan(code.Data);
                    });
                }
            }
        }

        public class PickerTextRecognitionDelegate : TextRecognitionDelegate
        {
            public PickerView PickerView { get; set; }

            public override BarcodePickerState DidScan(BarcodePicker picker, RecognizedText text)
            {
                var textRecognized = text.text;
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    PickerView.DidScan(textRecognized);
                });
                return BarcodePickerState.Paused;
            }
        }

        public class PickerPropertyObserver : PropertyObserver
        {
            public PickerViewRenderer PickerViewRenderer { get; set; }

            public override void PropertyChanged(BarcodePicker picker, NSString property, NSObject value)
            {
                if (property == "recognitionMode")
                {
                    var number = value as NSNumber;
                    if (number == null) 
                    {
                        return;
                    }
                    var mode = (RecognitionMode)number.Int64Value;
                    UIApplication.SharedApplication.InvokeOnMainThread(() =>
                    {
                        PickerViewRenderer.ApplyOverlaySettings(mode);
                    });
                }
            }
        }
    }
}
