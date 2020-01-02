using System;
using System.Collections.Generic;

using UIKit;
using Foundation;

namespace iOSSplitViewSample
{
    public partial class SplitViewResultTableViewController : UIViewController
    {

        public SplitViewResultTableViewController() : base("SplitViewResultTableViewController", null)
        {
        }

        public SplitViewResultTableViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            splitViewResultsTableView.Source = new SplitViewResultTableSource(new List<SplitViewResult>());
            splitViewResultsTableView.ReloadData();
        }

        public void AddResult(SplitViewResult result)
        {
            (splitViewResultsTableView.Source as SplitViewResultTableSource).AddItem(result);
            splitViewResultsTableView.BeginUpdates();
            splitViewResultsTableView.InsertRows(new[] { NSIndexPath.FromItemSection(0, 0) }, UITableViewRowAnimation.Top);
            splitViewResultsTableView.EndUpdates();
        }

        public void ClearResults()
        {
            (splitViewResultsTableView.Source as SplitViewResultTableSource).ClearItems();
            splitViewResultsTableView.ReloadData();
        }
    }
}

