using System;
using System.Collections.Generic;

using UIKit;
using Foundation;

namespace iOSSplitViewSample
{
    public partial class SplitViewResultTableViewController : UIViewController
    {

        public List<SplitViewResult> splitViewResults = new List<SplitViewResult>();

        public SplitViewResultTableViewController() : base("SplitViewResultTableViewController", null)
        {
        }

        public SplitViewResultTableViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            splitViewResultsTableView.Source = new SplitViewResultTableSource(splitViewResults);
            splitViewResultsTableView.ReloadData();
        }

        public void AddResult(SplitViewResult result)
        {
            splitViewResults.Insert(0, result);
            splitViewResultsTableView.Source = new SplitViewResultTableSource(splitViewResults);
            splitViewResultsTableView.BeginUpdates();
            splitViewResultsTableView.InsertRows(new[] { NSIndexPath.FromItemSection(0, 0) }, UITableViewRowAnimation.Top);
            splitViewResultsTableView.EndUpdates();
        }

        public void ClearResults()
        {
            splitViewResults = new List<SplitViewResult>();
            splitViewResultsTableView.Source = new SplitViewResultTableSource(splitViewResults);
            splitViewResultsTableView.ReloadData();
        }
    }
}

