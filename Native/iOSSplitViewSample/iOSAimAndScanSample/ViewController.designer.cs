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
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView barcodePickerContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton clearButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView pausedStateOverlayView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint pickerHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView tableViewContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton tapToContinueButton { get; set; }
        [Action ("scanButtonClicked:")]
        partial void scanButtonClicked (Foundation.NSObject sender);

        [Action ("ClearButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ClearButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("TapToContinueButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TapToContinueButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (barcodePickerContainer != null) {
                barcodePickerContainer.Dispose ();
                barcodePickerContainer = null;
            }

            if (clearButton != null) {
                clearButton.Dispose ();
                clearButton = null;
            }

            if (pausedStateOverlayView != null) {
                pausedStateOverlayView.Dispose ();
                pausedStateOverlayView = null;
            }

            if (pickerHeightConstraint != null) {
                pickerHeightConstraint.Dispose ();
                pickerHeightConstraint = null;
            }

            if (tableViewContainer != null) {
                tableViewContainer.Dispose ();
                tableViewContainer = null;
            }

            if (tapToContinueButton != null) {
                tapToContinueButton.Dispose ();
                tapToContinueButton = null;
            }
        }
    }
}