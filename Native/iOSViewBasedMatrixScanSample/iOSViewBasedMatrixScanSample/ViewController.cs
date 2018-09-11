using System;
using CoreGraphics;
using UIKit;
using ScanditBarcodeScanner.iOS;
using Foundation;
using CoreFoundation;
using System.Diagnostics;

namespace iOSViewBasedMatrixScanSample
{
    public partial class ViewController : UIViewController
    {
        UIView contatinerView;
        UIButton freezeButton;

        BarcodePicker picker;
        MatrixScanHandler matrixScanHandler;
        SimpleMatrixScanOverlay simpleMatrixScanOverlay;
        ViewBasedMatrixScanOverlay viewBasedMatrixScanOverlay;

        protected ViewController(IntPtr handle) : base(handle) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            contatinerView = new UIView(CGRect.Empty);
            View.AddSubview(contatinerView);
            freezeButton = new UIButton(CGRect.Empty);
            freezeButton.SetTitle("Freeze", UIControlState.Normal);
            freezeButton.SetBackgroundImage(UIImageExtensions.Brand.GetImage(), UIControlState.Normal);
            freezeButton.TouchUpInside += (sender, e) => 
            {
                var scanning = picker.IsScanning();
                if (scanning)
                {
                    matrixScanHandler.Enabled = false;
                    picker.PauseScanning();
                    freezeButton.SetTitle("Done", UIControlState.Normal);
                }
                else
                {
                    matrixScanHandler.RemoveAllAugmentations();
                    matrixScanHandler.Enabled = true;
                    picker.StartScanning();
                    freezeButton.SetTitle("Freeze", UIControlState.Normal);
                }
            };
            View.AddSubview(freezeButton);

            freezeButton.TranslatesAutoresizingMaskIntoConstraints = false;
            contatinerView.TranslatesAutoresizingMaskIntoConstraints = false;
            View.AddConstraints(new[]
            {
                contatinerView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
                contatinerView.TopAnchor.ConstraintEqualTo(View.TopAnchor),
                contatinerView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
                contatinerView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor),
                freezeButton.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
                freezeButton.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, -20),
                freezeButton.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20),
                freezeButton.HeightAnchor.ConstraintEqualTo(60)
            });

            var settings = ScanSettings.DefaultSettings();
            settings.SetSymbologyEnabled(Symbology.EAN13, true);
            settings.MatrixScanEnabled = true;
            picker = new BarcodePicker(settings);
            picker.OverlayView.GuiStyle = GuiStyle.None;

            matrixScanHandler = new MatrixScanHandler(picker);
            matrixScanHandler.ShouldReject += (matrixScanHandler, trackedBarcode) => false;

            // This delegate method is called every time a new frame has been processed.
            // In this case we use it to update the offset of the augmentation.
            matrixScanHandler.DidProcess += (sender, e) => 
            {
                DispatchQueue.MainQueue.DispatchAsync(() =>
                {
                    foreach (var item in e.Frame.TrackedCodes)
                    {
                        var offset = GetYOffSet(item.Value as TrackedBarcode);
                        viewBasedMatrixScanOverlay.SetOffset(offset, item.Key as NSNumber);
                    }
                });
            };

            simpleMatrixScanOverlay = new SimpleMatrixScanOverlay()
            {
                UserTapEnabled = true
            };

            // This method is called every time a new barcode has been tracked.
            // You can implement this method to customize the color of the highlight.
            simpleMatrixScanOverlay.ColorForOverlay += (overlay, barcode, identifier) => Model.MockedColor(barcode.Data);

            // This method is called when the user taps the highlight.
            simpleMatrixScanOverlay.OverlayDidTap += (sender, e) => 
            {
                var model = Model.MockedModel(e.Barcode.Data);
                var overlayViewController = new OverlayViewController
                {
                    Model = model,
                    ModalTransitionStyle = UIModalTransitionStyle.CoverVertical,
                    ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext
                };
                PresentViewController(overlayViewController, false, null);
            };

            // Add a SimpleMatrixScanOverlay in order to highlight the barcodes.
            matrixScanHandler.AddOverlay(simpleMatrixScanOverlay);

            viewBasedMatrixScanOverlay = new ViewBasedMatrixScanOverlay();

            // This method is called every time a new barcode has been tracked.
            // You can implement this method to return the offset that will be used to position the augmentation
            // with respect to the center of the tracked barcode.
            viewBasedMatrixScanOverlay.OffsetForOverlay += (overlay, barcode, identifier) => GetYOffSet(barcode);

            // This delegate method is called every time a new barcode has been tracked.
            // You can implement this method to return the view that will be used as the augmentation.
            viewBasedMatrixScanOverlay.ViewForOverlay += (overlay, barcode, identifier) =>
            {
                if (barcode.Data == null) return new UIView(CGRect.Empty);
                var view = new StockView(new CGRect(0, 0, StockView.StandardWidth, StockView.StandardHeight));
                var model = Model.MockedModel(barcode.Data);
                view.AddGestureRecognizer(new UITapGestureRecognizer(() =>
                {
                    var overlayViewController = new OverlayViewController 
                    {
                        Model = model,
                        ModalTransitionStyle = UIModalTransitionStyle.CoverVertical,
                        ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext
                    };
                    PresentViewController(overlayViewController, false, null);
                }));
                view.Model = model;

                return view;
            };

            // Add a ViewBasedMatrixScanOverlay in order to have custom UIView instances as augmentations.
            matrixScanHandler.AddOverlay(viewBasedMatrixScanOverlay);

            AddChildViewController(picker);
            picker.View.Frame = contatinerView.Frame;
            picker.View.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            contatinerView.AddSubview(picker.View);
            picker.DidMoveToParentViewController(this);
            picker.StartScanning();
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
            return (float)Math.Max(bottomLeft.Y - topLeft.Y, bottomRight.Y - topRight.Y);
        }
    }

    static class UIImageExtensions
    {
        public static readonly UIColor Brand = new UIColor(57.0f / 255.0f, 193.0f / 255.0f, 204.0f / 255.0f, 1.0f);

        public static UIImage GetImage(this UIColor color)
        {
            var rect = new CGRect(0, 0, 1, 1);
            UIGraphics.BeginImageContext(rect.Size);
            var context = UIGraphics.GetCurrentContext();
            if (context == null) return null;
            context.SetFillColor(color.CGColor);
            context.FillRect(rect);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return image;
        }
    }
}
