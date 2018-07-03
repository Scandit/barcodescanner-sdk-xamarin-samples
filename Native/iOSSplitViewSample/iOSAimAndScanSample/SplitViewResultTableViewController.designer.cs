// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iOSSplitViewSample
{
    [Register ("SplitViewResultTableViewController")]
    partial class SplitViewResultTableViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView splitViewResultsTableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (splitViewResultsTableView != null) {
                splitViewResultsTableView.Dispose ();
                splitViewResultsTableView = null;
            }
        }
    }
}