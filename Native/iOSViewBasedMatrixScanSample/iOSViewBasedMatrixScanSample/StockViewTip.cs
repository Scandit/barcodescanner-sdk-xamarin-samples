using CoreAnimation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace iOSViewBasedMatrixScanSample
{
    internal class StockViewTip : UIView
    {
        public UIColor FillColor { get; set; } = UIColor.Black;

        [Export("layerClass")]
        public static Class LayerClass()
        {
            return new Class(typeof(CAShapeLayer));
        }

        public override void LayoutSubviews()
        {
            Setup();
        }

        private CAShapeLayer ShapeLayer => (CAShapeLayer) Layer;

        internal StockViewTip(CGRect frame) : base(frame)
        {
            Setup();
        }

        internal StockViewTip(NSCoder coder) : base(coder)
        {
            Setup();
        }

        private void Setup()
        {
            var path = new UIBezierPath();
            var origin = Bounds.Location;
            origin.Y -= 1;
            path.MoveTo(origin);
            path.AddLineTo(new CGPoint(Bounds.Location.X + Bounds.Size.Width, Bounds.Location.Y - 0.1f));
            path.AddLineTo(new CGPoint(Bounds.GetMidX(), Bounds.GetMaxY()));
            path.ClosePath();
            path.MiterLimit = 4;
            ShapeLayer.Path = path.CGPath;
            ShapeLayer.FillColor = FillColor.CGColor;
        }
    }
}