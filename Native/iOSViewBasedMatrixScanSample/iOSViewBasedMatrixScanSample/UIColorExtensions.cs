using System;
using UIKit;
using CoreGraphics;
namespace iOSViewBasedMatrixScanSample
{
    public static class UIColorExtensions
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
