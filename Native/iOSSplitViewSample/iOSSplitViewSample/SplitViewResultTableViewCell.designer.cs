// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace iOSSplitViewSample
{
    [Register ("SplitViewResultTableViewCell")]
    partial class SplitViewResultTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel barcodeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel symbologyLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (barcodeLabel != null) {
                barcodeLabel.Dispose ();
                barcodeLabel = null;
            }

            if (symbologyLabel != null) {
                symbologyLabel.Dispose ();
                symbologyLabel = null;
            }
        }
    }
}