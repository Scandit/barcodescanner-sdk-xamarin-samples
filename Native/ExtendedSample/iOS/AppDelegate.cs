using Foundation;
using UIKit;

namespace ExtendedSample.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public bool ShouldAutoRotate { get; set; } = true;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        [Export("application:supportedInterfaceOrientationsForWindow:")]
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            return ShouldAutoRotate ? UIInterfaceOrientationMask.All : UIInterfaceOrientationMask.Portrait;
        }
    }
}
