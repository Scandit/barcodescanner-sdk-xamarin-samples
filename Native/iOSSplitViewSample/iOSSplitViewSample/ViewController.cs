using System;

using UIKit;
using Foundation;

using ScanditBarcodeScanner.iOS;

namespace iOSSplitViewSample
{
    public partial class ViewController : UIViewController
    {

        private enum ScanState
        {
            scanning, paused, stopped
        }

        public static string appKey = "-- ENTER YOUR SCANDIT LICENSE KEY HERE --";

        BarcodePicker barcodePickerViewController;
        PickerScanDelegate scanDelegate;
        SplitViewResultTableViewController splitViewResultTableViewController;
        private NSTimer scanningTimer;
        private ScanSettings scanSettings;

        private ScanState _scanState;
        private ScanState scanState
        {
            get => _scanState;
            set {
                _scanState = value;
                switch (_scanState) 
                {
                    case ScanState.scanning:
                        StartTimer();
                        break;
                    case ScanState.paused:
                        break;
                    case ScanState.stopped:
                        break;
                }
            }
        }

        // Scanner timeOut in seconds.
        private double timeOut = 10.0;
        // Scan View Height in percents relative to ViewController's height.
        private double scanViewHeight = 50.0;
        // Determines if the beep sound will be played when the code will be found.
        private bool beepEnabled = true;
        // Duplicate code filter setting in seconds.
        private int duplicateFilterSeconds = 1;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Set the app key before instantiating the picker.
            License.SetAppKey(appKey);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.Title = "Split View";
            pickerHeightConstraint.Constant = (View.Frame.Size.Height * (System.nfloat)scanViewHeight) / 100;
            SetupBarcodeScanner(BuildSettings());
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            RestartScanning();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            PauseScanning();
        }

        private void SetupBarcodeScanner(ScanSettings settings)
        {
            this.scanSettings = settings;
            barcodePickerViewController = new BarcodePicker(settings);

            if (barcodePickerViewController == null) {
                return;
            }
            barcodePickerViewController.OverlayView.ShowToolBar(false);
            barcodePickerViewController.OverlayView.SetCameraSwitchVisibility(CameraSwitchVisibility.OnTablet);
            barcodePickerViewController.OverlayView.GuiStyle = GuiStyle.Laser;

            // Add delegate for the scan event. We keep references to the
            // delegates until the picker is no longer used as the delegates are softly
            // referenced and can be removed because of low memory.
            scanDelegate = new PickerScanDelegate(StartTimer, splitViewResultTableViewController);
            barcodePickerViewController.ScanDelegate = scanDelegate;

            AddChildViewController(barcodePickerViewController);
            barcodePickerViewController.View.Frame = barcodePickerContainer.Bounds;
            barcodePickerContainer.AddSubview(barcodePickerViewController.View);
            barcodePickerViewController.DidMoveToParentViewController(this);

            // Start scanning.
            barcodePickerViewController.StartScanning();
        }

        private ScanSettings BuildSettings()
        {
            ScanSettings settings = ScanSettings.DefaultSettings();
            settings.SetActiveScanningArea(new CoreGraphics.CGRect(0.0, 0.48, 1.0, 0.04));
            settings.RestrictedAreaScanningEnabled = true;

            NSSet symbologiesToEnable = new NSSet(
                Symbology.EAN13,
                Symbology.EAN8,
                Symbology.UPC12,
                Symbology.UPCE,
                Symbology.Code39,
                Symbology.Code128,
                Symbology.ITF
            );
            settings.EnableSymbologies(symbologiesToEnable);
            settings.CodeDuplicateFilter = duplicateFilterSeconds >= 0 ? duplicateFilterSeconds * 1000 : duplicateFilterSeconds;

            if (barcodePickerViewController != null) {
                barcodePickerViewController.OverlayView.SetBeepEnabled(beepEnabled);
                barcodePickerViewController.OverlayView.GuiStyle = GuiStyle.Laser;
            }

            return settings;
        }

        void StartTimer() {
            if (scanningTimer != null)
            {
                scanningTimer.Invalidate();
                scanningTimer = null;
            }
            scanningTimer = NSTimer.CreateScheduledTimer(timeOut, delegate {
                PauseScanning();
            });
        }

        void PauseScanning() {
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                this.barcodePickerViewController.PauseScanning();
                this.scanState = ScanState.paused;
                this.pausedStateOverlayView.Hidden = false;
                this.tapToContinueButton.Hidden = false;
            });
        }

        void StartScanning() {
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                this.barcodePickerViewController.StartScanning();
                this.scanState = ScanState.scanning;
                this.pausedStateOverlayView.Hidden = true;
                this.tapToContinueButton.Hidden = true;
            });
        }

        void RestartScanning() {
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                this.barcodePickerViewController.ResumeScanning();
                this.scanState = ScanState.scanning;
                this.pausedStateOverlayView.Hidden = true;
                this.tapToContinueButton.Hidden = true;
            });
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {            
            if (segue.Identifier == "EmbedSplitViewResultTableViewController")
            {
                splitViewResultTableViewController = segue.DestinationViewController as SplitViewResultTableViewController;
            }
        }

        partial void TapToContinueButton_TouchUpInside(UIButton sender)
        {
            this.RestartScanning();
        }

        partial void ClearButton_TouchUpInside(UIButton sender)
        {
            this.splitViewResultTableViewController.ClearResults();
        }

        public class PickerScanDelegate : ScanDelegate
        {
            WeakReference startTimer;
            WeakReference splitViewResultTableViewController;

            public PickerScanDelegate(Action startTimer, SplitViewResultTableViewController splitViewResultTableViewController) {
                this.startTimer = new WeakReference(startTimer);
                this.splitViewResultTableViewController = new WeakReference(splitViewResultTableViewController);
            }

            private void StartTimer() {
                if (startTimer.Target is Action action)
                {
                    action();
                }
            }

            private void AddResult(SplitViewResult splitViewResult) {
                SplitViewResultTableViewController viewController = splitViewResultTableViewController.Target as SplitViewResultTableViewController;
                if (viewController != null) {
                    viewController.AddResult(splitViewResult);
                }
            }

            public override void DidScan(BarcodePicker picker, IScanSession session)
            {
                NSArray<Barcode> codes = session.NewlyRecognizedCodes;
                if (codes.Count == 1 && (codes[0].Symbology == Symbology.FiveDigitAddOn || codes[0].Symbology == Symbology.TwoDigitAddOn)) 
                {
                    return;    
                }

                UIApplication.SharedApplication.InvokeOnMainThread(() => {
                    StartTimer();
                    foreach(var code in codes) 
                    {
                        if (code.Data != null) 
                        {
                            AddResult(new SplitViewResult(code.Data, code.SymbologyString));
                        } 
                        else 
                        {
                            continue;
                        }
                    }
                });
            }
        }

    }
}
