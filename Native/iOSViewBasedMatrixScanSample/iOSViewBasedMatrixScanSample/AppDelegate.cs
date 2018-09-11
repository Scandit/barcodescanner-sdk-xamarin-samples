using Foundation;
using UIKit;
using ScanditBarcodeScanner.iOS;

namespace iOSViewBasedMatrixScanSample
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            License.SetAppKey("AUHZdocpFGN0PbR7rx3nMvkhqda+RY6QlAwjTJtxa6lkX/2IzSweNmdZapCzWNfXBF65QhRSqI6iOOARdlBJQjZcCZh6KGRD0CkDgJlggtS/1ZKclxyEzVC1b2vhauBUIrRMvtWjbRVm7v1lHfjV8nKQdRRYZzrCj3TxMBUcK8SHBUoSlTq+wW9RXbrn07S3tOsXmrG33WiuGTQGxrL6iznzF2er46MlpjPSclsxOCCd7DWQ9gd4Kuq70XAp7dLqW5ymRGyuszO0XZetOXZd0rpBGqIDw/tScik/KFTnkOF+alkixSAmOoeFyqvr48cdw+iHMLbj3myArgC5uELHnmcDAhNwu4xP6BHJQqTkN4b5GQWE6gMkJxKrdRpN/wqhOYUbftoA7VqJ1MpWyQWxzn6EQ15EAiKiAR1RIASXFXRx1ml/fc4WuhccC/IP69yBYsqkd2C/l77GadLxPuQVq3fTSHFa6RkXxij2+ZGAEgSVxVzHGgH/M6NdWVIu9Fy4o6Pv49CNnosMhHN1HqHIGhGLZsQXrusBSwGZnPmcfW0mgi4CY+hRyoczmG/PAIPdOSf3o4b5cjiivWxirsrnFIR8VEDMUOaNSIcpyQ1ccL/2lamYH/idiIcl4PWDPuCf+lKW0YX4czhzva4of4y0wtT8ceW+05tG293tWJPHcYeNjHX+opIv88R0O1/df8wGzkjLe4rhl8LO1MUksHIP5BRIE5bOkRLUqiJRemdE2GMESm/leQWtmdkOJ73iXL/E+HcS078MXdL5AQ==");
            return true;
        }
    }
}

