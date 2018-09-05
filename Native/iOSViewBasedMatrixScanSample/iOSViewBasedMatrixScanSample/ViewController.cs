using System;
using System.Collections.Generic;
using CoreFoundation;
using UIKit;
using CoreGraphics;
using Foundation;
using ScanditBarcodeScanner.iOS;

namespace iOSViewBasedMatrixScanSample
{
    internal enum State
    {
        Stopped,
        Tracking,
        Frozen
    }

    public class ViewController : UIViewController
    {

        private UIButton freezeButton;
        private UIView containerView;

        private BarcodePicker picker;
        private MatrixScanHandler matrixScanHandler;
        private ViewBasedMatrixScanOverlay viewbasedOverlay;


        private State _state = State.Stopped;

        private State State
        {
            get => _state;
            set
            {
                _state = value;
                switch (value)
                {
                    case State.Tracking:
                        Reset();
                        matrixScanHandler.Enabled = true;
                        picker.StartScanning(true);
                        break;
                    case State.Frozen:
                        matrixScanHandler.Enabled = false;
                        picker.StopScanning();
                        break;
                    case State.Stopped:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private void Reset()
        {
            matrixScanHandler.RemoveAllAugmentations();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            freezeButton = new UIButton(CGRect.Empty);
            containerView = new UIView(CGRect.Empty);
            View.AddSubview(containerView);
            View.AddSubview(freezeButton);
            SetupConstraints();
            freezeButton.SetBackgroundImage(UIColorExtensions.Brand.GetImage(), UIControlState.Normal);
            freezeButton.SetTitle("Freeze", UIControlState.Normal);
            freezeButton.TouchUpInside += (sender, e) => 
            {
                switch (State)
                {
                    case State.Frozen:
                    case State.Stopped:
                        State = State.Tracking;
                        freezeButton.SetTitle("Freeze", UIControlState.Normal);
                        break;
                    case State.Tracking:
                        State = State.Frozen;
                        freezeButton.SetTitle("Done", UIControlState.Normal);
                        break;
                }
            };

            var settings = ScanSettings.DefaultSettings();
            settings.SetSymbologyEnabled(Symbology.EAN13, true);
            settings.MatrixScanEnabled = true;
            settings.MaxNumberOfCodesPerFrame = 15;
            settings.HighDensityModeEnabled = true;
            picker = new BarcodePicker(settings) {OverlayView = {GuiStyle = GuiStyle.None}};
            matrixScanHandler = new MatrixScanHandler(picker);
            matrixScanHandler.ShouldReject += (handler, barcode) => false;
            matrixScanHandler.DidProcess += (sender, args) =>
            {
                DispatchQueue.MainQueue.DispatchAsync(() =>
                {
                    foreach (var keyValuePair in args.Frame.TrackedCodes)
                    {
                        var code = keyValuePair.Value as TrackedBarcode;
                        var offset = GetYOffSet(code);
                        viewbasedOverlay.SetOffset(offset, keyValuePair.Key as NSNumber);
                    }
                });
            };
            
            viewbasedOverlay = new ViewBasedMatrixScanOverlay();
            viewbasedOverlay.OffsetForOverlay += (overlay, barcode, identifier) => GetYOffSet(barcode);
            viewbasedOverlay.ViewForOverlay += (overlay, barcode, identifier) =>
            {
                if (barcode.Data == null) return new UIView(CGRect.Empty);
                var view = new StockView(new CGRect(0, 0, StockView.StandardWidth, StockView.StandardHeight));
                var model = Model.MockedModel(barcode.Data);
                view.OnViewClicked += (sender, args) =>
                {
                    var overlayViewController = new OverlayViewController {Model = args.Model};
                    PresentViewController(overlayViewController, false, null);
                };
                view.Model = model;

                return view;
            };
            matrixScanHandler.AddOverlay(viewbasedOverlay);
            
            var simpleOverlay = new SimpleMatrixScanOverlay();
            simpleOverlay.ColorForOverlay += (overlay, barcode, identifier) => Model.MockedColor(barcode.Data);

            //TODO
            AddChildViewController(picker);
            picker.View.Frame = containerView.Bounds;
            picker.View.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            containerView.AddSubview(picker.View);
            picker.DidMoveToParentViewController(this);

            State = State.Tracking;
        }

        private UIOffset GetYOffSet(TrackedBarcode code)
        {
            var barcodeHeight = GetBarcodeHeight(code.PredictedLocation);
            var stockViewHeight = StockView.StandardHeight;
            const float stockViewDistance = 8.0f;
            var yOffset = -(barcodeHeight / 2 + stockViewHeight / 2 + stockViewDistance);
            return new UIOffset(0, yOffset);
        }

        private float GetBarcodeHeight(Quadrilateral predictedLocation)
        {
            var topLeft = picker.ConvertPoint(predictedLocation.topLeft);
            var bottomLeft = picker.ConvertPoint(predictedLocation.bottomLeft);
            var topRight = picker.ConvertPoint(predictedLocation.topRight);
            var bottomRight = picker.ConvertPoint(predictedLocation.bottomRight);
            return (float) Math.Max(bottomLeft.Y - topLeft.Y, bottomRight.Y - topRight.Y);
        }

        private void SetupConstraints()
        {
            freezeButton.TranslatesAutoresizingMaskIntoConstraints = false;
            containerView.TranslatesAutoresizingMaskIntoConstraints = false;
            View.AddConstraints(new[]
            {
                freezeButton.HeightAnchor.ConstraintEqualTo(60),
                freezeButton.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
                freezeButton.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, -20),
                freezeButton.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20),
                containerView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
                containerView.TopAnchor.ConstraintEqualTo(View.TopAnchor),
                containerView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
                containerView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor)
            });
        }
    }
}
