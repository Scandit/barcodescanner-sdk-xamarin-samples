using System;
using ScanditBarcodeScanner.iOS;
using UIKit;

namespace ExtendedSample.iOS
{
    public class RotationSettingAwareBarcodePicker : BarcodePicker
    {
        private bool _shouldAutoRotate;

        public bool ShouldFollowDeviceOrientation
        {
            get
            {
                return _shouldAutoRotate;
            }
            set
            {
                _shouldAutoRotate = value;
            }
        }

        public RotationSettingAwareBarcodePicker(ScanSettings scanSettings) : base(scanSettings) {}

        public override bool ShouldAutorotate()
        {
            return ShouldFollowDeviceOrientation;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
            appDelegate.ShouldAutoRotate = ShouldFollowDeviceOrientation;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
            appDelegate.ShouldAutoRotate = !ShouldFollowDeviceOrientation;
        }
    }
}
